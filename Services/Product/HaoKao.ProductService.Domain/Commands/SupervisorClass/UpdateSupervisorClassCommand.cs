namespace HaoKao.ProductService.Domain.Commands.SupervisorClass;

/// <summary>
/// 更新班级督学命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">班级名称</param>
/// <param name="Year">年份</param>
/// <param name="ProductPackageId">产品包id</param>
/// <param name="ProductPackageName">产品包名称</param>
/// <param name="ProductId">产品id</param>
/// <param name="ProductName">产品名称</param>
/// <param name="SalespersonId">营销人员Id</param>
/// <param name="SalespersonName">营销人员名称</param>
public record UpdateSupervisorClassCommand(
    Guid Id,
    string Name,
    int Year,
    Guid ProductPackageId,
    string ProductPackageName,
    Guid ProductId,
    string ProductName,
    Guid SalespersonId,
    string SalespersonName
) : Command("更新班级督学")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("班级名称不能为空")
                 .MaximumLength(100).WithMessage("班级名称长度不能大于100")
                 .MinimumLength(2).WithMessage("班级名称长度不能小于2");

        validator.RuleFor(x => ProductName)
                 .NotEmpty().WithMessage("产品名称不能为空")
                 .MaximumLength(100).WithMessage("产品名称长度不能大于100")
                 .MinimumLength(2).WithMessage("产品名称长度不能小于2");

        validator.RuleFor(x => SalespersonId)
                 .NotEmpty().WithMessage("营销人员Id不能为空");

        validator.RuleFor(x => SalespersonName)
                 .NotEmpty().WithMessage("营销人员名称不能为空")
                 .MaximumLength(50).WithMessage("营销人员名称长度不能大于50")
                 .MinimumLength(2).WithMessage("营销人员名称长度不能小于2");
    }
}