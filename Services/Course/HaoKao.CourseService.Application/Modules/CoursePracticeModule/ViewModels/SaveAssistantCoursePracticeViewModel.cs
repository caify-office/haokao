using HaoKao.Common.Enums;
using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;

[AutoMapTo(typeof(SaveAssistantCoursePracticeCommand))]
public record SaveAssistantCoursePracticeViewModel : IDto
{
    /// <summary>
    /// 科目id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 课程id
    /// </summary>
    [DisplayName("课程id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseId { get; init; }

    /// <summary>
    /// 练习类型
    /// </summary>
    [DisplayName("练习类型")]
    [Required(ErrorMessage = "{0}不能为空")]
    public PracticeType PracticeType { get; init; }

    /// <summary>
    /// 关联的课程章节id
    /// </summary>
    [DisplayName("关联的课程章节id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid CourseChapterId { get; init; }

    /// <summary>
    /// 关联的课程章节名称
    /// </summary>
    [DisplayName("关联的课程章节名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string CourseChapterName { get; init; }

    /// <summary>
    /// 关联的知识点id（智辅课程专用）
    /// </summary>
    [DisplayName("关联的知识点id（智辅课程专用）")]
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 考试频率
    /// </summary>
    [DisplayName("考试频率")]
    [Required(ErrorMessage = "{0}不能为空")]
    public ExamFrequency ExamFrequency { get; init; }

    /// <summary>
    /// 关联的试题章节id（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题章节id")]
    public Guid? ChapterNodeId { get; init; }

    /// <summary>
    /// 关联的试题章节名称（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题章节名称")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string ChapterNodeName { get; init; }

    /// <summary>
    /// 关联的试题分类Id（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题分类Id")]
    public Guid? QuestionCategoryId { get; init; }

    /// <summary>
    /// 关联的试题分类名称（阶段课程专用）
    /// </summary>
    [DisplayName("关联的试题分类名称")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string QuestionCategoryName { get; init; }

    /// <summary>
    /// 试题配置(智辅学习课程专用)
    /// </summary>
    [DisplayName("试题id集合(智辅学习课程，添加课后练习使用)")]
    public string QuestionConfig { get; set; } = null;

    /// <summary>
    /// 试题数量(智辅学习课程专用)
    /// </summary>
    [DisplayName("试题id集合(智辅学习课程，添加课后练习使用)")]
    public int QuestionCount { get; set; } = 0;
}