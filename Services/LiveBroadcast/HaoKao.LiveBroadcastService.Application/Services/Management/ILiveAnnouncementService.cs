using HaoKao.LiveBroadcastService.Application.ViewModels.LiveAnnouncement;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveAnnouncementService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveAnnouncementViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveAnnouncementQueryViewModel> Get(LiveAnnouncementQueryViewModel queryViewModel);

    /// <summary>
    /// 创建直播公告
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveAnnouncementViewModel model);

    /// <summary>
    /// 根据主键删除指定直播公告
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定直播公告
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateLiveAnnouncementViewModel model);
}