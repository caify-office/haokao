namespace HaoKao.WebsiteConfigurationService.Domain.Commands.TemplateStyle;
/// <summary>
/// 创建模板命令
/// </summary>
/// <param name="DomainName">域名</param>
/// <param name="Name">名称</param>
/// <param name="Path">路径</param>
public record CreateTemplateStyleCommand(
    string DomainName,
    string Name,
    string Path
) : Command("创建模板")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => DomainName)
            .NotEmpty().WithMessage("域名不能为空")
            .MaximumLength(500).WithMessage("域名长度不能大于500")
            .MinimumLength(2).WithMessage("域名长度不能小于2");

        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("名称不能为空")
            .MaximumLength(50).WithMessage("名称长度不能大于50")
            .MinimumLength(2).WithMessage("名称长度不能小于2");

        validator.RuleFor(x => Path)
            .NotEmpty().WithMessage("路径不能为空")
            .MaximumLength(500).WithMessage("路径长度不能大于500")
            .MinimumLength(2).WithMessage("路径长度不能小于2");

    }
}