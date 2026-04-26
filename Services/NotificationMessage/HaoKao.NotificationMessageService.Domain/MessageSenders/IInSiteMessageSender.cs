using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.MessageSenders;

public interface IInSiteMessageSender : IMessageSender<InSiteMessageSetting, string[]>, IManager;