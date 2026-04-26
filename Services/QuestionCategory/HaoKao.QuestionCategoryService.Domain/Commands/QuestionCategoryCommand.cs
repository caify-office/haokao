using System;
using FluentValidation;
using HaoKao.QuestionCategoryService.Domain.Enums;

namespace HaoKao.QuestionCategoryService.Domain.Commands;

/// <summary>
/// 创建题库类别命令
/// </summary>
/// <param name="Name">类名名称</param>
/// <param name="Code">类别代码</param>
/// <param name="AdaptPlace">适应场景</param>
/// <param name="DisplayCondition">显示条件</param>
/// <param name="ProductPackageId">产品包Id(购买跳转对象)</param>
/// <param name="ProductPackageType">产品包类型</param>
public record CreateQuestionCategoryCommand(
    string Name,
    string Code,
    AdaptPlace AdaptPlace,
    DisplayConditionEnum DisplayCondition,
    Guid ProductPackageId,
    ProductPackageType? ProductPackageType
) : Command("创建题库类别")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("类名名称不能为空")
                 .MaximumLength(50).WithMessage("类名名称长度不能大于50")
                 .MinimumLength(2).WithMessage("类名名称长度不能小于2");

        validator.RuleFor(x => Code)
                 .NotEmpty().WithMessage("类别代码不能为空")
                 .MaximumLength(50).WithMessage("类别代码长度不能大于50")
                 .MinimumLength(2).WithMessage("类别代码长度不能小于2");
    }
}

/// <summary>
/// 更新题库类别命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="Name">类名名称</param>
/// <param name="Code">类别代码</param>
/// <param name="AdaptPlace">适应场景</param>
/// <param name="DisplayCondition">显示条件</param>
/// <param name="ProductPackageId">产品包Id(购买跳转对象)</param>
/// <param name="ProductPackageType">产品包类型</param>
public record UpdateQuestionCategoryCommand(
    Guid Id,
    string Name,
    string Code,
    AdaptPlace AdaptPlace,
    DisplayConditionEnum DisplayCondition,
    Guid ProductPackageId,
    ProductPackageType? ProductPackageType
) : Command("更新题库类别")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("类名名称不能为空")
                 .MaximumLength(50).WithMessage("类名名称长度不能大于50")
                 .MinimumLength(2).WithMessage("类名名称长度不能小于2");

        validator.RuleFor(x => Code)
                 .NotEmpty().WithMessage("类别代码不能为空")
                 .MaximumLength(50).WithMessage("类别代码长度不能大于50")
                 .MinimumLength(2).WithMessage("类别代码长度不能小于2");
    }
}

/// <summary>
/// 删除题库类别命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteQuestionCategoryCommand(Guid Id) : Command("删除题库类别");
