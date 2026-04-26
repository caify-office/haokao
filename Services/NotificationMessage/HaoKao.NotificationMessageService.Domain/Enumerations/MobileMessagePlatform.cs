namespace HaoKao.NotificationMessageService.Domain.Enumerations;

/// <summary>
/// 手机短信平台
/// </summary>
public enum MobileMessagePlatform
{
    /// <summary>
    /// 阿里云
    /// </summary>
    [Description("阿里云")] Aliyun,

    /// <summary>
    /// 腾讯云
    /// </summary>
    [Description("腾讯云")] TencentCloud
}