using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.MessageSenders;

public interface IWechatMessageSender : IMessageSender<WechatMessageSetting, Dictionary<string, string>>, IManager;