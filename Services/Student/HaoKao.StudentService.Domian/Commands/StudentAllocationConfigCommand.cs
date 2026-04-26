using HaoKao.StudentService.Domain.Entities;

namespace HaoKao.StudentService.Domain.Commands;

/// <summary>
/// 保存学员分配配置命令
/// </summary>
/// <param name="Data">配置数据</param>
/// <param name="WaysOfAllocation">分配方式</param>
/// <param name="TenantId">租户Id</param>
public record SaveStudentAllocationConfigCommand(
    HashSet<PercentageAllocation> Data,
    WaysOfAllocation WaysOfAllocation,
    Guid TenantId
) : Command("保存学员分配配置命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Data).Must(x => x.Sum(y => y.Percentage) == 1).WithMessage("配置数据中的百分比之和必须等于 1");
    }
}