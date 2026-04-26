using HaoKao.OpenPlatformService.Domain.Commands.AccessClient;
using HaoKao.OpenPlatformService.Domain.Entities;

namespace HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

public class CreateClientViewModel
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [DisplayName("是否启用")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 客户端标识
    /// </summary>
    [DisplayName("客户端标识")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(200, ErrorMessage = "{0}长度不能大于{1}")]
    public string ClientId { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    [DisplayName("协议类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(200, ErrorMessage = "{0}长度不能大于{1}")]
    public string ProtocolType { get; set; }

    /// <summary>
    /// 客户端密钥
    /// </summary>
    [DisplayName("客户端密钥")]
    public List<AccessClientSecretViewModel> ClientSecrets { get; set; }

    /// <summary>
    /// 需要客户端密钥
    /// </summary>
    [DisplayName("需要客户端密钥")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool RequireClientSecret { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    [DisplayName("客户端名称")]
    [MaxLength(200, ErrorMessage = "{0}长度不能大于{1}")]
    public string ClientName { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [DisplayName("描述")]
    [MaxLength(1000, ErrorMessage = "{0}长度不能大于{1}")]
    public string Description { get; set; }

    /// <summary>
    /// 客户端 Uri
    /// </summary>
    [DisplayName("客户端 Uri ")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string ClientUri { get; set; }

    /// <summary>
    /// 微标 Uri
    /// </summary>
    [DisplayName("微标 Uri ")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string LogoUri { get; set; }

    /// <summary>
    /// 需要同意
    /// </summary>
    [DisplayName("需要同意")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool RequireConsent { get; set; }

    /// <summary>
    /// 允许记住同意
    /// </summary>
    [DisplayName("允许记住同意")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AllowRememberConsent { get; set; }

    /// <summary>
    /// 始终在身份令牌中包含用户声明
    /// </summary>
    [DisplayName("始终在身份令牌中包含用户声明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

    /// <summary>
    /// 允许的授权类型
    /// </summary>
    [DisplayName("允许的授权类型")]
    public List<string> AllowedGrantTypes { get; set; }

    /// <summary>
    /// 需要 Pkce
    /// </summary>
    [DisplayName("需要 Pkce")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool RequirePkce { get; set; }

    /// <summary>
    /// 允许纯文本 Pkce
    /// </summary>
    [DisplayName("允许纯文本 Pkce")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AllowPlainTextPkce { get; set; }

    /// <summary>
    /// 需要请求对象
    /// </summary>
    [DisplayName("需要请求对象")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool RequireRequestObject { get; set; }

    /// <summary>
    /// 允许通过浏览器访问令牌
    /// </summary>
    [DisplayName("允许通过浏览器访问令牌")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AllowAccessTokensViaBrowser { get; set; }

    /// <summary>
    /// 重定向 Uri
    /// </summary>
    [DisplayName("重定向 Uri")]
    public List<string> RedirectUris { get; set; }

    /// <summary>
    /// 注销重定向 Uri
    /// </summary>
    [DisplayName("注销重定向 Uri ")]
    public List<string> PostLogoutRedirectUris { get; set; }

    /// <summary>
    /// 前端通道注销 Uri
    /// </summary>
    [DisplayName(" 前端通道注销 Uri")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string FrontChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要前端通道注销会话
    /// </summary>
    [DisplayName("需要前端通道注销会话")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool FrontChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 后端通道退出 Uri
    /// </summary>
    [DisplayName("后端通道退出 Uri ")]
    [MaxLength(2000, ErrorMessage = "{0}长度不能大于{1}")]
    public string BackChannelLogoutUri { get; set; }

    /// <summary>
    /// 需要后端通道注销会话
    /// </summary>
    [DisplayName("需要后端通道注销会话")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool BackChannelLogoutSessionRequired { get; set; }

    /// <summary>
    /// 允许离线访问 
    /// </summary>
    [DisplayName("允许离线访问 ")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AllowOfflineAccess { get; set; }

    /// <summary>
    /// 允许的作用域
    /// </summary>
    [DisplayName("允许的作用域")]
    public List<string> AllowedScopes { get; set; }

    /// <summary>
    /// 身份令牌生命周期
    /// </summary>
    [DisplayName("身份令牌生命周期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int IdentityTokenLifetime { get; set; }

    /// <summary>
    /// 允许的身份令牌签名算法
    /// </summary>
    [DisplayName("允许的身份令牌签名算法")]
    public List<string> AllowedIdentityTokenSigningAlgorithms { get; set; }

    /// <summary>
    /// 访问令牌生命周期
    /// </summary>
    [DisplayName("访问令牌生命周期 ")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int AccessTokenLifetime { get; set; }

    /// <summary>
    /// 授权码生命周期
    /// </summary>
    [DisplayName("授权码生命周期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int AuthorizationCodeLifetime { get; set; } = 300;

    /// <summary>
    /// 同意生命周期
    /// </summary>
    [DisplayName("同意生命周期")]
    public int? ConsentLifetime { get; set; }

    /// <summary>
    /// 绝对刷新令牌生命周期
    /// </summary>
    [DisplayName("绝对刷新令牌生命周期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int AbsoluteRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 滚动刷新令牌生命周期
    /// </summary>
    [DisplayName("滚动刷新令牌生命周期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int SlidingRefreshTokenLifetime { get; set; }

    /// <summary>
    /// 刷新令牌使用情况
    /// </summary>
    [DisplayName(" 刷新令牌使用情况")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int RefreshTokenUsage { get; set; }

    /// <summary>
    /// 刷新时更新访问令牌声明
    /// </summary>
    [DisplayName("刷新时更新访问令牌声明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

    /// <summary>
    /// 刷新令牌过期
    /// </summary>
    [DisplayName("刷新令牌过期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int RefreshTokenExpiration { get; set; }

    /// <summary>
    /// 访问令牌类型
    /// </summary>
    [DisplayName("访问令牌类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int AccessTokenType { get; set; }

    /// <summary>
    /// 启用本地登录
    /// </summary>
    [DisplayName("启用本地登录")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool EnableLocalLogin { get; set; }

    /// <summary>
    /// 身份提供程序限制
    /// </summary>
    [DisplayName("身份提供程序限制")]
    public List<string> IdentityProviderRestrictions { get; set; }

    /// <summary>
    /// 包括 Jwt 标识
    /// </summary>
    [DisplayName("包括 Jwt 标识")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IncludeJwtId { get; set; }

    /// <summary>
    /// 声明
    /// </summary>
    [DisplayName("声明")]
    public Dictionary<string, string> Claims { get; set; }

    /// <summary>
    /// 始终发送客户端声明
    /// </summary>
    [DisplayName("始终发送客户端声明")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool AlwaysSendClientClaims { get; set; }

    /// <summary>
    /// 客户端声明前缀
    /// </summary>
    [DisplayName("客户端声明前缀")]
    [MaxLength(200, ErrorMessage = "{0}长度不能大于{1}")]
    public string ClientClaimsPrefix { get; set; }

    /// <summary>
    /// 配对主体盐
    /// </summary>
    [DisplayName(" 配对主体盐")]
    [MaxLength(200, ErrorMessage = "{0}长度不能大于{1}")]
    public string PairWiseSubjectSalt { get; set; }

    /// <summary>
    /// 允许跨域来源
    /// </summary>
    [DisplayName("允许跨域来源")]
    public List<string> AllowedCorsOrigins { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    [DisplayName("属性")]
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// 用户 SSO 生命周期
    /// </summary>
    [DisplayName("用户 SSO 生命周期")]
    public int? UserSsoLifetime { get; set; }

    /// <summary>
    /// 用户代码类型
    /// </summary>
    [DisplayName("用户代码类型")]
    [MaxLength(100, ErrorMessage = "{0}长度不能大于{1}")]
    public string UserCodeType { get; set; }

    /// <summary>
    /// 设备代码生命周期
    /// </summary>
    [DisplayName("设备代码生命周期")]
    [Required(ErrorMessage = "{0}不能为空")]
    public int DeviceCodeLifetime { get; set; }

    /// <summary>
    /// 域名代理
    /// </summary>
    [DisplayName("域名代理")]
    public List<DomainProxyViewModel> DomainProxies { get; set; }
}

public static class ConvertHelper
{
    public static CreateAccessClientCommand ConvertToCommand(this CreateClientViewModel model)
    {
        var clientSecrets = new List<AccessClientSecret>();
        model.ClientSecrets?.ForEach(x =>
        {
            clientSecrets.Add(new AccessClientSecret
            {
                Description = x.Description,
                Value = x.Value,
                Expiration = x.Expiration,
                Type = x.Type,
                HashType = x.HashType,
                Created = x.Created
            });
        });

        var allowedGrantTypes = new List<AccessClientGrantType>();
        model.AllowedGrantTypes?.ForEach(x =>
        {
            allowedGrantTypes.Add(new AccessClientGrantType
            {
                GrantType = x
            });
        });

        var redirectUris = new List<AccessClientRedirectUri>();
        model.RedirectUris?.ForEach(x =>
        {
            redirectUris.Add(new AccessClientRedirectUri
            {
                RedirectUri = x
            });
        });

        var postLogoutRedirectUris = new List<AccessClientPostLogoutRedirectUri>();
        model.PostLogoutRedirectUris?.ForEach(x =>
        {
            postLogoutRedirectUris.Add(new AccessClientPostLogoutRedirectUri
            {
                PostLogoutRedirectUri = x
            });
        });

        var allowedScopes = new List<AccessClientScope>();
        model.AllowedScopes?.ForEach(x =>
        {
            allowedScopes.Add(new AccessClientScope
            {
                Scope = x
            });
        });

        var allowedIdentityTokenSigningAlgorithms = new List<AccessClientSigningAlgorithm>();
        model.AllowedIdentityTokenSigningAlgorithms?.ForEach(x =>
        {
            allowedIdentityTokenSigningAlgorithms.Add(new AccessClientSigningAlgorithm
            {
                SigningAlgorithm = x
            });
        });

        var identityProviderRestrictions = new List<AccessClientIdPRestriction>();
        model.IdentityProviderRestrictions?.ForEach(x =>
        {
            identityProviderRestrictions.Add(new AccessClientIdPRestriction
            {
                Provider = x
            });
        });

        var claims = model.Claims?.Select(x =>
        {
            return new AccessClientClaim
            {
                Type = x.Key,
                Value = x.Value
            };
        }).ToList();

        var allowedCorsOrigins = new List<AccessClientCorsOrigin>();
        model.AllowedCorsOrigins?.ForEach(x =>
        {
            allowedCorsOrigins.Add(new AccessClientCorsOrigin
            {
                Origin = x
            });
        });

        var properties = model.Properties?.Select(x =>
        {
            return new AccessClientProperty
            {
                Key = x.Key,
                Value = x.Value
            };
        }).ToList();


        var domainProxies = new List<Domain.Entities.DomainProxy>();
        model.DomainProxies?.ForEach(x =>
        {
            domainProxies.Add(new Domain.Entities.DomainProxy
            {
                Domain = x.Domain,
                TenantId = x.TenantId,
                TenantName = x.TenantName,
            });
        });


        var command = new CreateAccessClientCommand(
            model.Enabled,
            model.ClientId,
            model.ProtocolType,
            clientSecrets,
            model.RequireClientSecret,
            model.ClientName,
            model.Description,
            model.ClientUri,
            model.LogoUri,
            model.RequireConsent,
            model.AllowRememberConsent,
            model.AlwaysIncludeUserClaimsInIdToken,
            allowedGrantTypes,
            model.RequirePkce,
            model.AllowPlainTextPkce,
            model.RequireRequestObject,
            model.AllowAccessTokensViaBrowser,
            redirectUris,
            postLogoutRedirectUris,
            model.FrontChannelLogoutUri,
            model.FrontChannelLogoutSessionRequired,
            model.BackChannelLogoutUri,
            model.BackChannelLogoutSessionRequired,
            model.AllowOfflineAccess,
            allowedScopes,
            model.IdentityTokenLifetime,
            allowedIdentityTokenSigningAlgorithms,
            model.AccessTokenLifetime,
            model.AuthorizationCodeLifetime,
            model.ConsentLifetime,
            model.AbsoluteRefreshTokenLifetime,
            model.SlidingRefreshTokenLifetime,
            model.RefreshTokenUsage,
            model.UpdateAccessTokenClaimsOnRefresh,
            model.RefreshTokenExpiration,
            model.AccessTokenType,
            model.EnableLocalLogin,
            identityProviderRestrictions,
            model.IncludeJwtId,
            claims,
            model.AlwaysSendClientClaims,
            model.ClientClaimsPrefix,
            model.PairWiseSubjectSalt,
            allowedCorsOrigins,
            properties,
            model.UserSsoLifetime,
            model.UserCodeType,
            model.DeviceCodeLifetime,
            domainProxies
        );

        return command;
    }
}