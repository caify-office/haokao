namespace HaoKao.NotificationMessageService.Application.ViewModels.NotificationMessage;

[AutoMapFrom(typeof(NotificationMessageQuery))]
[AutoMapTo(typeof(NotificationMessageQuery))]
public class NotificationMessageQueryViewModel : QueryDtoBase<NotificationMessageQueryListViewModel>
{
    /// <summary>
    /// 接收渠道
    /// </summary>
    public ReceivingChannel? ReceivingChannel { get; set; }

    /// <summary>
    /// 发送状态
    /// </summary>
    public NotificationMessageSendState? SendState { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public EventNotificationMessageType? NotificationMessageType { get; set; }

    /// <summary>
    /// 消息模板ID
    /// </summary>
    public string MessageTemplateId { get; set; }

    /// <summary>
    /// 消息所属者的身份证号码
    /// </summary>
    public string IdCard { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndDateTime { get; set; }
}

[AutoMapFrom(typeof(Domain.Models.NotificationMessage))]
[AutoMapTo(typeof(Domain.Models.NotificationMessage))]
public class NotificationMessageQueryListViewModel : IDto
{
    /// <summary>
    /// 消息标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 参数消息内容
    /// </summary>
    public string ParameterContent { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 接收渠道
    /// </summary>
    public ReceivingChannel ReceivingChannel { get; set; }

    /// <summary>
    /// 发送状态
    /// </summary>
    public NotificationMessageSendState SendState { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    public EventNotificationMessageType NotificationMessageType { get; set; }

    /// <summary>
    /// 失败内容
    /// </summary>
    public string Failure { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public string Receiver { get; set; }

    /// <summary>
    /// 是否阅读
    /// </summary>
    public bool IsRead { get; set; }
}