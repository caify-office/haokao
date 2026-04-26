using HaoKao.NotificationMessageService.Domain.Enumerations;

namespace HaoKao.NotificationMessageService.Domain.Models;

/// <summary>
/// 手机短信平台设置
/// </summary>
public class MobileMessageSetting : MessageSetting
{
    /// <summary>
    /// 短信平台类型
    /// </summary>
    public MobileMessagePlatform MobileMessagePlatform { get; set; }

    /// <summary>
    /// 短信SDk 应用ID
    /// </summary>
    public string SmsSdkAppId { get; set; }

    /// <summary>
    /// 签名列表
    /// </summary>
    public List<string> SignList { get; set; } = [];

    /// <summary>
    /// 默认签名
    /// </summary>
    public string DefaultSign { get; set; }
}