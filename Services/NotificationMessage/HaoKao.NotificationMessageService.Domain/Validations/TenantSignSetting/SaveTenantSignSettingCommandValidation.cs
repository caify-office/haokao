using HaoKao.NotificationMessageService.Domain.Commands.TenantSignSetting;

namespace HaoKao.NotificationMessageService.Domain.Validations.TenantSignSetting;

public class SaveTenantSignSettingCommandValidation : GirvsCommandValidator<SaveTenantSignSettingCommand>
{
    public SaveTenantSignSettingCommandValidation()
    {
        RuleFor(ms => ms.Sign)
            .NotEmpty().WithMessage("签名设置不能为空")
            .MaximumLength(100).WithMessage("签名长度不能大于100");
    }
}