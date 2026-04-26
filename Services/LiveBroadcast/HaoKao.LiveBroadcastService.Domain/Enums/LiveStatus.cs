namespace HaoKao.LiveBroadcastService.Domain.Enums;

public enum LiveStatus
{
    /// <summary>
    /// 未开始
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// 直播中
    /// </summary>
    LiveStreaming = 1,

    /// <summary>
    /// 已结束
    /// </summary>
    Ended = 2,
}