namespace HaoKao.LiveBroadcastService.Domain.Enums;

/// <summary>
/// 直播消息类型
/// </summary>
public enum LiveMessageType
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    Chat,

    /// <summary>
    /// 进入直播间
    /// </summary>
    JoinRoom,

    /// <summary>
    /// 产品上架
    /// </summary>
    ProductOnSell,

    /// <summary>
    /// 分享直播
    /// </summary>
    ShareLive,
}