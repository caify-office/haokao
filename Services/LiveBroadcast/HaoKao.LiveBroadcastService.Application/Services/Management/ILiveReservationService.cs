using HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播预约服务接口
/// </summary>
public interface ILiveReservationService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveReservationViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryLiveReservationViewModel> Get(QueryLiveReservationViewModel queryViewModel);

    /// <summary>
    /// 创建直播预约
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<bool> Create(CreateLiveReservationViewModel model);
}