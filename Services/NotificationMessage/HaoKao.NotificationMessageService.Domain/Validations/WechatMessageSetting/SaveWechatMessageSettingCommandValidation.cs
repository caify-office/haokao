using HaoKao.NotificationMessageService.Domain.Commands.WechatMessageSetting;

namespace HaoKao.NotificationMessageService.Domain.Validations.WechatMessageSetting;

public class SaveWechatMessageSettingCommandValidation : GirvsCommandValidator<SaveWechatMessageSettingCommand>
{
    public SaveWechatMessageSettingCommandValidation()
    {
        RuleFor(ms => ms.AppId)
            .NotEmpty().WithMessage("AppId不能为空")
            .MaximumLength(100).WithMessage("AppId长度不能大于100");
            
            
        RuleFor(ms => ms.AppSecret)
            .NotEmpty().WithMessage("AppSecret不能为空")
            .MaximumLength(200).WithMessage("AppSecret长度不能大于200");

    }
}