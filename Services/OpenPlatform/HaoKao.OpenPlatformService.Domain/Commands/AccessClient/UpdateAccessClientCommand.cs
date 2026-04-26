using FluentValidation;

namespace HaoKao.OpenPlatformService.Domain.Commands.AccessClient;

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Enabled">是否启用</param>
/// <param name="ClientId">客户端标识</param>
/// <param name="ProtocolType">协议类型</param>
/// <param name="ClientSecrets">客户端密钥</param>
/// <param name="RequireClientSecret">需要客户端密钥</param>
/// <param name="ClientName">客户端名称</param>
/// <param name="Description">描述</param>
/// <param name="ClientUri">客户端 Uri</param>
/// <param name="LogoUri">微标 Uri </param>
/// <param name="RequireConsent">需要同意</param>
/// <param name="AllowRememberConsent">允许记住同意</param>
/// <param name="AlwaysIncludeUserClaimsInIdToken">始终在身份令牌中包含用户声明</param>
/// <param name="AllowedGrantTypes">允许的授权类型</param>
/// <param name="RequirePkce">需要 Pkce</param>
/// <param name="AllowPlainTextPkce">允许纯文本 Pkce</param>
/// <param name="RequireRequestObject">需要请求对象</param>
/// <param name="AllowAccessTokensViaBrowser">允许通过浏览器访问令牌</param>
/// <param name="RedirectUris">重定向 Uri</param>
/// <param name="PostLogoutRedirectUris">注销重定向 Uri </param>
/// <param name="FrontChannelLogoutUri">前端通道注销 Uri</param>
/// <param name="FrontChannelLogoutSessionRequired">需要前端通道注销会话</param>
/// <param name="BackChannelLogoutUri">后端通道退出 Uri </param>
/// <param name="BackChannelLogoutSessionRequired">需要后端通道注销会话</param>
/// <param name="AllowOfflineAccess">允许离线访问 </param>
/// <param name="AllowedScopes">允许的作用域</param>
/// <param name="IdentityTokenLifetime">身份令牌生命周期</param>
/// <param name="AllowedIdentityTokenSigningAlgorithms">允许的身份令牌签名算法</param>
/// <param name="AccessTokenLifetime">访问令牌生命周期 </param>
/// <param name="AuthorizationCodeLifetime">授权码生命周期</param>
/// <param name="ConsentLifetime">同意生命周期</param>
/// <param name="AbsoluteRefreshTokenLifetime">绝对刷新令牌生命周期</param>
/// <param name="SlidingRefreshTokenLifetime">滚动刷新令牌生命周期</param>
/// <param name="RefreshTokenUsage">刷新令牌使用情况</param>
/// <param name="UpdateAccessTokenClaimsOnRefresh">刷新时更新访问令牌声明</param>
/// <param name="RefreshTokenExpiration">刷新令牌过期</param>
/// <param name="AccessTokenType">访问令牌类型</param>
/// <param name="EnableLocalLogin">启用本地登录</param>
/// <param name="IdentityProviderRestrictions">身份提供程序限制</param>
/// <param name="IncludeJwtId">包括 Jwt 标识</param>
/// <param name="Claims">声明</param>
/// <param name="AlwaysSendClientClaims">始终发送客户端声明</param>
/// <param name="ClientClaimsPrefix">客户端声明前缀</param>
/// <param name="PairWiseSubjectSalt">配对主体盐</param>
/// <param name="AllowedCorsOrigins">允许跨域来源</param>
/// <param name="Properties">属性</param>
/// <param name="UserSsoLifetime">用户 SSO 生命周期</param>
/// <param name="UserCodeType">用户代码类型</param>
/// <param name="DeviceCodeLifetime">设备代码生命周期</param>
/// <param name="DomainProxies"></param>
public record UpdateAccessClientCommand(
    Guid Id,
    bool Enabled,
    string ClientId,
    string ProtocolType,
    List<AccessClientSecret> ClientSecrets,
    bool RequireClientSecret,
    string ClientName,
    string Description,
    string ClientUri,
    string LogoUri,
    bool RequireConsent,
    bool AllowRememberConsent,
    bool AlwaysIncludeUserClaimsInIdToken,
    List<AccessClientGrantType> AllowedGrantTypes,
    bool RequirePkce,
    bool AllowPlainTextPkce,
    bool RequireRequestObject,
    bool AllowAccessTokensViaBrowser,
    List<AccessClientRedirectUri> RedirectUris,
    List<AccessClientPostLogoutRedirectUri> PostLogoutRedirectUris,
    string FrontChannelLogoutUri,
    bool FrontChannelLogoutSessionRequired,
    string BackChannelLogoutUri,
    bool BackChannelLogoutSessionRequired,
    bool AllowOfflineAccess,
    List<AccessClientScope> AllowedScopes,
    int IdentityTokenLifetime,
    List<AccessClientSigningAlgorithm> AllowedIdentityTokenSigningAlgorithms,
    int AccessTokenLifetime,
    int AuthorizationCodeLifetime,
    int? ConsentLifetime,
    int AbsoluteRefreshTokenLifetime,
    int SlidingRefreshTokenLifetime,
    int RefreshTokenUsage,
    bool UpdateAccessTokenClaimsOnRefresh,
    int RefreshTokenExpiration,
    int AccessTokenType,
    bool EnableLocalLogin,
    List<AccessClientIdPRestriction> IdentityProviderRestrictions,
    bool IncludeJwtId,
    List<AccessClientClaim> Claims,
    bool AlwaysSendClientClaims,
    string ClientClaimsPrefix,
    string PairWiseSubjectSalt,
    List<AccessClientCorsOrigin> AllowedCorsOrigins,
    List<AccessClientProperty> Properties,
    int? UserSsoLifetime,
    string UserCodeType,
    int DeviceCodeLifetime,
    List<DomainProxy> DomainProxies
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => ClientId)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(200).WithMessage("长度不能大于200")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => ProtocolType)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(200).WithMessage("长度不能大于200")
                 .MinimumLength(2).WithMessage("长度不能小于2");


        validator.RuleFor(x => ClientName)
                 .MaximumLength(200).WithMessage("长度不能大于200");

        validator.RuleFor(x => Description)
                 .MaximumLength(1000).WithMessage("长度不能大于1000");

        validator.RuleFor(x => ClientUri)
                 .MaximumLength(2000).WithMessage("长度不能大于2000");

        validator.RuleFor(x => LogoUri)
                 .MaximumLength(2000).WithMessage("长度不能大于2000");

        validator.RuleFor(x => FrontChannelLogoutUri)
                 .MaximumLength(2000).WithMessage("长度不能大于2000");

        validator.RuleFor(x => BackChannelLogoutUri)
                 .MaximumLength(2000).WithMessage("长度不能大于2000");

        validator.RuleFor(x => ClientClaimsPrefix)
                 .MaximumLength(200).WithMessage("200");

        validator.RuleFor(x => PairWiseSubjectSalt)
                 .MaximumLength(200).WithMessage("200");

        validator.RuleFor(x => UserCodeType)
                 .MaximumLength(100).WithMessage("100");
    }
}