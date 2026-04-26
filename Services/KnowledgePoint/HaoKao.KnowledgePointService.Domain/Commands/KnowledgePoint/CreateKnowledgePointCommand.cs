using FluentValidation;
using System;

namespace HaoKao.KnowledgePointService.Domain.Commands.KnowledgePoint;

/// <summary>
/// 创建知识点命令
/// </summary>
/// <param name="Name">名称</param>
/// <param name="ChapterNodeId">章节id</param>
/// <param name="ChapterNodeName">章节名称</param>
/// <param name="Remark">备注</param>
/// <param name="SubjectName">科目名称</param>
public record CreateKnowledgePointCommand(
    string Name,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    string Remark,
    string SubjectName
) : Command("创建知识点")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("知识点不能为空");

        validator.RuleFor(x => ChapterNodeId)
                 .NotEmpty().WithMessage("章节id不能为空");

        validator.RuleFor(x => ChapterNodeName)
                 .NotEmpty().WithMessage("章节名称不能为空");

        validator.RuleFor(x => SubjectName)
                 .NotEmpty().WithMessage("科目名称不能为空");
    }
}