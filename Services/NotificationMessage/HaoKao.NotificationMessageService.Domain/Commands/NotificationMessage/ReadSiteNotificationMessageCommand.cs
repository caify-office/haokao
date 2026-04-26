namespace HaoKao.NotificationMessageService.Domain.Commands.NotificationMessage;
/// <summary>
/// 考生阅读指定消息
/// </summary>
/// <param name="Id">消息Id</param>
public record ReadSiteNotificationMessageCommand(Guid Id) : Command("考生阅读指定消息");