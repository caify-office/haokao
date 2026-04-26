using HaoKao.BasicService.Domain.Commands.SystemSetting;

namespace HaoKao.BasicService.Domain.Validations.SystemSetting;

public class UpdateSystemSettingCommandValidation : GirvsCommandValidator<UpdateSystemSettingCommand>
{
    public UpdateSystemSettingCommandValidation()
    {
        RuleFor(s => s.WebSiteName)
            .NotEmpty().WithMessage("网站站点名称不能为空")
            .MinimumLength(4).WithMessage("网站站点名称长度不能小于4")
            .MaximumLength(20).WithMessage("网站站点名称长度不能大于20");

        RuleFor(s => s.Logo)
            .MaximumLength(500).WithMessage("网站Logo不能超过500");

        RuleFor(s => s.HttpAddress)
            .MaximumLength(500).WithMessage("网站访问地址不能超过500");

        RuleFor(s => s.OrganizationalUnit)
            .MaximumLength(20).WithMessage("组织单位长度不能大于20");

        RuleFor(s => s.IcpFiling)
            .MaximumLength(30).WithMessage("网站备案号长度不能大于30");

        RuleFor(s => s.FilingAddress)
            .MaximumLength(500).WithMessage("网站备案号链接地址长度不能大于500");

        RuleFor(s => s.Copyright)
            .MaximumLength(50).WithMessage("版权所有长度不能大于50");

        RuleFor(s => s.CopyrightAddress)
            .MaximumLength(500).WithMessage("版权所有链接地址长度不能大于500");

        RuleFor(s => s.WeChatImage)
            .MaximumLength(500).WithMessage("公众号图标链接地址长度不能大于500");

        RuleFor(s => s.WeChatAppId)
            .MaximumLength(20).WithMessage("公众号AppId不能大于20");

        RuleFor(s => s.MiniOAddress)
            .MaximumLength(500).WithMessage("对象存储地址长度不能大于500");

        RuleFor(s => s.MiniOBucket)
            .MinimumLength(2).WithMessage("对象存储桶长度不能小于2")
            .MaximumLength(30).WithMessage("对象存储桶长度不能大于30");

        RuleFor(s => s.MiniOAppId)
            .MaximumLength(50).WithMessage("Minio Id 长度不能大于50");

        RuleFor(s => s.MiniOAppSecret)
            .MaximumLength(50).WithMessage("Minio Secret 长度不能大于50");
    }
}