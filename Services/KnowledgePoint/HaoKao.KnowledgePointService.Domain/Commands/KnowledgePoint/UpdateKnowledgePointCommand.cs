using FluentValidation;
using System;

namespace HaoKao.KnowledgePointService.Domain.Commands.KnowledgePoint;

/// <summary>
/// 更新知识点命令
/// </summary>
/// <param name="Id">主键id</param>
/// <param name="Name">名称</param>
/// <param name="ChapterNodeId">章节Id</param>
/// <param name="ChapterNodeName">章节名称</param>
/// <param name="Remark">备注</param>
public record UpdateKnowledgePointCommand(
    Guid Id,
    string Name,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    string Remark
) : Command("更新知识点")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => Name)
                 .NotEmpty().WithMessage("知识点名称不能为空");

        validator.RuleFor(x => ChapterNodeId)
                 .NotEmpty().WithMessage("章节Id不能为空");

        validator.RuleFor(x => ChapterNodeName)
                 .NotEmpty().WithMessage("章节名称不能为空");
    }
}