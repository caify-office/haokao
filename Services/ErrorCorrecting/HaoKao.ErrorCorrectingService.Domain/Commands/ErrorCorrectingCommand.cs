using FluentValidation;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using System;

namespace HaoKao.ErrorCorrectingService.Domain.Commands;

/// <summary>
/// 创建本题纠错命令
/// </summary>
/// <param name="QuestionId">问题id</param>
/// <param name="UserId">用户id</param>
/// <param name="Description">描述</param>
/// <param name="QuestionTypes">问题类型</param>
/// <param name="SubjectId">科目id</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="QuestionTypeId">题目类型id</param>
/// <param name="QuestionTypeName">题目类型名称</param>
/// <param name="QuestionText">题干</param>
/// <param name="Nickname">昵称</param>
/// <param name="Phone">手机号码</param>
/// <param name="CategoryId">题库类型id</param>
/// <param name="CategoryName">题库类型名称</param>
public record CreateErrorCorrectingCommand(
    Guid QuestionId,
    Guid UserId,
    string Description,
    string QuestionTypes,
    Guid SubjectId,
    string SubjectName,
    Guid QuestionTypeId,
    string QuestionTypeName,
    string QuestionText,
    string Nickname,
    string Phone,
    Guid CategoryId,
    string CategoryName
) : Command("创建本题纠错")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Description)
                 .NotEmpty().WithMessage("描述不能为空")
                 .MaximumLength(2000).WithMessage("描述长度不能大于2000");

        validator.RuleFor(x => QuestionTypes)
                 .NotEmpty().WithMessage("问题类型不能为空")
                 .MaximumLength(50).WithMessage("问题类型长度不能大于50");
    }
}

/// <summary>
/// 删除题库类别命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteErrorCorrectingCommand(Guid Id) : Command("删除题库类别");

/// <summary>
/// 更新本题纠错命令
/// </summary>
/// <param name="Id"></param>
/// <param name="Status"></param>
public record UpdateErrorCorrectingCommand(Guid Id, StatusEnum Status) : Command("更新本题纠错");