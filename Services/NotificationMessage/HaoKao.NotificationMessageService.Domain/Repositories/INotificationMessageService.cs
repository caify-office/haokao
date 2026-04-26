using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.Repositories;

public interface INotificationMessageService : IManager
{
    Task SendMessageAsync(MessageSender messageSender);
}


public class MessageSender
{
    public MessageSetting MessageSetting { get; set; }
    public NotificationMessage NotificationMessage { get; set; }
    public Dictionary<string, string> Parameter { get; set; }

    public TenantSignSetting TenantSignSetting { get; set; }

    public RegisteredUser RegisteredUser { get; set; }
}