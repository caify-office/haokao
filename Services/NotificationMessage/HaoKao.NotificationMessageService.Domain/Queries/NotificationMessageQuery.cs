using HaoKao.Common.Events.NotificationMessage;
using HaoKao.NotificationMessageService.Domain.Enumerations;

namespace HaoKao.NotificationMessageService.Domain.Queries;

public class NotificationMessageQuery : QueryBase<Models.NotificationMessage>
{
    /// <summary>
    /// 接收渠道
    /// </summary>
    [QueryCacheKey]
    public ReceivingChannel? ReceivingChannel { get; set; }

    /// <summary>
    /// 发送状态
    /// </summary>
    [QueryCacheKey]
    public NotificationMessageSendState? SendState { get; set; }

    /// <summary>
    /// 消息类型
    /// </summary>
    [QueryCacheKey]
    public EventNotificationMessageType? NotificationMessageType { get; set; }

    /// <summary>
    /// 消息模板ID
    /// </summary>
    [QueryCacheKey]
    public string MessageTemplateId { get; set; }

    /// <summary>
    /// 消息所属者的身份证号码
    /// </summary>
    [QueryCacheKey]
    public string IdCard { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    [QueryCacheKey]
    public string Receiver { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartDateTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndDateTime { get; set; }

    /// <summary>
    /// 对应的考试Id
    /// </summary>
    [QueryCacheKey]
    public Guid? TenantAccessId { get; set; }

    public override Expression<Func<Models.NotificationMessage, bool>> GetQueryWhere()
    {
        Expression<Func<Models.NotificationMessage, bool>> expression = x => true;
        if (ReceivingChannel.HasValue)
        {
            expression = expression.And(x => x.ReceivingChannel == ReceivingChannel);
        }

        if (SendState.HasValue)
        {
            expression = expression.And(x => x.SendState == SendState);
        }

        if (NotificationMessageType.HasValue)
        {
            expression = expression.And(x => x.NotificationMessageType == NotificationMessageType);
        }

        if (!string.IsNullOrEmpty(MessageTemplateId))
        {
            expression = expression.And(x => x.MessageTemplateId == MessageTemplateId);
        }

        if (!string.IsNullOrEmpty(IdCard))
        {
            expression = expression.And(x => x.IdCard.Contains(IdCard));
        }

        if (!string.IsNullOrEmpty(Receiver))
        {
            expression = expression.And(x => x.Receiver.Contains(Receiver));
        }

        if (StartDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartDateTime);
        }

        if (EndDateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndDateTime);
        }

        return expression;
    }
}