using HaoKao.Common.Events.NotificationMessage;

namespace HaoKao.NotificationMessageService.Domain.Models;

public abstract class MessageSetting : AggregateRoot<Guid>
{
    /// <summary>
    /// AppId
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// AppSecret
    /// </summary>
    public string AppSecret { get; set; }

    /// <summary>
    /// 模板列表
    /// </summary>
    public List<MessageTemplate> Templates { get; set; }
}

public class MessageTemplate
{
    public EventNotificationMessageType NotificationMessageType { get; set; }

    public string TemplateId { get; set; }

    public string Desc { get; set; }
}