using AspNet.Security.OAuth.Weixin;
using Girvs.Extensions;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth.HaokaoWeixin;

public class HaoKaoWeixinAuthenticationHandler(
    IOptionsMonitor<WeixinAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : WeixinAuthenticationHandler(options, logger, encoder), IHaoKaoAuthenticationHandler
{
    private const string _DefaultHeadImg =
        "https://mmbiz.qpic.cn/mmbiz/icTdbqWNOwNRna42FI242Lcia07jQodd2FJGIYQfG0LAJGFxM4FbnQP6yfMxBgJ0F3YRqJCJ1aPAK2dQagdusBZg/0";

    public string GetRedirectUrl(AuthenticationProperties properties, string authenticationCallBackUrl)
    {
        var redirectUri = string.IsNullOrEmpty(authenticationCallBackUrl)
            ? BuildRedirectUri(Options.CallbackPath)
            : authenticationCallBackUrl + Options.CallbackPath;

        GenerateCorrelationId(properties);
        return base.BuildChallengeUrl(properties, redirectUri);
    }

    public ExternalUser GetCurrentAuthenticationUniqueIdentifier(IEnumerable<System.Security.Claims.Claim> claims)
    {
        var unionId = claims.FirstOrDefault(x => x.Type == "urn:weixin:unionid")?.Value;
        var nikeName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var headImage = claims.FirstOrDefault(x => x.Type == WeixinAuthenticationConstants.Claims.HeadImgUrl)?.Value;
        var otherInformation = claims.ToDictionary(x => x.Type, x => x.Value);

        if (headImage.IsNullOrEmpty())
        {
            headImage = _DefaultHeadImg;
        }

        return new ExternalUser
        {
            Schemem = WeixinAuthenticationDefaults.AuthenticationScheme,
            NikeName = nikeName,
            HeadImage = headImage,
            UniqueIdentifier = unionId,
            OtherInformation = otherInformation
        };
    }

    private static AuthenticationProperties GetAuthenticationProperties()
    {
        return new AuthenticationProperties
        {
            RedirectUri = "~/",
            Items =
            {
                { "scheme", WeixinAuthenticationDefaults.AuthenticationScheme },
            }
        };
    }

    public async Task<ExternalUser> CreateExternalTicketAsync(ExternalCodeAuthenticationOptions options)
    {
        var codeExchangeContext = new OAuthCodeExchangeContext(GetAuthenticationProperties(), options.Code, BuildRedirectUri(Options.CallbackPath));

        if (options.SchemeType.ToLower() == "app")
        {
            var requestResult = await ExchangeAppCodeAsync(codeExchangeContext, options);
            if (requestResult.Succeeded)
            {
                return GetCurrentAuthenticationUniqueIdentifier(requestResult.Principal!.Claims);
            }
            throw new GirvsException($"{requestResult.Failure?.Message}");
        }
        var claimsPrincipal = await ExchangeMiniProgramCodeAsync(codeExchangeContext, options);
        return GetCurrentAuthenticationUniqueIdentifier(claimsPrincipal.Claims);
    }

    #region MiniProgramCode

    private async Task<ClaimsPrincipal> ExchangeMiniProgramCodeAsync(OAuthCodeExchangeContext context, ExternalCodeAuthenticationOptions options)
    {
        const string tokenEndpoint = "https://api.weixin.qq.com/sns/jscode2session";
        var tokenRequestParameters = new Dictionary<string, string>
        {
            ["appid"] = options.ClientId,
            ["secret"] = options.ClientSecret,
            ["js_code"] = options.Code,
            ["grant_type"] = "authorization_code",
        };

        var address = QueryHelpers.AddQueryString(tokenEndpoint, tokenRequestParameters);

        using var response = await Backchannel.GetAsync(address);
        if (!response.IsSuccessStatusCode)
        {
            throw new GirvsException("An error occurred while retrieving an access token.");
        }

        var json = await response.Content.ReadAsStringAsync(Context.RequestAborted);
        var payload = JsonDocument.Parse(json);

        var errorCode = payload.RootElement.GetString("errcode");
        if (!string.IsNullOrEmpty(errorCode))
        {
            throw new GirvsException("An error occurred while retrieving an access token.", error: json);
        }

        var unionid = payload.RootElement.GetString("unionid");
        if (string.IsNullOrEmpty(unionid))
        {
            throw new GirvsException("未获取到指定的unionid,请检查当前小程序是否绑定对应的开放平台", error: json);
        }

        var identity = new ClaimsIdentity(ClaimsIssuer);
        identity.AddClaim(new System.Security.Claims.Claim("urn:weixin:unionid", unionid));
        identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, "微信用户"));
        identity.AddClaim(new System.Security.Claims.Claim(WeixinAuthenticationConstants.Claims.HeadImgUrl, _DefaultHeadImg));
        var principal = new ClaimsPrincipal(identity);

        await Context.SignInAsync(SignInScheme, principal, context.Properties);
        return principal;
    }

    #endregion

    #region App Code

    private async Task<HandleRequestResult> ExchangeAppCodeAsync(OAuthCodeExchangeContext context, ExternalCodeAuthenticationOptions options)
    {
        var properties = context.Properties;
        using var tokens = await ExchangeExternalCodeAsync(context, options);

        if (tokens.Error != null)
        {
            return HandleRequestResult.Fail(tokens.Error, properties);
        }

        if (string.IsNullOrEmpty(tokens.AccessToken))
        {
            return HandleRequestResult.Fail("Failed to retrieve access token.", properties);
        }

        var identity = new ClaimsIdentity(ClaimsIssuer);
        var ticket = await CreateTicketAsync(identity, properties, tokens);

        await Context.SignInAsync(SignInScheme, ticket.Principal!, ticket.Properties);
        return HandleRequestResult.Success(ticket);
    }

    private async Task<OAuthTokenResponse> ExchangeExternalCodeAsync(OAuthCodeExchangeContext context, ExternalCodeAuthenticationOptions options)
    {
        var tokenRequestParameters = new Dictionary<string, string>
        {
            ["appid"] = options.ClientId,
            ["secret"] = options.ClientSecret,
            ["code"] = context.Code,
            ["grant_type"] = "authorization_code",
        };

        const string tokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";

        var address = QueryHelpers.AddQueryString(tokenEndpoint, tokenRequestParameters);
        using var response = await Backchannel.GetAsync(address);

        if (!response.IsSuccessStatusCode)
        {
            return OAuthTokenResponse.Failed(new Exception($"通过Code:{context.Code} 获取微信Access出错：{response.StatusCode}"));
        }

        var json = await response.Content.ReadAsStringAsync(Context.RequestAborted);
        var payload = JsonDocument.Parse(json);
        var errorCode = payload.RootElement.GetString("errcode");

        if (!string.IsNullOrEmpty(errorCode))
        {
            return OAuthTokenResponse.Failed(new Exception($"通过Code:{context.Code} 读取微信Access出错：{errorCode} {json}"));
        }

        return OAuthTokenResponse.Success(payload);
    }

    #endregion
}