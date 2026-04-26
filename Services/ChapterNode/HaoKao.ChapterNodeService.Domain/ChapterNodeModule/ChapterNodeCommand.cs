namespace HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="SubjectId"></param>
/// <param name="Code"></param>
/// <param name="Name"></param>
/// <param name="ParentId"></param>
/// <param name="ParentName"></param>
/// <param name="Sort"></param>
public record CreateChapterNodeCommand(
    Guid SubjectId,
    string Code,
    string Name,
    Guid? ParentId,
    string ParentName,
    int Sort
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Code)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id"></param>
/// <param name="SubjectId"></param>
/// <param name="Code"></param>
/// <param name="Name"></param>
/// <param name="ParentId"></param>
/// <param name="ParentName"></param>
/// <param name="Sort"></param>
public record UpdateChapterNodeCommand(
    Guid Id,
    Guid SubjectId,
    string Code,
    string Name,
    Guid? ParentId,
    string ParentName,
    int Sort
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Code)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteChapterNodeCommand(Guid Id) : Command("删除");