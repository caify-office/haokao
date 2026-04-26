using HaoKao.LearningPlanService.Domain.Enumerations;

namespace HaoKao.LearningPlanService.Domain.Records;

/// <summary>
/// 章节练习任务，对应课后习题或章节测试
/// </summary>
public record ExerciseTaskContent
{
    /// <summary>
    /// 课程ID，关联到具体课程
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 课程类型（0:阶段课程 1：智辅课程）
    /// </summary>
    public CourseType CourseType { get; set; }

    /// <summary>
    /// 章节ID，标识练习所属章节
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterName { get; set; }

    /// <summary>
    /// 试题章节ID
    /// </summary>
    public Guid QuestionChapterId { get; set; }

    /// <summary>
    /// 试题章节名称
    /// </summary>
    public string QuestionChapterName { get; set; }

    /// <summary>
    /// 试题分类ID
    /// </summary>
    public Guid QuestionCategoryId { get; set; }

    /// <summary>
    /// 试题分类名称，如"章节练习"、"章节强化"
    /// </summary>
    public string QuestionCategoryName { get; set; }

    /// <summary>
    /// 练习包含的题目数量
    /// </summary>
    public int QuestionCount { get; set; }
}