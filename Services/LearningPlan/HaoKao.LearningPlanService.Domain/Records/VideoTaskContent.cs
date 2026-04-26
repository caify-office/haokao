using HaoKao.LearningPlanService.Domain.Enumerations;

namespace HaoKao.LearningPlanService.Domain.Records;

/// <summary>
/// 视频学习任务，对应在线视频课程学习
/// </summary>
public record VideoTaskContent
{
    /// <summary>
    /// 课程ID，关联到具体课程
    /// </summary>
    public Guid CourseId { get; set; }

    /// <summary>
    /// 课程名称，关联到具体课程
    /// </summary>
    public string CourseName { get; set; }

    /// <summary>
    /// 课程类型（0:阶段课程 1：智辅课程）
    /// </summary>
    public CourseType CourseType { get; set; }

    /// <summary>
    /// 章节ID，标识视频所属章节
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterName { get; set; }

    /// <summary>
    /// 知识点id(智辅课程专用)
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 知识点名称(智辅课程专用)
    /// </summary>
    public string KnowledgePointName { get; set; }

    /// <summary>
    /// 知识点考试频率(智辅课程专用)
    /// </summary>
    public ExamFrequency KnowledgePointExamFrequency { get; set; }

    /// <summary>
    /// 视频资源ID
    /// </summary>
    public string VideoId { get; set; }

    /// <summary>
    /// 视频名称
    /// </summary>
    public string VideoName { get; set; }
}