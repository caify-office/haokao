using HaoKao.WebsiteConfigurationService.Domain.Enumerations;

namespace HaoKao.WebsiteConfigurationService.Domain.Commands.WebsiteTemplate;
/// <summary>
/// 创建模板命令
/// </summary>
/// <param name="Id"></param>
/// <param name="Name">名称</param>
/// <param name="WebsiteTemplateType">模板类型</param>
/// <param name="Desc">描述</param>
/// <param name="ColumnId">所属栏目Id</param>
/// <param name="ColumnName">所属栏目名称</param>
/// <param name="IsDefault">是否默认</param>
public record CreateWebsiteTemplateCommand(
    Guid Id,
    string Name,
    WebsiteTemplateType WebsiteTemplateType,
    string Desc,
    Guid ColumnId,
    string ColumnName,
    bool IsDefault
) : Command("创建模板")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("名称不能为空")
            .MaximumLength(50).WithMessage("名称长度不能大于50")
            .MinimumLength(2).WithMessage("名称长度不能小于2");



        validator.RuleFor(x => ColumnName)
            .NotEmpty().WithMessage("所属栏目名称不能为空")
            .MaximumLength(50).WithMessage("所属栏目名称长度不能大于50")
            .MinimumLength(2).WithMessage("所属栏目名称长度不能小于2");




    }
}