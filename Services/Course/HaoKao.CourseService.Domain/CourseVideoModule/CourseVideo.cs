namespace HaoKao.CourseService.Domain.CourseVideoModule;

/// <summary>
/// 课程视频
/// </summary>
public class CourseVideo : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 关联的课程章节id（阶段学习为课程章节id,智慧辅助学习为课程Id）
    /// </summary>
    public Guid CourseChapterId { get; set; }

    /// <summary>
    /// 关联的知识点id
    /// </summary>
    public Guid KnowledgePointId { get; set; }

    /// <summary>
    /// 关联的知识点id(阶段学习，保存多个知识点id拼接数组)
    /// </summary>
    public string KnowledgePointIds { get; set; }

    /// <summary>
    /// 后缀
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// 时长
    /// </summary>
    public decimal Duration { get; set; }

    /// <summary>
    /// 是否试听  ture--试听 false --不可试听
    /// </summary>
    public bool IsTry { get; set; }

    /// <summary>
    /// 视频名称
    /// </summary>
    public string VideoName { get; set; }

    /// <summary>
    /// 前缀name
    /// </summary>
    public string QzName { get; set; }

    /// <summary>
    /// 视频源名称
    /// </summary>
    public string SourceName { get; set; }

    /// <summary>
    /// 播放url-冗余
    /// </summary>
    public string VideoUrl { get; set; }

    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

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