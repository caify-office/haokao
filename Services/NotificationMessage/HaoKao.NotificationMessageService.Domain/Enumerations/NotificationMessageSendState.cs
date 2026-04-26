namespace HaoKao.NotificationMessageService.Domain.Enumerations;

/// <summary>
/// 消息的发送状态
/// </summary>
public enum NotificationMessageSendState
{
    /// <summary>
    /// 未发送
    /// </summary>
    NotSend,

    /// <summary>
    /// 发送成功
    /// </summary>
    SendSuccess,

    /// <summary>
    /// 发送失败
    /// </summary>
    SendFail
}