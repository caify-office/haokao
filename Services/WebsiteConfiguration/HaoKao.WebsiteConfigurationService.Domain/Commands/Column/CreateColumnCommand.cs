namespace HaoKao.WebsiteConfigurationService.Domain.Commands.Column;
/// <summary>
/// 创建栏目命令
/// </summary>
/// <param name="Id"></param>
/// <param name="DomainName">域名</param>
/// <param name="Name">名称</param>
/// <param name="Alias">别名</param>
/// <param name="EnglishName">英文名</param>
/// <param name="ParentId">父节点id</param>
/// <param name="Icon">图标</param>
/// <param name="ActiveIcon">当前图标</param>
/// <param name="IsHomePage">是否首页</param>
/// <param name="Sort">排序</param>
public record CreateColumnCommand(
    Guid Id,
    string DomainName,
    string Name,
    string Alias,
    string EnglishName,
    Guid? ParentId,
    string Icon,
    string ActiveIcon,
    bool IsHomePage,
    int Sort
) : Command("创建栏目")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {

        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("名称不能为空")
            .MaximumLength(50).WithMessage("名称长度不能大于50")
            .MinimumLength(2).WithMessage("名称长度不能小于2");



    }
}