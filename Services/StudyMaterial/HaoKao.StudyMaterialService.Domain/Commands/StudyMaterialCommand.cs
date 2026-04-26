using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Domain.Commands;

/// <summary>
/// 创建学习资料命令
/// </summary>
/// <param name="Name">资料名称</param>
/// <param name="Year">年份</param>
/// <param name="Enable">启用/禁用</param>
/// <param name="Materials">资料内容</param>
/// <param name="Subjects">科目</param>
public record CreateStudyMaterialCommand(
    string Name,
    int Year,
    bool Enable,
    List<Material> Materials,
    string Subjects
) : Command("创建学习资料")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("资料名称不能为空")
                 .MaximumLength(50).WithMessage("资料名称长度不能大于50");
    }
}

/// <summary>
/// 更新学习资料命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">资料名称</param>
/// <param name="Year">年份</param>
/// <param name="Enable">启用/禁用</param>
/// <param name="Materials">资料内容</param>
/// <param name="Subjects">科目</param>
public record UpdateStudyMaterialCommand(
    Guid Id,
    string Name,
    int Year,
    bool Enable,
    List<Material> Materials,
    string Subjects
) : Command("更新学习资料")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("资料名称不能为空")
                 .MaximumLength(50).WithMessage("资料名称长度不能大于50");
    }
}

/// <summary>
/// 批量启用/禁用学习资料服务
/// </summary>
/// <param name="Ids">Ids</param>
/// <param name="Enable">启用/禁用</param>
public record EnableStudyMaterialCommand(List<Guid> Ids, bool Enable) : Command("批量启用/禁用学习资料服务");

/// <summary>
/// 批量删除学习资料命令
/// </summary>
/// <param name="Ids">Ids</param>
public record DeleteStudyMaterialCommand(List<Guid> Ids) : Command("批量删除学习资料");