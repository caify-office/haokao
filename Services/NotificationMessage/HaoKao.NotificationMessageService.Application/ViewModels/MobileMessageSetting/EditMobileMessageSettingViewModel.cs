namespace HaoKao.NotificationMessageService.Application.ViewModels.MobileMessageSetting;

[AutoMapFrom(typeof(Domain.Models.MobileMessageSetting))]
[AutoMapTo(typeof(Domain.Models.MobileMessageSetting))]
public class EditMobileMessageSettingViewModel : IDto
{
    /// <summary>
    /// 短信平台类型
    /// </summary>
    public MobileMessagePlatform MobileMessagePlatform { get; set; }
        
    /// <summary>
    /// AppId
    /// </summary>
    public string AppId { get; set; }
        
    /// <summary>
    /// AppSecret
    /// </summary>
    public string AppSecret { get; set; }
        
    /// <summary>
    /// 签名列表
    /// </summary>
    public List<string> SignList { get; set; }

    /// <summary>
    /// 默认签名
    /// </summary>
    public string DefaultSign { get; set; }

    /// <summary>
    /// 模板列表
    /// </summary>
    public List<MessageTemplate> Templates { get; set; }
        
    /// <summary>
    /// 短信SDk 应用ID
    /// </summary>
    public string SmsSdkAppId { get; set; }
}