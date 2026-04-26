using AspNet.Security.OAuth.Weixin;
using Girvs.Configuration;
using Girvs.Extensions;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.Configuration;
using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;
using HaoKao.OpenPlatformService.Application.ViewModels.Identity;
using HaoKao.OpenPlatformService.Domain.Repositories;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

/// <summary>
/// 外部平台登陆相关接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[AllowAnonymous]
public class ExternalIdentityService(
    IAuthenticationSchemeProvider schemeProvider,
    IIdentityServerInteractionService identityServerInteractionService,
    IRegisterUserRepository registerUserRepository
) : IExternalIdentityService
{
    private readonly IAuthenticationSchemeProvider _schemeProvider = schemeProvider ?? throw new ArgumentNullException(nameof(schemeProvider));
    private readonly IIdentityServerInteractionService _interaction = identityServerInteractionService ?? throw new ArgumentNullException(nameof(identityServerInteractionService));
    private readonly IRegisterUserRepository _registerUserRepository = registerUserRepository ?? throw new ArgumentNullException(nameof(registerUserRepository));

    /// <summary>
    /// 获取所有第三方的认证平台列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<ExternalProvider>> GetExternalSchemeAsync()
    {
        var context = await _interaction.GetAuthorizationContextAsync(string.Empty);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            return [];
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes.Where(x => x.DisplayName != null)
                               .Select(x => new ExternalProvider
                               {
                                   DisplayName = x.DisplayName ?? x.Name,
                                   AuthenticationScheme = x.Name
                               }).ToList();

        return providers;
    }

    /// <summary>
    /// 根据Scheme获取第三方的认证平台列表的Url地址
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet]
    public async Task<string> GetOAuthUrlByScheme([FromQuery] OAuthContextViewModel model)
    {
        if (string.IsNullOrEmpty(model.ReturnUrl)) model.ReturnUrl = "~/";

        var props = new AuthenticationProperties
        {
            RedirectUri = model.ReturnUrl,
            Items =
            {
                { "scheme", model.Scheme },
            }
        };

        var haoKaoHandler = await GetHaoKaoAuthenticationHandler(model.Scheme);
        if (haoKaoHandler != null)
        {
            return haoKaoHandler.GetRedirectUrl(props, model.CallBackServerUrl);
        }

        throw new GirvsException("获取外部认证错误");
    }

    private async Task<IHaoKaoAuthenticationHandler> GetHaoKaoAuthenticationHandler(string scheme)
    {
        var schemeProvider = await _schemeProvider.GetSchemeAsync(scheme);
        if (schemeProvider == null) return null;
        var requestHttpContext = EngineContext.Current.HttpContext;

        var handler =
            (requestHttpContext.RequestServices.GetService(schemeProvider.HandlerType) ??
             ActivatorUtilities.CreateInstance(requestHttpContext.RequestServices, schemeProvider.HandlerType)) as
            IAuthenticationHandler;

        if (handler != null)
        {
            await handler.InitializeAsync(schemeProvider, requestHttpContext);
        }

        return handler as IHaoKaoAuthenticationHandler;
    }

    /// <summary>
    /// 后台回调地址
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpGet]
    public Task<ExternalAuthenticationViewModel> OAuthCallBack()
    {
        return GetExternalUserInfoIsRegister();
    }

    [NonAction]
    public async Task<(string, ExternalUser)> GetExternalUserInfo()
    {
        var httpContext = EngineContext.Current.HttpContext;

        var result = await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        if (result.Succeeded != true)
        {
            throw new GirvsException("外部认证错误");
        }

        var scheme = result.Properties?.Items["scheme"];
        if (scheme.IsNullOrEmpty())
        {
            throw new GirvsException("未知外部认证");
        }

        var haoKaoHandler = await GetHaoKaoAuthenticationHandler(scheme);
        if (haoKaoHandler == null)
        {
            throw new GirvsException("外部认证处理程序报错");
        }

        var externalUser = haoKaoHandler.GetCurrentAuthenticationUniqueIdentifier(result.Principal.Claims);

        externalUser.Schemem = scheme;

        return (scheme, externalUser);
    }

    /// <summary>
    /// 根据当前第三方登陆的信息，获取注册用户相关信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ExternalAuthenticationViewModel> GetExternalUserInfoIsRegister()
    {
        var (scheme, externalUser) = await GetExternalUserInfo();

        var isRegister = await _registerUserRepository.ExistByExternalIdentity(scheme, externalUser.UniqueIdentifier);

        return new ExternalAuthenticationViewModel
        {
            Scheme = scheme,
            ExternalUser = externalUser,
            IsRegister = isRegister
        };
    }

    /// <summary>
    /// 微信小程序以及App 获取到微信相关的用户信息后，根据unionId判断当前用户是否已经注册
    /// </summary>
    /// <param name="unionId"></param>
    /// <returns></returns>
    [HttpGet("{unionId}")]
    public Task<bool> WechatAppMiniProgramUserIsRegister(string unionId)
    {
        return _registerUserRepository.ExistByExternalIdentity(WeixinAuthenticationDefaults.AuthenticationScheme, unionId);
    }

    /// <summary>
    /// 通过Code 完成外部认证等
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    public async Task<ExternalAuthenticationViewModel> CreateExternalTicketByCodeAsync([FromBody] ExternalCodeAuthenticationOptions options)
    {
        var haoKaoHandler = await GetHaoKaoAuthenticationHandler(options.Scheme);
        if (haoKaoHandler == null)
        {
            throw new GirvsException("外部认证处理程序报错");
        }

        if (options.Scheme == WeixinAuthenticationDefaults.AuthenticationScheme)
        {
            var config = Singleton<AppSettings>.Instance.ModuleConfigurations[nameof(ExternalPlatformConfig)] as ExternalPlatformConfig;
            var weixin = config.WeixinApps.FirstOrDefault(x => x.AppId == options.ClientId);
            if (weixin == null)
            {
                throw new GirvsException("invalid clientId");
            }
            options.ClientId = weixin.AppId;
            options.ClientSecret = weixin.AppSecret;
        }

        var externalUser = await haoKaoHandler.CreateExternalTicketAsync(options);

        if (externalUser == null)
        {
            throw new GirvsException("认证处理程序报错");
        }

        var isRegister = await _registerUserRepository.ExistByExternalIdentity(options.Scheme, externalUser.UniqueIdentifier);

        return new ExternalAuthenticationViewModel
        {
            Scheme = options.Scheme,
            ExternalUser = externalUser,
            IsRegister = isRegister
        };
    }
}