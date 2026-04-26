using HaoKao.NotificationMessageService.Domain.Commands.MobileMessageSetting;

namespace HaoKao.NotificationMessageService.Domain.Validations.MobileMessageSetting;

public class SaveMobileMessageSettingCommandValidation :  GirvsCommandValidator<SaveMobileMessageSettingCommand> 
{
    public SaveMobileMessageSettingCommandValidation()
    {
        RuleFor(ms => ms.AppId)
            .NotEmpty().WithMessage("AppId不能为空")
            .MaximumLength(100).WithMessage("AppId长度不能大于100");
            
            
        RuleFor(ms => ms.AppSecret)
            .NotEmpty().WithMessage("AppSecret不能为空")
            .MaximumLength(200).WithMessage("AppSecret长度不能大于200");
            
            
        RuleFor(ms => ms.DefaultSign)
            .NotEmpty().WithMessage("默认的签名不能为空")
            .MaximumLength(40).WithMessage("默认的签名长度不能大于40");
            
        RuleFor(ms => ms.SmsSdkAppId)
            .NotEmpty().WithMessage("短信SDk 应用ID不能为空")
            .MaximumLength(100).WithMessage("短信SDk 应用ID长度不能大于100");
    }
}