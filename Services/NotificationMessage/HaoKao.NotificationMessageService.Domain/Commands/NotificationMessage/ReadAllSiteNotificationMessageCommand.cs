namespace HaoKao.NotificationMessageService.Domain.Commands.NotificationMessage;
/// <summary>
/// 设置考生阅读全部站点消息
/// </summary>
/// <param name="IdCard">身份证号</param>
public record ReadAllSiteNotificationMessageCommand(string IdCard) : Command("设置考生阅读全部站点消息");