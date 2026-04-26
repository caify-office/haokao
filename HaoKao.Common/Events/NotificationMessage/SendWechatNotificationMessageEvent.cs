namespace HaoKao.Common.Events.NotificationMessage;

public record SendWechatNotificationMessageEvent(
    EventNotificationMessageType MessageType,
    string OpenId,
    Dictionary<string, string> Parameter
) : SendNotificationMessageEvent(
    string.Empty,
    string.Empty,
    OpenId,
    MessageType,
    EventReceivingChannel.WebChat
);