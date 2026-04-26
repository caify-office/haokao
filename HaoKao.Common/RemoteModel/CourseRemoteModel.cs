using Girvs.BusinessBasis.Dto;

namespace HaoKao.Common.RemoteModel;

#region 课程视频模型

public record BrowseCourseVideoInfo : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid CourseChapterId { get; set; }

    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的知识点id(阶段学习保存多个知识点id拼接数组使用)
    /// </summary>
    public string KnowledgePointIds { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix { get; init; }

    /// <summary>
    /// 时长
    /// </summary>
    public long Duration { get; init; }

    /// <summary>
    /// 是否试听  ture--试听 false --不可试听
    /// </summary>
    public bool IsTry { get; init; }

    /// <summary>
    /// 视频名称
    /// </summary>
    public string VideoName { get; init; }

    /// <summary>
    /// 视频源名称
    /// </summary>
    public string SourceName { get; init; }

    /// <summary>
    /// 播放url-冗余
    /// </summary>
    public string VideoUrl { get; init; }

    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; init; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; init; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 视频分类id
    /// </summary>
    public long? CateId { get; set; }

    /// <summary>
    /// 视频分类名称
    /// </summary>
    public string CateName { get; set; }

    /// <summary>
    /// 视频标签
    /// </summary>
    public string Tags { get; set; }
}

public record CourseVideoQueryListInfo : BrowseCourseVideoInfo
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

public class QueryCourseVideoInfo : QueryDtoBase<CourseVideoQueryListInfo>
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

#endregion

#region 课程章节练习模型

public record BrowseCoursePracticeInfo : IDto
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
    /// 试题id集合(智辅学习课程，添加课后练习使用)
    /// </summary>
    public List<Guid> QuestionIds { get; set; }
}

#endregion

#region 课程章节模型

public record BrowseCourseChapterViewModel : IDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 父id
    /// </summary>
    public Guid ParentId { get; init; }

    /// <summary>
    /// 关联的课程id
    /// </summary>
    public Guid CourseId { get; init; }
}

#endregion