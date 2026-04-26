using HaoKao.NotificationMessageService.Domain.Enumerations;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.MessageSenders;

public interface IMessageSender<TSetting, TParameter> where TSetting : MessageSetting
{
    /// <summary>
    /// 消息设置, 继承自MessageSetting
    /// </summary>
    TSetting Setting { get; set; }

    /// <summary>
    /// 消息模板的参数
    /// <example>
    /// template: "Say Hi! {0}";
    /// parameter: ["Alice"]
    /// </example>
    /// </summary>
    TParameter Parameter { get; set; }

    /// <summary>
    /// 需要发送给的注册用户
    /// </summary>
    RegisteredUser RegisteredUser { get; set; }

    /// <summary>
    /// 消息发送方法
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <returns></returns>
    Task<(NotificationMessageSendState, string)> SendAsync(MessageTemplate messageTemplate);
}