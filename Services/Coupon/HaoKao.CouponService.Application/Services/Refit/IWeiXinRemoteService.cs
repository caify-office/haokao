using Girvs.Refit;
using HaoKao.CouponService.Application.ViewModels.Refit;
using Refit;

namespace HaoKao.CouponService.Application.Services.Refit;

[RefitService("WeiXinService", false)]
public interface IWeiXinRemoteService : IGirvsRefit
{
    [Post("/cgi-bin/stable_token")]
    Task<string> GetAccessTokenAsync([Body(true)] WeiXinAccessTokenBody body);
}