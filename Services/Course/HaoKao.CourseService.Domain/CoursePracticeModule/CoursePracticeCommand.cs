using HaoKao.Common.Enums;

namespace HaoKao.CourseService.Domain.CoursePracticeModule;

/// <summary>
/// 创建课后练习命令
/// </summary>
/// <param name="SubjectId">科目id</param>
/// <param name="CourseChapterId">关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）</param>
/// <param name="CourseChapterName"></param>
/// <param name="ChapterNodeId">试题章节id</param>
/// <param name="ChapterNodeName">试题章节名称</param>
/// <param name="QuestionCategoryId">试题分类Id</param>
/// <param name="QuestionCategoryName">试题分类名称</param>
public record CreateCoursePracticeCommand(
    Guid SubjectId,
    Guid CourseChapterId,
    string CourseChapterName,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    Guid? QuestionCategoryId,
    string QuestionCategoryName
) : Command("创建课后练习")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CourseChapterName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => ChapterNodeName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => QuestionCategoryName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 保存智辅课程练习命令
/// </summary>
/// <param name="SubjectId">科目id</param>
/// <param name="CourseId">课程id</param>
/// <param name="PracticeType">练习类型</param>
/// <param name="CourseChapterId">关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）</param>
/// <param name="KnowledgePointId">知识点Id</param>
/// <param name="ExamFrequency">考试频率</param>
/// <param name="ChapterNodeId">试题章节id</param>
/// <param name="ChapterNodeName">试题章节名称</param>
/// <param name="QuestionCategoryId">试题分类Id</param>
/// <param name="QuestionCategoryName">试题分类名称</param>
/// <param name="CourseChapterName">章节名称</param>
/// <param name="QuestionConfig">试题配置</param>
/// <param name="QuestionCount">试题数量</param>
public record SaveAssistantCoursePracticeCommand(
    Guid SubjectId,
    Guid CourseId,
    PracticeType PracticeType,
    Guid CourseChapterId,
    Guid KnowledgePointId,
    ExamFrequency ExamFrequency,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    Guid? QuestionCategoryId,
    string QuestionCategoryName,
    string CourseChapterName,
    string QuestionConfig,
    int QuestionCount
) : Command("保存智辅课程练习命令")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CourseChapterName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 更新课后练习命令
/// </summary>
/// <param name="Id">主键</param>
/// <param name="SubjectId">科目id</param>
/// <param name="CourseChapterId">关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）</param>
/// <param name="CourseChapterName"></param>
/// <param name="ChapterNodeId">试题章节id</param>
/// <param name="ChapterNodeName">试题章节名称</param>
/// <param name="QuestionCategoryId">试题分类Id</param>
/// <param name="QuestionCategoryName">试题分类名称</param>
public record UpdateCoursePracticeCommand(
    Guid Id,
    Guid SubjectId,
    Guid CourseChapterId,
    string CourseChapterName,
    Guid? ChapterNodeId,
    string ChapterNodeName,
    Guid? QuestionCategoryId,
    string QuestionCategoryName
) : Command("更新课后练习")
{
    public override void AddFluentValidationRule<TCommand>(AbstractValidator<TCommand> validator)
    {
        validator.RuleFor(x => CourseChapterName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => ChapterNodeName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");

        validator.RuleFor(x => QuestionCategoryName)
                 .NotEmpty().WithMessage("不能为空")
                 .MaximumLength(50).WithMessage("长度不能大于50")
                 .MinimumLength(2).WithMessage("长度不能小于2");
    }
}

/// <summary>
/// 删除课后练习命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteCoursePracticeCommand(Guid Id) : Command("删除课后练习");