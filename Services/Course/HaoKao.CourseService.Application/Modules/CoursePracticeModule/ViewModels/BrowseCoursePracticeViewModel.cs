using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

[AutoMapFrom(typeof(CoursePractice))]
public record BrowseCoursePracticeViewModel : IDto
{
    public Guid Id { get; init; }

    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 关联的课程章节id
    /// </summary>
    public Guid CourseChapterId { get; init; }

    /// <summary>
    ///  关联的课程章节名称
    /// </summary>
    public string CourseChapterName { get; init; }

    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的试题章节id
    /// </summary>
    public Guid? ChapterNodeId { get; init; }

    /// <summary>
    /// 关联的试题章节名称
    /// </summary>
    public string ChapterNodeName { get; init; }

    /// <summary>
    /// 关联的试题分类Id
    /// </summary>
    public Guid? QuestionCategoryId { get; init; }

    /// <summary>
    /// 关联的试题分类名称
    /// </summary>
    public string QuestionCategoryName { get; init; }

    /// <summary>
    /// 试题配置(智辅学习课程，添加课后练习使用)
    /// </summary>
    public string QuestionConfig { get; set; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; set; }
}

public record ExamFrequencyQuestionCountViewModel
{
    /// <summary>
    /// 高频知识点统计
    /// </summary>
    public int High { get; set; }

    /// <summary>
    /// 中频知识点统计
    /// </summary>
    public int Medium { get; set; }

    /// <summary>
    /// 低频知识点统计
    /// </summary>
    public int Low { get; set; }
}