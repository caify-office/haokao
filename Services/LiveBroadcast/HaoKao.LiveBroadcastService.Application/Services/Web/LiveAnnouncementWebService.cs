using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveAnnouncement;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

/// <summary>
/// 直播公告接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveAnnouncementWebService(
    ILiveAnnouncementService liveAnnouncementService
) : ILiveAnnouncementWebService
{
    #region 初始参数

    private readonly ILiveAnnouncementService _liveAnnouncementService = liveAnnouncementService ?? throw new ArgumentNullException(nameof(liveAnnouncementService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseLiveAnnouncementViewModel> Get(Guid id)
    {
        return _liveAnnouncementService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<LiveAnnouncementQueryViewModel> Get([FromQuery] LiveAnnouncementQueryViewModel queryViewModel)
    {
        return _liveAnnouncementService.Get(queryViewModel);
    }

    #endregion
}