namespace HaoKao.Common.Events.NotificationMessage;

public record SendInSiteNotificationMessageEvent(
    string Title,
    EventNotificationMessageType EventNotificationMessageType,
    string IdCard,
    string PhoneNumber,
    string[] Parameter
) : SendNotificationMessageEvent(
    Title,
    IdCard,
    PhoneNumber,
    EventNotificationMessageType,
    EventReceivingChannel.InSite
);