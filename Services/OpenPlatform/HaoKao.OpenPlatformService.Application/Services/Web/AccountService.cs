using Girvs.AuthorizePermission;
using Girvs.Configuration;
using Girvs.Extensions;
using HaoKao.Common;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.OpenPlatformService.Application.Configuration;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.Extension;
using HaoKao.OpenPlatformService.Application.Services.Management;
using HaoKao.OpenPlatformService.Application.Services.Refit;
using HaoKao.OpenPlatformService.Application.ViewModels.Account;
using HaoKao.OpenPlatformService.Application.ViewModels.Identity;
using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using PersistedGrant = HaoKao.OpenPlatformService.Domain.Entities.PersistedGrant;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

/// <summary>
/// 外部平台登陆相关接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize]
public class AccountService(
    IRegisterUserRepository repository,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPhoneCodeService phoneCodeService,
    IIdentityServerInteractionService interaction,
    IEventService events,
    IClientStore clientStore,
    IAuthenticationSchemeProvider schemeProvider,
    ILoginHelpService loginHelpService,
    IStaticCacheManager cacheManager,
    IWeiXinRemoteService remoteService
) : IAccountService
{
    private readonly IRegisterUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IPhoneCodeService _phoneCodeService = phoneCodeService ?? throw new ArgumentNullException(nameof(phoneCodeService));
    private readonly IIdentityServerInteractionService _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
    private readonly IEventService _events = events ?? throw new ArgumentNullException(nameof(events));
    private readonly IClientStore _clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
    private readonly IAuthenticationSchemeProvider _schemeProvider = schemeProvider ?? throw new ArgumentNullException(nameof(schemeProvider));
    private readonly ILoginHelpService _loginHelpService = loginHelpService ?? throw new ArgumentNullException(nameof(loginHelpService));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IWeiXinRemoteService _remoteService = remoteService ?? throw new ArgumentNullException(nameof(remoteService));

    /// <summary>
    /// 获取当前用户的用户信息
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpGet]
    public async Task<RegisterUserDetailViewModel> GetUserInfo()
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(userId.ToString()),
            async () => await _repository.GetByInclude(x => x.Id == userId)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        return registerUser.MapToDto<RegisterUserDetailViewModel>();
    }

    /// <summary>
    /// 按token获取当前用户的用户信息
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpGet, Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
    public Task<RegisterUserDetailViewModel> GetUserInfoByToken()
    {
        return GetUserInfo();
    }

    /// <summary>
    /// 用户换绑手机号码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPatch]
    public async Task<bool> ChangePhone(ChangePhoneViewModel model)
    {
        _phoneCodeService.CheckPhoneCode(EventNotificationMessageType.ChangePhoneNumber, model.NewPhone, model.NewPhoneCode);

        if (await _repository.ExistEntityAsync(x => x.Phone == model.NewPhone)) throw new GirvsException("该手机号已被其他账户绑定");

        var currentUserId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();

        var currentRegisterUser = await _repository.GetAsync(x => x.Id == currentUserId);

        if (currentRegisterUser.Phone != model.OldPhone) throw new GirvsException("旧的手机号码不正确");

        if (currentRegisterUser == null) throw new GirvsException("未找到对应的用户");

        if (currentRegisterUser.UserState == UserState.Disable) throw new GirvsException("当前用户名已被禁用，无法操作");

        var command = new UpdateRegisterUserCommand(
            currentRegisterUser.Id,
            model.NewPhone,
            null,
            currentRegisterUser.UserGender,
            currentRegisterUser.NickName,
            currentRegisterUser.UserState,
            currentRegisterUser.EmailAddress,
            currentRegisterUser.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 根据手机验证码，修改密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch, AllowAnonymous]
    public async Task<bool> ModifyPasswordByCode(ModifyPasswordByCodeViewModel model)
    {
        _phoneCodeService.CheckPhoneCode(EventNotificationMessageType.RetrievePassword, model.Phone, model.PhoneCode);

        var user = await _repository.GetAsync(x => x.Phone == model.Phone) ?? throw new GirvsException("未找到对应的用户");

        if (user.UserState == UserState.Disable) throw new GirvsException("当前用户名已被禁用，无法操作");

        var command = new UpdateRegisterUserCommand(
            user.Id,
            user.Phone,
            model.NewPassword,
            user.UserGender,
            user.NickName,
            user.UserState,
            user.EmailAddress,
            user.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 校验修改密码的手机号码和短信验证码
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public bool CheckModifyPasswordPhoneCode(string phone, string code)
    {
        return _phoneCodeService.CheckPhoneCode(EventNotificationMessageType.RetrievePassword, phone, code);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<bool> ModifyPassword(ModifyPasswordViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();

        var user = await _repository.GetAsync(x => x.Id == userId);

        if (user.Password != model.OldPassword.ToMd5()) throw new GirvsException("输入的旧的密码不正确");

        if (user == null) throw new GirvsException("未找到对应的用户");

        if (user.UserState == UserState.Disable) throw new GirvsException("当前用户名已被禁用，无法操作");

        var command = new UpdateRegisterUserCommand(
            user.Id,
            user.Phone,
            model.NewPassword,
            user.UserGender,
            user.NickName,
            user.UserState,
            user.EmailAddress,
            user.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 修改用户信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<bool> UpdateRegisterInformation(UpdateRegisterViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();

        var user = await _repository.GetAsync(x => x.Id == userId) ?? throw new GirvsException("未找到对应的用户");

        if (user.UserState == UserState.Disable) throw new GirvsException("当前用户名已被禁用，无法操作");

        var command = new UpdateRegisterUserCommand(
            user.Id,
            user.Phone,
            null,
            model.UserGender,
            model.NickName,
            user.UserState,
            model.EmailAddress,
            model.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 获取当前的登陆信息
    /// </summary>
    /// <param name="returnUrl">回跳的地址</param>
    /// <returns></returns>
    [HttpGet]
    public Task<LoginViewModel> GetLoginInfo(string returnUrl)
    {
        return BuildLoginViewModelAsync(returnUrl);
    }

    /*****************************************/
    /* helper APIs for the AccountController */
    /*****************************************/
    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Username = context.LoginHint,
            };

            if (!local)
            {
                vm.ExternalProviders = [new ExternalProvider { AuthenticationScheme = context.IdP },];
            }

            return vm;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
                        .Where(x => x.DisplayName != null)
                        .Select(x => new ExternalProvider
                        {
                            DisplayName = x.DisplayName ?? x.Name,
                            AuthenticationScheme = x.Name
                        }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                {
                    providers = providers.Where(provider =>
                                                    client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }
        }

        return new LoginViewModel
        {
            AllowRememberLogin = true,
            EnableLocalLogin = allowLocal,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint,
            ExternalProviders = providers.ToArray()
        };
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<string> Login(LoginInput model)
    {
        _ = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        var uri = new Uri(WebUtility.UrlDecode(model.ReturnUrl));
        var query = QueryHelpers.ParseQuery(uri.Query);
        var clientId = query["client_id"];
        model.ClientId = clientId;

        var user = await _loginHelpService.LoginRegisterUser(model);
        if (user == null)
        {
            await _events.RaiseAsync(new UserLoginFailureEvent(model.Phone, "invalid credentials", clientId: clientId));
            throw new GirvsException("操作失败！");
        }

        // 底下的 ** 登入 Login ** 需要下面兩個參數 (1) claimsIdentity  (2) authProperties
        // var claims = user.BuildClaims();
        // _ = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // only set explicit expiration here if user chooses "remember me".
        // otherwise we rely upon expiration configured in cookie middleware.
        AuthenticationProperties authProperties = null;
        if (model.RememberLogin)
        {
            authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                RedirectUri = model.ReturnUrl,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
            };
        }

        await EngineContext.Current.HttpContext.SignInAsync(user.ToIdentityServerUser(), authProperties);
        await _events.RaiseAsync(new UserLoginSuccessEvent(user.NickName, user.Id.ToString(), user.NickName, clientId: clientId));

        return model.ReturnUrl;
    }

    /// <summary>
    /// 用户退出登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<LoggedOutViewModel> Logout(LogoutInput input)
    {
        var httpContext = EngineContext.Current.HttpContext;
        var contextUser = httpContext.User;

        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(input.LogoutId);

        // build a model so the logged out page knows what to display
        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = true,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = input.LogoutId,
        };

        if (contextUser?.Identity.IsAuthenticated == true)
        {
            var idp = contextUser.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
            {
                var providerSupportsSignout = await httpContext.GetSchemeSupportsSignOutAsync(idp);
                if (providerSupportsSignout)
                {
                    vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();
                    vm.ExternalAuthenticationScheme = idp;
                }
            }

            // delete local authentication cookie
            await httpContext.SignOutAsync(await GetCookieAuthenticationSchemeAsync());

            // delete persisted grant
            var clientId = logout.ClientId;
            var userId = contextUser.GetSubjectId();
            var sessionId = await EngineContext.Current.Resolve<IUserSession>().GetSessionIdAsync();
            await EngineContext.Current.Resolve<IPersistedGrantRepository>().DeleteAsync(userId, clientId, sessionId);

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(contextUser.GetSubjectId(), contextUser.GetDisplayName()));
        }

        return vm;
    }

    private static async Task<string> GetCookieAuthenticationSchemeAsync()
    {
        var context = EngineContext.Current.HttpContext;
        var requiredService = context.RequestServices.GetRequiredService<IdentityServerOptions>();
        if (requiredService.Authentication.CookieAuthenticationScheme != null)
        {
            return requiredService.Authentication.CookieAuthenticationScheme;
        }

        var provider = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
        var scheme = await provider.GetDefaultAuthenticateSchemeAsync()
                  ?? throw new InvalidOperationException("No DefaultAuthenticateScheme found or no CookieAuthenticationScheme configured on IdentityServerOptions.");
        return scheme.Name;
    }

    /// <summary>
    /// 是否多设备登录
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="userId"></param>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Task<bool> IsMultipleDeviceLogin(string clientId, string userId, string sessionId)
    {
        return EngineContext.Current.Resolve<IPersistedGrantRepository>().IsConsumedAsync(userId, clientId, sessionId);
    }

    /// <summary>
    /// 获取相关的错误信息
    /// </summary>
    /// <param name="errorId"></param>
    /// <returns></returns>
    [HttpGet("{errorId}")]
    [AllowAnonymous]
    public async Task<ErrorViewModel> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        // retrieve error details from identityserver
        var message = await _interaction.GetErrorContextAsync(errorId);
        if (message != null)
        {
            vm.Error = message;
        }

        return vm;
    }

    #region Consent 相关的私有方法

    private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
    {
        var result = new ProcessConsentResult();

        // validate return url is still valid
        var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        if (request == null) return result;

        ConsentResponse grantedConsent = null;

        // user clicked 'no' - send back the standard 'access_denied' response
        if (model?.Button == "no")
        {
            grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

            // emit event
            await _events.RaiseAsync(new ConsentDeniedEvent(EngineContext.Current.HttpContext.User.GetSubjectId(),
                                                            request.Client.ClientId, request.ValidatedResources.RawScopeValues));
        }
        // user clicked 'yes' - validate the data
        else if (model?.Button == "yes")
        {
            // if the user consented to some scope, build the response model
            if (model.ScopesConsented != null && model.ScopesConsented.Any())
            {
                var scopes = model.ScopesConsented;
                if (ConsentOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = model.Description
                };

                // emit event
                await _events.RaiseAsync(new ConsentGrantedEvent(EngineContext.Current.HttpContext.User.GetSubjectId(),
                                                                 request.Client.ClientId, request.ValidatedResources.RawScopeValues,
                                                                 grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
            }
            else
            {
                result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
            }
        }
        else
        {
            result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identityserver
            await _interaction.GrantConsentAsync(request, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            result.RedirectUri = model.ReturnUrl;
            result.Client = request.Client;
        }
        else
        {
            // we need to redisplay the consent UI
            result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
        }

        return result;
    }

    private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
    {
        var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (request != null)
        {
            return CreateConsentViewModel(model, returnUrl, request);
        }

        return null;
    }

    private ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }

    private ConsentViewModel CreateConsentViewModel(
        ConsentInputModel model, string returnUrl,
        AuthorizationRequest request)
    {
        var vm = new ConsentViewModel
        {
            RememberConsent = model?.RememberConsent ?? true,
            ScopesConsented = model?.ScopesConsented ?? [],
            Description = model?.Description,

            ReturnUrl = returnUrl,

            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources
                                   .Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

        var apiScopes = new List<ScopeViewModel>();
        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
            if (apiScope != null)
            {
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope,
                                                   vm.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                apiScopes.Add(scopeVm);
            }
        }

        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(
                              vm.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) ||
                              model == null));
        }

        vm.ApiScopes = apiScopes;

        return vm;
    }

    private ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        var displayName = apiScope.DisplayName ?? apiScope.Name;
        if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        {
            displayName += ":" + parsedScopeValue.ParsedParameter;
        }

        return new ScopeViewModel
        {
            Value = parsedScopeValue.RawValue,
            DisplayName = displayName,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    #endregion

    /// <summary>
    /// 获取同意条款
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet("{returnUrl}")]
    [AllowAnonymous]
    public async Task<ConsentViewModel> GetConsentContent(string returnUrl)
    {
        return await BuildViewModelAsync(returnUrl);
    }

    /// <summary>
    /// 用户点击相关的同意条款
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ConsentViewModel> Consent([FromBody] ConsentInputModel model)
    {
        var result = await ProcessConsent(model);

        return result.ViewModel;
    }

    /// <summary>
    /// 设备相关的页面，暂时不做实现
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public Task<DeviceAuthorizationViewModel> UserCodeCapture(string userCode)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 判断当前手机号码是否已经注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<bool> IsRegister(IsRegisterInput input)
    {
        return _repository.ExistEntityAsync(x => x.Phone == input.Phone);
    }

    /// <summary>
    /// 用户注册、销户
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> CancelAccount()
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var command = new DeleteRegisterUserCommand(userId);
        await _bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 根据UserId获取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId:guid}")]
    [Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
    public Task<BrowseRegisterUserViewModel> GetRegisterUser(Guid userId)
    {
        var service = EngineContext.Current.Resolve<IRegisterUserService>();
        return service.Get(userId);
    }

    #region 微信相关接口

    /// <summary>
    /// 根据code换取手机号码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public async Task<string> GetWeiXinUserPhone(Guid id, string code)
    {
        var token = await GetWeiXinAccessToken(id);
        var json = await _remoteService.GetUserPhoneNumber(token.Access_token, new WeiXinUserPhoneNumberBody(code));
        var result = JsonConvert.DeserializeObject<WeiXinUserPhoneNumberViewModel>(json);
        while (result.Errcode == 40001)
        {
            // token过期
            var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create($"{nameof(GetWeiXinAccessToken)}:{id}");
            await _cacheManager.RemoveAsync(cacheKey);
            token = await GetWeiXinAccessToken(id);
            json = await _remoteService.GetUserPhoneNumber(token.Access_token, new WeiXinUserPhoneNumberBody(code));
            result = JsonConvert.DeserializeObject<WeiXinUserPhoneNumberViewModel>(json);
        }
        if (result.Errcode != 0)
        {
            throw new GirvsException($"{result.Errcode}: {result.Errmsg}");
        }
        return result.Phone_info.PhoneNumber;
    }

    /// <summary>
    /// 获取微信用户OpenId
    /// </summary>
    /// <param name="id"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public Task<string> GetWeiXinOpenId(Guid id, string code)
    {
        var weixin = GetWeiXinAppConfig(id);
        return _remoteService.Code2Session(weixin.AppId, weixin.AppSecret, code);
    }

    /// <summary>
    /// 获取加密scheme码
    /// 该接口用于获取小程序 scheme 码，适用于短信、邮件、外部网页、微信内等拉起小程序的业务场景
    /// </summary>
    /// <param name="id"></param>
    /// <param name="jumpWxa"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<string> GenerateScheme(Guid id, [FromQuery] WeiXinGenerateSchemeJumpWxa jumpWxa)
    {
        var token = await GetWeiXinAccessToken(id);
        var body = new WeiXinGenerateSchemeBody { jump_wxa = jumpWxa };
        var content = JsonConvert.SerializeObject(body);

        var resp = await _remoteService.GenerateScheme(token.Access_token, content);
        while (resp.errcode == 40001)
        {
            // token过期
            var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create($"{nameof(GetWeiXinAccessToken)}:{id}");
            await _cacheManager.RemoveAsync(cacheKey);
            token = await GetWeiXinAccessToken(id);
            resp = await _remoteService.GenerateScheme(token.Access_token, content);
        }

        if (resp.errcode != 0)
        {
            throw new GirvsException($"{resp.errcode}: {resp.errmsg}");
        }

        return resp.openlink;
    }

    /// <summary>
    /// 查询scheme码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPost, Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
    public async Task<WeiXinQuerySchemeResponse> QueryScheme(Guid id, [FromBody] WeiXinQuerySchemeBody body)
    {
        var token = await GetWeiXinAccessToken(id);
        var content = JsonConvert.SerializeObject(body);
        return await _remoteService.QueryScheme(token.Access_token, content);
    }

    /// <param name="id"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost, Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
    public async Task BindWeiXin(Guid id, string code)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        var user = await _repository.GetAsync(x => x.Id == userId) ?? throw new GirvsException("未找到对应的用户");

        var weixin = GetWeiXinAppConfig(id);
        var json = await _remoteService.Code2Session(weixin.AppId, weixin.AppSecret, code);
        var result = JsonConvert.DeserializeObject<WeiXinCode2SessionResponse>(json);
        if (result.errcode != 0)
        {
            throw new GirvsException($"{result.errcode}: {result.errmsg}");
        }
        if (string.IsNullOrEmpty(result.unionid))
        {
            throw new GirvsException("未获取到unionid");
        }

        // 发送绑定微信命令
        var command = new BindExternalUserCommand(
            user.Phone,
            new(
                "Weixin",
                result.unionid,
                user.NickName,
                user.UserGender,
                user.EmailAddress,
                user.HeadImage,
                null));
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
        }
    }

    [HttpGet, AllowAnonymous]
    public Task<string> WeiXinCode2Session(Guid id, string code)
    {
        var weixin = GetWeiXinAppConfig(id);
        return _remoteService.Code2Session(weixin.AppId, weixin.AppSecret, code);
    }

    private static ExternalPlatformConfig.WeixinApp GetWeiXinAppConfig(Guid id)
    {
        var config = Singleton<AppSettings>.Instance.Get<ExternalPlatformConfig>();
        return config.WeixinApps.FirstOrDefault(x => x.Id == id) ?? throw new GirvsException("invalid clientId");
    }

    private Task<WeiXinAccessTokenViewModel> GetWeiXinAccessToken(Guid id, bool refresh = false)
    {
        // var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create($"{nameof(GetWeiXinAccessToken)}:{id}", cacheTime: 119);
        // return _cacheManager.GetAsync(cacheKey, () => GetWechatRemoteTokenResponse(id, refresh));
        return GetWechatRemoteTokenResponse(id, refresh);
    }

    private async Task<WeiXinAccessTokenViewModel> GetWechatRemoteTokenResponse(Guid id, bool refresh = false)
    {
        var weixin = GetWeiXinAppConfig(id);
        var json = await _remoteService.GetAccessTokenAsync(new WeiXinAccessTokenBody(weixin.AppId, weixin.AppSecret, force_refresh: refresh));
        return JsonConvert.DeserializeObject<WeiXinAccessTokenViewModel>(json);
    }

    [HttpGet, Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
    public object ClaimDict()
    {
        return EngineContext.Current.HttpContext.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
    }

    #endregion

    /// <summary>
    /// 获取微信公众号二维码
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<dynamic> GetWeiXinOffiAccountQrCode()
    {
        var body = new
        {
            expire_seconds = 600,
            action_name = "QR_STR_SCENE",
            action_info = new
            {
                scene = new
                {
                    scene_str = Guid.NewGuid().ToString()
                }
            }
        };
        var bodyJson = JsonConvert.SerializeObject(body);
        var accessToken = await GetHaoKaoWeiXinOffiAccountAccessToken();
        var result = await _remoteService.GetOffiAccountQrCode(accessToken, bodyJson);
        return new { body.action_info.scene.scene_str, result.ticket, result.expire_seconds, result.url };
    }

    /// <summary>
    /// 微信公众号接收事件推送
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost, AllowAnonymous]
    public async Task<dynamic> WeiXinOffiAccountReceiveMessage([FromBody] WeiXinOffiAccountQrCodeSubscribeEvent model)
    {
        var openId = model.FromUserName;
        var accessToken = await GetHaoKaoWeiXinOffiAccountAccessToken();
        var userInfo = await _remoteService.UserInfo(accessToken, openId);

        // 根据unionId到系统中查询用户信息
        var externalUser = await _repository.GetByExternalIdentity("Weixin", userInfo.unionid);

        // 保存二维码的ticket到缓存中
        var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(model.EventKey.RemovePreFix("qrscene_"));
        await _cacheManager.SetAsync(cacheKey, new ScanQrCodeUserInfo { unionid = userInfo.unionid, phone = externalUser?.RegisterUser?.Phone });

        return userInfo;
    }

    private static async Task<string> GetHaoKaoWeiXinOffiAccountAccessToken()
    {
        var uri = new Uri("https://op.haokao123.com/Wechat/Extension/GetAcessToken?type=1");
        using var response = await HttpClientFactory.Create().GetAsync(uri);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 微信公众号扫描二维码轮询
    /// </summary>
    /// <param name="scene_str"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public ScanQrCodeUserInfo ScanQrCodeLoop(string scene_str)
    {
        var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(scene_str);
        return _cacheManager.Get<ScanQrCodeUserInfo>(cacheKey, () => null);
    }

    [HttpGet, AllowAnonymous]
    public async Task AddToWhiteList(string subjectId)
    {
        var cacheTime = (int)TimeSpan.FromDays(365 * 10).TotalMinutes;
        var cacheKey = GirvsEntityCacheDefaults<PersistedGrant>.QueryCacheKey.Create("whitelist", cacheTime: cacheTime);
        var hashSet = await _cacheManager.GetAsync(cacheKey, () => Task.FromResult(new HashSet<string>()));
        hashSet.Add(subjectId);
        await _cacheManager.SetAsync(cacheKey, hashSet);
    }
}

public record ScanQrCodeUserInfo
{
    public string unionid { get; init; }

    public string phone { get; init; }
}