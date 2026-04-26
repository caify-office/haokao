using IdentityServer4.Models;

namespace HaoKao.OpenPlatformService.Domain.Entities;

public class AccessClient : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 客户端标识
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    public string ProtocolType { get; set; } = "oidc";

    /// <summary>
    /// 客户端密钥
    /// </summary>
    public List<AccessClientSecret> ClientSecrets { get; set; } = [];

    /// <summary>
    /// 需要客户端密钥
    /// </summary>
    public bool RequireClientSecret { get; set; } = true;

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
    public bool RequireConsent { get; set; } = false;

    /// <summary>
    /// 允许记住同意
    /// </summary>
    public bool AllowRememberConsent { get; set; } = true;

    /// <summary>
    /// 始终在身份令牌中包含用户声明
    /// </summary>
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 允许的授权类型
    /// </summary>
    public List<AccessClientGrantType> AllowedGrantTypes { get; set; } = [];

    /// <summary>
    /// 需要 Pkce
    /// </summary>
    public bool RequirePkce { get; set; } = true;

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
    public List<AccessClientRedirectUri> RedirectUris { get; set; } = [];

    /// <summary>
    /// 注销重定向 Uri
    /// </summary>
    public List<AccessClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; } = [];

    /// <summary>
    /// 前端通道注销 Uri
    /// </summary>
    public string FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要前端通道注销会话
    /// </summary>
    public bool FrontChannelLogoutSessionRequired { get; set; } = true;

    /// <summary>
    /// 后端通道退出 Uri
    /// </summary>
    public string BackChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要后端通道注销会话
    /// </summary>
    public bool BackChannelLogoutSessionRequired { get; set; } = true;

    /// <summary>
    /// 允许离线访问
    /// </summary>
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 允许的作用域
    /// </summary>
    public List<AccessClientScope> AllowedScopes { get; set; } = [];

    /// <summary>
    /// 身份令牌生命周期
    /// </summary>
    public int IdentityTokenLifetime { get; set; } = 300;

    /// <summary>
    /// 允许的身份令牌签名算法
    /// </summary>
    public List<AccessClientSigningAlgorithm> AllowedIdentityTokenSigningAlgorithms { get; set; } = [];

    /// <summary>
    /// 访问令牌生命周期
    /// </summary>
    public int AccessTokenLifetime { get; set; } = 3600;

    /// <summary>
    /// 授权码生命周期
    /// </summary>
    public int AuthorizationCodeLifetime { get; set; } = 300;

    /// <summary>
    /// 同意生命周期
    /// </summary>
    public int? ConsentLifetime { get; set; } = null;

    /// <summary>
    /// 绝对刷新令牌生命周期
    /// </summary>
    public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

    /// <summary>
    /// 滚动刷新令牌生命周期
    /// </summary>
    public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

    /// <summary>
    /// 刷新令牌使用情况
    /// </summary>
    public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;

    /// <summary>
    /// 刷新时更新访问令牌声明
    /// </summary>
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 刷新令牌过期
    /// </summary>
    public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;

    /// <summary>
    /// 访问令牌类型
    /// </summary>
    public int AccessTokenType { get; set; } = 0; // AccessTokenType.Jwt;

    /// <summary>
    /// 启用本地登录
    /// </summary>
    public bool EnableLocalLogin { get; set; } = true;

    /// <summary>
    /// 身份提供程序限制
    /// </summary>
    public List<AccessClientIdPRestriction> IdentityProviderRestrictions { get; set; } = [];

    /// <summary>
    /// 包括 Jwt 标识
    /// </summary>
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// 声明
    /// </summary>
    public List<AccessClientClaim> Claims { get; set; } = [];

    /// <summary>
    /// 始终发送客户端声明
    /// </summary>
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 客户端声明前缀
    /// </summary>
    public string ClientClaimsPrefix { get; set; } = "client_";

    /// <summary>
    /// 配对主体盐
    /// </summary>
    public string PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// 允许跨域来源
    /// </summary>
    public List<AccessClientCorsOrigin> AllowedCorsOrigins { get; set; } = [];

    /// <summary>
    /// 属性
    /// </summary>
    public List<AccessClientProperty> Properties { get; set; } = [];

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
    public int DeviceCodeLifetime { get; set; } = 300;

    /// <summary>
    /// 域名代理
    /// </summary>
    public List<DomainProxy> DomainProxies { get; set; } = [];

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}