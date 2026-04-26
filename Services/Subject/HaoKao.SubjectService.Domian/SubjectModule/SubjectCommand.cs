using FluentValidation;
using System;

namespace HaoKao.SubjectService.Domain.SubjectModule;


/// <summary>
/// 创建科目命令
/// </summary>
/// <param name="Name">名称</param>
/// <param name="IsCommon">是否是专业科目</param>
/// <param name="Sort">排序</param>
/// <param name="IsShow"></param>
public record CreateSubjectCommand(
    string Name,
    SubjectTypeEnum IsCommon,
    int Sort,
    bool IsShow
) : Command("创建科目")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("科目名称不能为空")
            .MaximumLength(50).WithMessage("科目名称长度不能大于50")
            .MinimumLength(2).WithMessage("科目名称长度不能小于2");
    }
}

/// <summary>
/// 更新科目命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">科目名称</param>
/// <param name="IsCommon">普通科目/专业科目</param>
/// <param name="Sort">排序</param>
/// <param name="IsShow">是否显示</param>
public record UpdateSubjectCommand(
   Guid Id,
   string Name,
   SubjectTypeEnum IsCommon,
   int Sort,
   bool IsShow
) : Command("更新科目")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
            .NotEmpty().WithMessage("科目名称不能为空")
            .MaximumLength(50).WithMessage("科目名称长度不能大于50")
            .MinimumLength(2).WithMessage("科目名称长度不能小于2");
    }
}

/// <summary>
/// 删除科目命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteSubjectCommand(Guid Id) : Command("删除科目");