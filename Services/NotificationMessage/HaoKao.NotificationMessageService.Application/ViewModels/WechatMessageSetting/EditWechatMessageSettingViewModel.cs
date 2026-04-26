namespace HaoKao.NotificationMessageService.Application.ViewModels.WechatMessageSetting;

[AutoMapFrom(typeof(Domain.Models.WechatMessageSetting))]
[AutoMapTo(typeof(Domain.Models.WechatMessageSetting))]
public class EditWechatMessageSettingViewModel : IDto
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