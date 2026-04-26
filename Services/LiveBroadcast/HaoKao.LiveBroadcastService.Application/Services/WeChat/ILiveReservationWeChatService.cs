using HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播预约接口服务
/// </summary>
public interface ILiveReservationWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建直播预约
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<bool> Create([FromBody] CreateLiveReservationViewModel model);

    /// <summary>
    /// 我的直播预约
    /// </summary>
    /// <returns></returns>
    Task<List<BrowseLiveReservationViewModel>> MyLiveReservation();

    /// <summary>
    ///  预约人数统计
    /// </summary>
    /// <param name="productIds"></param>
    /// <returns></returns>
    Task<dynamic> LiveReservationCount(Guid[] productIds);
}