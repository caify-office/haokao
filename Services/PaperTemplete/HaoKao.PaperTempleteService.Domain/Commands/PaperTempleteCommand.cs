namespace HaoKao.PaperTempleteService.Domain.Commands;

/// <summary>
/// 创建命令
/// </summary>
/// <param name="TempleteName"></param>
/// <param name="Remark"></param>
/// <param name="SuitableSubjects"></param>
public record CreatePaperTempleteCommand(
    string TempleteName,
    string Remark,
    string SuitableSubjects
) : Command("创建")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => TempleteName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => Remark)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于100")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 更新命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="TempleteName"></param>
/// <param name="Remark"></param>
/// <param name="SuitableSubjects"></param>
public record UpdatePaperTempleteCommand(
    Guid Id,
    string TempleteName,
    string Remark,
    string SuitableSubjects
) : Command("更新")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => TempleteName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => Remark)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 更新试卷模板结构命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="TemplateStructDatas"></param>
public record UpdateTempleteStructCommand(Guid Id, string TemplateStructDatas) : Command("修改试卷模板结构");

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeletePaperTempleteCommand(Guid Id) : Command("删除");