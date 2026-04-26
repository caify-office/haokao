using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 直播评论接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveCommentWebService(ILiveCommentService service) : ILiveCommentWebService
{
    /// <summary>
    /// 根据直播查询评论
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    [HttpGet("{liveId:guid}")]
    public Task<BrowseLiveCommentViewModel> Get(Guid liveId)
    {
        return service.Get(liveId);
    }

    /// <summary>
    /// 创建直播评论
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public Task Create([FromBody] CreateLiveCommentViewModel model)
    {
        return service.Create(model);
    }
}