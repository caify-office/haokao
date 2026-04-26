using Girvs.AuthorizePermission;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveOnlineUserWebService(ILiveOnlineUserRepository repository) : ILiveOnlineUserWebService
{
    /// <summary>
    /// 获取直播间在线用户
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    public Task<List<OnlineUserViewModel>> GetOnlineUserList(Guid liveId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        return repository.Query.Where(x => x.LiveId == liveId && x.IsOnline && x.CreatorId != userId)
                         .Select(x => new OnlineUserViewModel(x.CreatorId, x.CreatorName))
                         .Distinct()
                         .ToListAsync();
    }
}