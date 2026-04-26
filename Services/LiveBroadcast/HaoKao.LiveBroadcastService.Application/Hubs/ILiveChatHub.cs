using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.Hubs;

public interface ILiveChatHub
{
    /// <summary>
    /// 聊天发送的消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ChatMessage(LiveChatMessageOutput message);

    /// <summary>
    /// 加入直播间的消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task JoinRoom(LiveMessageOutput message);

    /// <summary>
    /// 用户分享直播
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task ShareLive(LiveMessageOutput message);

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task ProductOnSell(Guid[] ids);

    /// <summary>
    /// 产品下架通知
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task ProductDownSell(Guid[] ids);

    /// <summary>
    /// 优惠券上架通知
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task CouponPickUp(Guid[] ids);

    /// <summary>
    /// 优惠券下架通知
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task CouponPickDown(Guid[] ids);

    /// <summary>
    /// 置顶某条消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task PinTopMessage(PinTopMessageOutput message);

    /// <summary>
    /// 在线人数
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    Task OnlineCount(int count);

    /// <summary>
    /// 直播状态变更
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    Task LiveStatusChanged(LiveStatus status);

    /// <summary>
    /// 撤回消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task RevokeMessage(Guid id);

    /// <summary>
    /// @用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task MentionUser(Guid id);

    /// <summary>
    /// 禁言
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    Task Muted(MutedOutput output);
}