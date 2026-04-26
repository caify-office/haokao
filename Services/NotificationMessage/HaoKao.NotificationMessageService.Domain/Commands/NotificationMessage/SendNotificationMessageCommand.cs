using HaoKao.Common.Events.NotificationMessage;
using HaoKao.NotificationMessageService.Domain.Enumerations;

namespace HaoKao.NotificationMessageService.Domain.Commands.NotificationMessage;

/// <summary>
/// 发送通知消息
/// </summary>
/// <param name="Title">消息标题</param>
/// <param name="ParameterContent">参数消息内容</param>
/// <param name="ReceivingChannel">接收渠道</param>
/// <param name="SendState">发送状态</param>
/// <param name="NotificationMessageType">消息类型</param>
/// <param name="MessageTemplateId">消息模板ID</param>
/// <param name="Failure">失败内容</param>
/// <param name="Receiver">接收者</param>
/// <param name="IsRead">是否阅读</param>
/// <param name="IdCard">消息所属者的身份证号码</param>
public record SendNotificationMessageCommand(
    string Title,
    string ParameterContent,
    ReceivingChannel ReceivingChannel,
    NotificationMessageSendState SendState,
    EventNotificationMessageType NotificationMessageType,
    string MessageTemplateId,
    string Failure,
    string Receiver,
    bool IsRead,
    string IdCard
) : Command("发送通知消息");