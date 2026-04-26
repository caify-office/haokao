using HaoKao.OpenPlatformService.Domain.Entities;

namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

[AutoMapFrom(typeof(AccessClient))]
public class BrowseClientViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 客户端标识
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    public string ProtocolType { get; set; }

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public List<AccessClientSecretViewModel> ClientSecrets { get; set; }

    /// <summary>
    /// 需要客户端密钥
    /// </summary>
    public bool RequireClientSecret { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    public string ClientName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 客户端 Uri
    /// </summary>
    public string ClientUri { get; set; }

    /// <summary>
    /// 微标 Uri
    /// </summary>
    public string LogoUri { get; set; }

    /// <summary>
    /// 需要同意
    /// </summary>
    public bool RequireConsent { get; set; }

    /// <summary>
    /// 允许记住同意
    /// </summary>
    public bool AllowRememberConsent { get; set; }

    /// <summary>
    /// 始终在身份令牌中包含用户声明
    /// </summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 允许的授权类型
    /// </summary>
    public List<string> AllowedGrantTypes { get; set; }

    /// <summary>
    /// 需要 Pkce
    /// </summary>
    public bool RequirePkce { get; set; }

    /// <summary>
    /// 允许纯文本 Pkce
    /// </summary>
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// 需要请求对象
    /// </summary>
    public bool RequireRequestObject { get; set; }

    /// <summary>
    /// 允许通过浏览器访问令牌
    /// </summary>
    public bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>
    /// 重定向 Uri
    /// </summary>
    public List<string> RedirectUris { get; set; }

    /// <summary>
    /// 注销重定向 Uri
    /// </summary>
    public List<string> PostLogoutRedirectUris { get; set; }

    /// <summary>
    /// 前端通道注销 Uri
    /// </summary>
    public string FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要前端通道注销会话
    /// </summary>
    public bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 后端通道退出 Uri
    /// </summary>
    public string BackChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要后端通道注销会话
    /// </summary>
    public bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 允许离线访问 
    /// </summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 允许的作用域
    /// </summary>
    public List<string> AllowedScopes { get; set; }

    /// <summary>
    /// 身份令牌生命周期
    /// </summary>
    public int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 允许的身份令牌签名算法
    /// </summary>
    public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// 访问令牌生命周期
    /// </summary>
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// 授权码生命周期
    /// </summary>
    public int AuthorizationCodeLifetime { get; set; }

    /// <summary>
    /// 同意生命周期
    /// </summary>
    public int? ConsentLifetime { get; set; }

    /// <summary>
    /// 绝对刷新令牌生命周期
    /// </summary>
    public int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 滚动刷新令牌生命周期
    /// </summary>
    public int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 刷新令牌使用情况
    /// </summary>
    public int RefreshTokenUsage { get; set; }

    /// <summary>
    /// 刷新时更新访问令牌声明
    /// </summary>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 刷新令牌过期
    /// </summary>
    public int RefreshTokenExpiration { get; set; }

    /// <summary>
    /// 访问令牌类型
    /// </summary>
    public int AccessTokenType { get; set; }

    /// <summary>
    /// 启用本地登录
    /// </summary>
    public bool EnableLocalLogin { get; set; }

    /// <summary>
    /// 身份提供程序限制
    /// </summary>
    public List<string> IdentityProviderRestrictions { get; set; }

    /// <summary>
    /// 包括 Jwt 标识
    /// </summary>
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// 声明
    /// </summary>
    public Dictionary<string, string> Claims { get; set; }

    /// <summary>
    /// 始终发送客户端声明
    /// </summary>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 客户端声明前缀
    /// </summary>
    public string ClientClaimsPrefix { get; set; }

    /// <summary>
    /// 配对主体盐
    /// </summary>
    public string PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// 允许跨域来源
    /// </summary>
    public List<string> AllowedCorsOrigins { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// 用户 SSO 生命周期
    /// </summary>
    public int? UserSsoLifetime { get; set; }

    /// <summary>
    /// 用户代码类型
    /// </summary>
    public string UserCodeType { get; set; }

    /// <summary>
    /// 设备代码生命周期
    /// </summary>
    public int DeviceCodeLifetime { get; set; }

    /// <summary>
    /// 域名代理
    /// </summary>
    public List<DomainProxyViewModel> DomainProxies { get; set; }
}

public static class BrowseClientViewModelConvertHelper
{
    public static BrowseClientViewModel ConvertToViewModel(this AccessClient model)
    {
        var clientSecrets = new List<AccessClientSecretViewModel>();
        model.ClientSecrets?.ForEach(x =>
        {
            clientSecrets.Add(new AccessClientSecretViewModel
            {
                Description = x.Description,
                Value = x.Value,
                Expiration = x.Expiration,
                Type = x.Type,
                HashType = x.HashType,
                Created = x.Created
            });
        });

        var viewModel = new BrowseClientViewModel
        {
            Id = model.Id,
            Enabled = model.Enabled,
            ClientId = model.ClientId,
            ProtocolType = model.ProtocolType,
            ClientSecrets = clientSecrets,
            RequireClientSecret = model.RequireClientSecret,
            ClientName = model.ClientName,
            Description = model.Description,
            ClientUri = model.ClientUri,
            LogoUri = model.LogoUri,
            RequireConsent = model.RequireConsent,
            AllowRememberConsent = model.AllowRememberConsent,
            AlwaysIncludeUserClaimsInIdToken = model.AlwaysIncludeUserClaimsInIdToken,
            AllowedGrantTypes = model.AllowedGrantTypes?.Select(x => x.GrantType).ToList(),
            RequirePkce = model.RequirePkce,
            AllowPlainTextPkce = model.AllowPlainTextPkce,
            RequireRequestObject = model.RequireRequestObject,
            AllowAccessTokensViaBrowser = model.AllowAccessTokensViaBrowser,
            RedirectUris = model.RedirectUris?.Select(x => x.RedirectUri).ToList(),
            PostLogoutRedirectUris = model.PostLogoutRedirectUris?.Select(x => x.PostLogoutRedirectUri).ToList(),
            FrontChannelLogoutUri = model.FrontChannelLogoutUri,
            FrontChannelLogoutSessionRequired = model.FrontChannelLogoutSessionRequired,
            BackChannelLogoutUri = model.BackChannelLogoutUri,
            BackChannelLogoutSessionRequired = model.BackChannelLogoutSessionRequired,
            AllowOfflineAccess = model.AllowOfflineAccess,
            AllowedScopes = model.AllowedScopes?.Select(x => x.Scope).ToList(),
            IdentityTokenLifetime = model.IdentityTokenLifetime,
            AllowedIdentityTokenSigningAlgorithms = model.AllowedIdentityTokenSigningAlgorithms?.Select(x => x.SigningAlgorithm).ToList(),
            AccessTokenLifetime = model.AccessTokenLifetime,
            AuthorizationCodeLifetime = model.AuthorizationCodeLifetime,
            ConsentLifetime = model.ConsentLifetime,
            AbsoluteRefreshTokenLifetime = model.AbsoluteRefreshTokenLifetime,
            SlidingRefreshTokenLifetime = model.SlidingRefreshTokenLifetime,
            RefreshTokenUsage = model.RefreshTokenUsage,
            UpdateAccessTokenClaimsOnRefresh = model.UpdateAccessTokenClaimsOnRefresh,
            RefreshTokenExpiration = model.RefreshTokenExpiration,
            AccessTokenType = model.AccessTokenType,
            EnableLocalLogin = model.EnableLocalLogin,
            IdentityProviderRestrictions = model.IdentityProviderRestrictions?.Select(x => x.Provider).ToList(),
            IncludeJwtId = model.IncludeJwtId,
            Claims = model.Claims?.ToDictionary(x => x.Type, x => x.Value),
            AlwaysSendClientClaims = model.AlwaysSendClientClaims,
            ClientClaimsPrefix = model.ClientClaimsPrefix,
            PairWiseSubjectSalt = model.PairWiseSubjectSalt,
            AllowedCorsOrigins = model.AllowedCorsOrigins?.Select(x => x.Origin).ToList(),
            Properties = model.Properties?.ToDictionary(x => x.Key, x => x.Value),
            UserSsoLifetime = model.UserSsoLifetime,
            UserCodeType = model.UserCodeType,
            DeviceCodeLifetime = model.DeviceCodeLifetime,
            DomainProxies = model.DomainProxies?.Select(x => new DomainProxyViewModel
            {
                Domain = x.Domain,
                TenantName = x.TenantName,
                TenantId = x.TenantId,
            }).ToList(),
        };

        return viewModel;
    }
}