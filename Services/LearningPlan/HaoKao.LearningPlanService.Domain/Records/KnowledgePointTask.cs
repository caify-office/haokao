using HaoKao.LearningPlanService.Domain.Enumerations;

namespace HaoKao.LearningPlanService.Domain.Records;

public class KnowledgePointTask
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
    /// 章节ID，标识所属章节（最外层的章用于获取章节练习）
    /// </summary>
    public Guid ChapterId { get; set; }

    /// <summary>
    /// 章节名称（最外层的章用于获取章节练习）
    /// </summary>
    public string ChapterName { get; set; }

    /// <summary>
    /// 视频信息
    /// </summary>
    public ICollection<VideoInfoViewModel> VideoInfoViewModels { get; set; }
}

public record VideoInfoViewModel
{
    /// <summary>
    /// 知识点id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 知识点名称
    /// </summary>
    public string KnowledgePointName { get; set; }

    /// <summary>
    /// 知识点考试频率
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

    /// <summary>
    /// 视频原始时长（秒），支持小数表示
    /// </summary>
    public decimal VideoDurationSeconds { get; set; }
}