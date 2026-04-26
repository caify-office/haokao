using Girvs.Refit;
using HaoKao.OpenPlatformService.Application.ViewModels.Account;
using HaoKao.OpenPlatformService.Domain.Entities;
using Refit;

namespace HaoKao.OpenPlatformService.Application.Services.Refit;

[RefitService("WeiXinService", false)]
public interface IWeiXinRemoteService : IGirvsRefit
{
    [Get("/sns/jscode2session")]
    Task<string> Code2Session(string appid, string secret, string js_code, string grant_type = "authorization_code");

    [Post("/cgi-bin/stable_token")]
    Task<string> GetAccessTokenAsync([Body(true)] WeiXinAccessTokenBody body);

    [Headers("Content-Type: application/json")]
    [Post("/wxa/business/getuserphonenumber")]
    Task<string> GetUserPhoneNumber(string access_token, [Body(true)] WeiXinUserPhoneNumberBody body);

    [Headers("Content-Type: application/json")]
    [Post("/wxa/generatescheme")]
    Task<WeiXinGenerateSchemeResponse> GenerateScheme(string access_token, [Body] string body);

    [Headers("Content-Type: application/json")]
    [Post("/wxa/queryscheme")]
    Task<WeiXinQuerySchemeResponse> QueryScheme(string access_token, [Body] string body);

    [Post("/cgi-bin/qrcode/create")]
    Task<WeiXinOffiAccountQrCodeResponse> GetOffiAccountQrCode(string access_token, [Body] string body);

    [Get("/cgi-bin/user/info")]
    Task<WeiXinUserInfoResponse> UserInfo(string access_token, string openid, string lang = "zh_CN");

    [Get("/cgi-bin/user/get")]
    Task UserGet(string access_token, string next_openid = "");
}
