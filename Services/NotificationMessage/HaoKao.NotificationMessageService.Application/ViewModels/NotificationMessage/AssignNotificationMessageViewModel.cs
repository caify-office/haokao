namespace HaoKao.NotificationMessageService.Application.ViewModels.NotificationMessage;

public class AssignNotificationMessageViewModel
{
    /// <summary>
    /// 接收渠道
    /// </summary>
    public EventReceivingChannel EventReceivingChannel { get; set; }
        
    /// <summary>
    /// 消息类型
    /// </summary>
    public EventNotificationMessageType EventNotificationMessageType { get; set; }

    /// <summary>
    /// 接收者
    /// </summary>
    public List<MessageReceiveUser> ReceiveUser { get; set; } = [];
}
    
public class MessageReceiveUser
{
    /// <summary>
    /// 手机号码
    /// </summary>
    public string PhoneNumber { get; set; }
        
    /// <summary>
    /// 身份证号码
    /// </summary>
    public string IdCard { get; set; }
}