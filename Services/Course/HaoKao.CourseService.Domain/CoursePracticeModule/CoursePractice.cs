using HaoKao.Common.Enums;

namespace HaoKao.CourseService.Domain.CoursePracticeModule;

/// <summary>
/// 课后练习
/// </summary>
public class CoursePractice : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 课程Id
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid CourseChapterId { get; set; }

    /// <summary>
    /// 关联的课程章节名称（阶段学习为课程章节名称,智慧辅助学习为课程名称）
    ///</summary>
    public string CourseChapterName { get; set; }

    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 练习类型
    /// </summary>
    public PracticeType PracticeType { get; set; }

    /// <summary>
    /// 知识点考试频率
    /// </summary>
    public ExamFrequency ExamFrequency { get; set; }

    /// <summary>
    /// 关联的试题章节id（阶段学习使用）
    /// </summary>
    public Guid? ChapterNodeId { get; set; }

    /// <summary>
    /// 关联的试题章节名称（阶段学习使用）
    ///</summary>
    public string ChapterNodeName { get; set; }

    /// <summary>
    /// 试题分类Id（阶段学习使用）
    /// </summary>
    public Guid? QuestionCategoryId { get; set; }

    /// <summary>
    /// 试题分类名称（阶段学习使用）
    ///</summary>
    public string QuestionCategoryName { get; set; }

    /// <summary>
    /// 试题配置(智辅学习课程，添加课后练习使用)
    /// </summary>
    public string QuestionConfig { get; set; }

    /// <summary>
    /// 试题数量
    /// </summary>
    public int QuestionCount { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
}

public enum PracticeType
{
    /// <summary>
    /// 阶段课程章节练习
    /// </summary>
    StageCourse = 0,

    /// <summary>
    /// 智辅课程章节练习
    /// </summary>
    AssistanceChapterPractice = 1,

    /// <summary>
    /// 智辅课程课后练习
    /// </summary>
    AssistanceAfterPractice = 2,
}