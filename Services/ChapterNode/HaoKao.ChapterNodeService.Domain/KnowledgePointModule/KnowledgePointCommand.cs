using HaoKao.Common.Enums;

namespace HaoKao.ChapterNodeService.Domain.KnowledgePointModule;

/// <summary>
/// 创建知识点命令
/// </summary>
/// <param name="Name">名称</param>
/// <param name="ChapterNodeId">章节id</param>
/// <param name="ChapterNodeName">章节名称</param>
/// <param name="ExamFrequency">考试频率</param>
/// <param name="Remark">备注</param>
/// <param name="SubjectName">科目名称</param>
/// <param name="Sort">排序</param>
public record CreateKnowledgePointCommand(
    string Name,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    ExamFrequency ExamFrequency,
    string Remark,
    string SubjectName,
    int Sort
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

/// <summary>
/// 更新知识点命令
/// </summary>
/// <param name="Id">主键id</param>
/// <param name="Name">名称</param>
/// <param name="ChapterNodeId">章节Id</param>
/// <param name="ChapterNodeName">章节名称</param>
/// <param name="ExamFrequency">考试频率</param>
/// <param name="Remark">备注</param>
/// <param name="Sort">排序</param>
public record UpdateKnowledgePointCommand(
    Guid Id,
    string Name,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    ExamFrequency ExamFrequency,
    string Remark,
    int Sort
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

/// <summary>
/// 删除知识点命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteKnowledgePointCommand(Guid Id) : Command("删除知识点");