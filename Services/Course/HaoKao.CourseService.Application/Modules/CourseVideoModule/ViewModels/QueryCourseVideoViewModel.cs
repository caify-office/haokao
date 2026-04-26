using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Application.Modules.CourseVideoModule.ViewModels;

[AutoMapFrom(typeof(CourseVideoQuery))]
[AutoMapTo(typeof(CourseVideoQuery))]
public class QueryCourseVideoViewModel : QueryDtoBase<CourseVideoQueryListViewModel>
{
    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid? KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid? CourseChapterId { get; set; }

    /// <summary>
    /// 课程id（阶段学习专用）
    /// </summary>
    public Guid? CourseId { get; set; }
}

[AutoMapFrom(typeof(CourseVideo))]
[AutoMapTo(typeof(CourseVideo))]
public record CourseVideoQueryListViewModel : BrowseCourseVideoViewModel
{
    /// <summary>
    /// 章节名称
    /// </summary>
    public string CourseChapterName { get; set; }

    /// <summary>
    /// 章节排序
    /// </summary>
    public int CourseChapterSort { get; set; }

    /// <summary>
    /// 前缀name
    /// </summary>
    public string QzName { get; set; }

    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否最新的
    /// </summary>
    public bool IsNew { get; set; }
}

public record QueryByIds(string Ids) : IDto;

public record QueryKnowledgePointConfigInfoViewModel(Guid CourseChapterId, List<KnowledgePointConfigInfo> Result) : IDto;

public class KnowledgePointConfigInfo : IDto
{
    public Guid? Id { get; set; }

    public Guid KnowledgePointId { get; set; }

    public string KnowledgePointName { get; set; }

    public string VideoName { get; set; }

    public decimal? Duration { get; set; }

    public string QuestionConfig { get; set; }

    public int QuestionCount { get; set; }

    public string FileUrl { get; set; }

    public string Name { get; set; }
}