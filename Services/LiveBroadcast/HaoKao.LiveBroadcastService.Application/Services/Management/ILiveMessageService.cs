using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveMessageService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveMessageViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryLiveMessageViewModel> Get(QueryLiveMessageViewModel queryViewModel);

    /// <summary>
    /// 获取直播间置顶的消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PinTopMessageOutput> GetPinedMessage(PinTopMessageRequest request);

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task ProductOnSell(Guid liveId, Guid[] ids);
    /// <summary>
    /// 产品下架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task ProductDownSell(Guid liveId, Guid[] ids);
    /// <summary>
    /// 优惠券上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task CouponPickUp(Guid liveId, Guid[] ids);

    /// <summary>
    /// 优惠券下架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task CouponPickDown(Guid liveId, Guid[] ids);


    /// <summary>
    /// 创建直播消息
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveMessageViewModel model);

    /// <summary>
    /// 删除直播消息
    /// </summary>
    /// <param name="id"></param>
    Task Delete(Guid id);
}