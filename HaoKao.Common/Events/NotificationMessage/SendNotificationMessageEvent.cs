using System.ComponentModel;

namespace HaoKao.Common.Events.NotificationMessage;

public abstract record SendNotificationMessageEvent(
    string Title,
    string IdCard,
    string PhoneNumber,
    EventNotificationMessageType EventNotificationMessageType,
    EventReceivingChannel EventReceivingChannel
) : IntegrationEvent;

[Flags]
public enum EventReceivingChannel : long
{
    /// <summary>
    /// 微信公众号接收
    /// </summary>
    WebChat = 1,

    /// <summary>
    /// 站内消息
    /// </summary>
    InSite = 2,

    /// <summary>
    /// 手机接收
    /// </summary>
    Mobile = 4,
}

public enum EventNotificationMessageType
{
    /// <summary>
    /// 用户注册
    /// </summary>
    [Description("用户注册")]
    Register,

    /// <summary>
    /// 验证码登陆
    /// </summary>
    [Description("验证码登陆")]
    Login,

    /// <summary>
    /// 找回密码
    /// </summary>
    [Description("找回密码")]
    RetrievePassword,

    /// <summary>
    /// 更换手机号码
    /// </summary>
    [Description("更换手机号码")]
    ChangePhoneNumber,

    /// <summary>
    /// 课程更新消息
    /// </summary>
    [Description("课程更新消息")]
    CourseUpdate,

    /// <summary>
    /// 进度提醒消息
    /// </summary>
    [Description("进度提醒消息")]
    ProgressReminder,

    /// <summary>
    /// 自定义1
    /// </summary>
    [Description("开播提醒")]
    Customize_1,

    [Description("卡卷到期提醒")]
    Customize_2,

    [Description("下午好")]
    Customize_3,

    [Description("忙完了")]
    Customize_4,

    [Description("每天学")]
    Customize_5,

    [Description("头等大事")]
    Customize_6,

    [Description("学完了")]
    Customize_7,

    [Description("伊伊添加企微提醒")]
    Customize_8,

    [Description("左左添加企微提醒")]
    Customize_9,

    [Description("自定义10")]
    Customize_10,

    [Description("自定义11")]
    Customize_11,

    [Description("自定义12")]
    Customize_12,
}