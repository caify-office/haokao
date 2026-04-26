using HaoKao.AgreementService.Domain.Entities;

namespace HaoKao.AgreementService.Domain.Commands;



/// <summary>
/// 创建课程协议命令
/// </summary>
/// <param name="Name">协议名称</param>
/// <param name="Content">协议内容</param>
/// <param name="Continuation">续读次数</param>
/// <param name="AgreementType">协议类型</param>
public record CreateCourseAgreementCommand(
    string Name,
    string Content,
    int Continuation,
    AgreementType AgreementType
) : Command("创建课程协议")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("协议名称不能为空")
                 .MaximumLength(50).WithMessage("协议名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("协议内容不能为空");
    }
}

/// <summary>
/// 更新课程协议命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">协议名称</param>
/// <param name="Content">协议内容</param>
/// <param name="Continuation">续读次数</param>
/// <param name="AgreementType">协议类型</param>
public record UpdateCourseAgreementCommand(
    Guid Id,
    string Name,
    string Content,
    int Continuation,
    AgreementType AgreementType
) : Command("更新课程协议")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("协议名称不能为空")
                 .MaximumLength(50).WithMessage("协议名称长度不能大于50");

        validator.RuleFor(x => Content)
                 .NotEmpty().WithMessage("协议内容不能为空");
    }
}

/// <summary>
/// 删除课程协议命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCourseAgreementCommand(Guid Id) : Command("删除课程协议");