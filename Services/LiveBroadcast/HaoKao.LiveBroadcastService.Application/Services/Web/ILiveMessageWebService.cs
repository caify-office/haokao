using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

public interface ILiveMessageWebService : IAppWebApiService, IManager
{
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
    /// 删除直播消息
    /// </summary>
    /// <param name="id"></param>
    Task Delete(Guid id);

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task ProductOnSell(Guid liveId,Guid[] ids);

    /// <summary>
    /// 优惠券领取通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    Task CouponPickUp(Guid liveId, Guid[] ids);
}