using HaoKao.Common.Extensions;
using HaoKao.OpenPlatformService.Domain.Commands.AccessClient;
using HaoKao.OpenPlatformService.Domain.Extensions;

namespace HaoKao.OpenPlatformService.Domain.CommandHandlers;

public class AccessClientCommandHandler(
    IUnitOfWork<AccessClient> uow,
    IAccessClientRepository repository,
    IMediatorHandler bus,
    IDomainProxyRepository domainProxyRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateAccessClientCommand, bool>,
    IRequestHandler<UpdateAccessClientCommand, bool>,
    IRequestHandler<DeleteAccessClientCommand, bool>
{
    private readonly IAccessClientRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IDomainProxyRepository _domainProxyRepository = domainProxyRepository ?? throw new ArgumentNullException(nameof(domainProxyRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateAccessClientCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.ExistEntityAsync(x => x.ClientId == request.ClientId);
        if (exist)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.ClientId, "客户端标识已存在", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        //判断域名唯一
        if (request.DomainProxies.Any())
        {
            var errorInfo = new List<string>();
            //获取当前对象中的重复项
            var repeatDomain = request.DomainProxies
                                      .Select(x => x.Domain)
                                      .GroupBy(x => x)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key).ToList();
            repeatDomain.ForEach(x => { errorInfo.Add($"域名[{x}]重复"); });
            //判断数据库中是否存在
            var domainProxies = await _domainProxyRepository.GetAllAsync(nameof(DomainProxy.Domain));
            var domainProxieNames = domainProxies.Select(x => x.Domain);
            var requestDomainName = request.DomainProxies.Select(x => x.Domain).Distinct().ToList();
            requestDomainName.ForEach(x =>
            {
                if (domainProxieNames.Contains(x))
                {
                    errorInfo.Add($"域名[{x}]已存在");
                }
            });
            if (errorInfo.Any())
            {
                _bus.RaiseEvent(
                    new DomainNotification(request.ClientId, string.Join(",", errorInfo), StatusCodes.Status404NotFound),
                    cancellationToken).Wait(cancellationToken);
                return false;
            }
        }

        var accessClient = new AccessClient
        {
            Enabled = request.Enabled,
            ClientId = request.ClientId,
            ProtocolType = request.ProtocolType,
            ClientSecrets = request.ClientSecrets,
            RequireClientSecret = request.RequireClientSecret,
            ClientName = request.ClientName,
            Description = request.Description,
            ClientUri = request.ClientUri,
            LogoUri = request.LogoUri,
            RequireConsent = request.RequireConsent,
            AllowRememberConsent = request.AllowRememberConsent,
            AlwaysIncludeUserClaimsInIdToken = request.AlwaysIncludeUserClaimsInIdToken,
            AllowedGrantTypes = request.AllowedGrantTypes,
            RequirePkce = request.RequirePkce,
            AllowPlainTextPkce = request.AllowPlainTextPkce,
            RequireRequestObject = request.RequireRequestObject,
            AllowAccessTokensViaBrowser = request.AllowAccessTokensViaBrowser,
            RedirectUris = request.RedirectUris,
            PostLogoutRedirectUris = request.PostLogoutRedirectUris,
            FrontChannelLogoutUri = request.FrontChannelLogoutUri,
            FrontChannelLogoutSessionRequired = request.FrontChannelLogoutSessionRequired,
            BackChannelLogoutUri = request.BackChannelLogoutUri,
            BackChannelLogoutSessionRequired = request.BackChannelLogoutSessionRequired,
            AllowOfflineAccess = request.AllowOfflineAccess,
            AllowedScopes = request.AllowedScopes,
            IdentityTokenLifetime = request.IdentityTokenLifetime,
            AllowedIdentityTokenSigningAlgorithms = request.AllowedIdentityTokenSigningAlgorithms,
            AccessTokenLifetime = request.AccessTokenLifetime,
            AuthorizationCodeLifetime = request.AuthorizationCodeLifetime,
            ConsentLifetime = request.ConsentLifetime,
            AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime,
            SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime,
            RefreshTokenUsage = request.RefreshTokenUsage,
            UpdateAccessTokenClaimsOnRefresh = request.UpdateAccessTokenClaimsOnRefresh,
            RefreshTokenExpiration = request.RefreshTokenExpiration,
            AccessTokenType = request.AccessTokenType,
            EnableLocalLogin = request.EnableLocalLogin,
            IdentityProviderRestrictions = request.IdentityProviderRestrictions,
            IncludeJwtId = request.IncludeJwtId,
            Claims = request.Claims,
            AlwaysSendClientClaims = request.AlwaysSendClientClaims,
            ClientClaimsPrefix = request.ClientClaimsPrefix,
            PairWiseSubjectSalt = request.PairWiseSubjectSalt,
            AllowedCorsOrigins = request.AllowedCorsOrigins,
            Properties = request.Properties,
            UserSsoLifetime = request.UserSsoLifetime,
            UserCodeType = request.UserCodeType,
            DeviceCodeLifetime = request.DeviceCodeLifetime,
            DomainProxies = request.DomainProxies,
        };

        await _repository.AddAsync(accessClient);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);

            accessClient.DomainProxies.ForEach(x =>
            {
                // 创建缓存Key
                var cacheKey = x.Domain.CreateDomainProxyCacheKey();
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
            });

            var clientIdKey = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.ClientId);

            await _bus.RaiseEvent(new RemoveCacheEvent(clientIdKey), cancellationToken);
           await _bus.RemoveListCacheEvent<AccessClient>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateAccessClientCommand request, CancellationToken cancellationToken)
    {
        var accessClient = await _repository.GetById(request.Id);
        if (accessClient == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }


        var client = await _repository.GetAsync(x => x.ClientId == request.ClientId);
        if (client != null && client.Id != request.Id)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.ClientId, "客户端标识已存在", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        //判断域名唯一
        if (request.DomainProxies.Any())
        {
            var errorInfo = new List<string>();
            //获取当前对象中的重复项
            var repeatDomain = request.DomainProxies
                                      .Select(x => x.Domain)
                                      .GroupBy(x => x)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key).ToList();
            repeatDomain.ForEach(x => { errorInfo.Add($"域名[{x}]重复"); });
            //判断数据库中是否存在
            var domainProxies = await _domainProxyRepository.GetAllAsync(nameof(DomainProxy.AccessClientId), nameof(DomainProxy.Domain));
            var domainProxieNames = domainProxies.Where(x => x.AccessClientId != request.Id).Select(x => x.Domain);
            var requestDomainName = request.DomainProxies.Select(x => x.Domain).Distinct().ToList();
            requestDomainName.ForEach(x =>
            {
                if (domainProxieNames.Contains(x))
                {
                    errorInfo.Add($"域名[{x}]已存在");
                }
            });
            if (errorInfo.Any())
            {
                _bus.RaiseEvent(
                    new DomainNotification(request.ClientId, string.Join(",", errorInfo), StatusCodes.Status404NotFound),
                    cancellationToken).Wait(cancellationToken);
                return false;
            }
        }


        accessClient.Enabled = request.Enabled;
        accessClient.ClientId = request.ClientId;
        accessClient.ProtocolType = request.ProtocolType;
        accessClient.ClientSecrets?.Clear();
        accessClient.ClientSecrets = request.ClientSecrets;
        accessClient.RequireClientSecret = request.RequireClientSecret;
        accessClient.ClientName = request.ClientName;
        accessClient.Description = request.Description;
        accessClient.ClientUri = request.ClientUri;
        accessClient.LogoUri = request.LogoUri;
        accessClient.RequireConsent = request.RequireConsent;
        accessClient.AllowRememberConsent = request.AllowRememberConsent;
        accessClient.AlwaysIncludeUserClaimsInIdToken = request.AlwaysIncludeUserClaimsInIdToken;
        accessClient.AllowedGrantTypes?.Clear();
        accessClient.AllowedGrantTypes = request.AllowedGrantTypes;
        accessClient.RequirePkce = request.RequirePkce;
        accessClient.AllowPlainTextPkce = request.AllowPlainTextPkce;
        accessClient.RequireRequestObject = request.RequireRequestObject;
        accessClient.AllowAccessTokensViaBrowser = request.AllowAccessTokensViaBrowser;
        accessClient.RedirectUris?.Clear();
        accessClient.RedirectUris = request.RedirectUris;
        accessClient.PostLogoutRedirectUris?.Clear();
        accessClient.PostLogoutRedirectUris = request.PostLogoutRedirectUris;
        accessClient.FrontChannelLogoutUri = request.FrontChannelLogoutUri;
        accessClient.FrontChannelLogoutSessionRequired = request.FrontChannelLogoutSessionRequired;
        accessClient.BackChannelLogoutUri = request.BackChannelLogoutUri;
        accessClient.BackChannelLogoutSessionRequired = request.BackChannelLogoutSessionRequired;
        accessClient.AllowOfflineAccess = request.AllowOfflineAccess;
        accessClient.AllowedScopes?.Clear();
        accessClient.AllowedScopes = request.AllowedScopes;
        accessClient.IdentityTokenLifetime = request.IdentityTokenLifetime;
        accessClient.AllowedIdentityTokenSigningAlgorithms = request.AllowedIdentityTokenSigningAlgorithms;
        accessClient.AccessTokenLifetime = request.AccessTokenLifetime;
        accessClient.AuthorizationCodeLifetime = request.AuthorizationCodeLifetime;
        accessClient.ConsentLifetime = request.ConsentLifetime;
        accessClient.AbsoluteRefreshTokenLifetime = request.AbsoluteRefreshTokenLifetime;
        accessClient.SlidingRefreshTokenLifetime = request.SlidingRefreshTokenLifetime;
        accessClient.RefreshTokenUsage = request.RefreshTokenUsage;
        accessClient.UpdateAccessTokenClaimsOnRefresh = request.UpdateAccessTokenClaimsOnRefresh;
        accessClient.RefreshTokenExpiration = request.RefreshTokenExpiration;
        accessClient.AccessTokenType = request.AccessTokenType;
        accessClient.EnableLocalLogin = request.EnableLocalLogin;
        accessClient.IdentityProviderRestrictions?.Clear();
        accessClient.IdentityProviderRestrictions = request.IdentityProviderRestrictions;
        accessClient.IncludeJwtId = request.IncludeJwtId;
        accessClient.Claims?.Clear();
        accessClient.Claims = request.Claims;
        accessClient.AlwaysSendClientClaims = request.AlwaysSendClientClaims;
        accessClient.ClientClaimsPrefix = request.ClientClaimsPrefix;
        accessClient.PairWiseSubjectSalt = request.PairWiseSubjectSalt;
        accessClient.AllowedCorsOrigins?.Clear();
        accessClient.AllowedCorsOrigins = request.AllowedCorsOrigins;
        accessClient.Properties?.Clear();
        accessClient.Properties = request.Properties;
        accessClient.UserSsoLifetime = request.UserSsoLifetime;
        accessClient.UserCodeType = request.UserCodeType;
        accessClient.DeviceCodeLifetime = request.DeviceCodeLifetime;
        accessClient.DomainProxies?.Clear();
        accessClient.DomainProxies = request.DomainProxies;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);

            accessClient.DomainProxies.ForEach(x =>
            {
                var cacheKey = x.Domain.CreateDomainProxyCacheKey();
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
            });

            var clientIdKey = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.ClientId);

            await _bus.RaiseEvent(new RemoveCacheEvent(clientIdKey), cancellationToken);
            await _bus.RemoveListCacheEvent<AccessClient>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteAccessClientCommand request, CancellationToken cancellationToken)
    {
        var accessClient = await _repository.GetByIdAsync(request.Id);
        if (accessClient == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(accessClient);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);

            var clientIdKey = GirvsEntityCacheDefaults<AccessClient>.ByIdCacheKey.Create(accessClient.ClientId);

            await _bus.RaiseEvent(new RemoveCacheEvent(clientIdKey), cancellationToken);

            accessClient.DomainProxies.ForEach(x =>
            {
                // 创建缓存Key
                var cacheKey = x.Domain.CreateDomainProxyCacheKey();
                // 将新增的纪录放到缓存中
                _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
            });
           await _bus.RemoveListCacheEvent<AccessClient>(cancellationToken);
        }

        return true;
    }
}