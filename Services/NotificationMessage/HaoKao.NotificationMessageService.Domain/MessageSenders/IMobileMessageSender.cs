using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.MessageSenders;

public interface IMobileMessageSender : IMessageSender<MobileMessageSetting, string[]>, IManager
{
    TenantSignSetting TenantSignSetting { get; set; }

    string PhoneNumber { get; set; }
}