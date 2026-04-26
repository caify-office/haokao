namespace HaoKao.NotificationMessageService.Domain.Enumerations;

[Flags]
public enum ReceivingChannel : long
{
    /// <summary>
    /// 微信公众号接收
    /// </summary>
    WebChat = 1,

    /// <summary>
    /// 站内消息
    /// </summary>
    InSite = 2,

    /// <summary>
    /// 手机接收
    /// </summary>
    Mobile = 4,
}