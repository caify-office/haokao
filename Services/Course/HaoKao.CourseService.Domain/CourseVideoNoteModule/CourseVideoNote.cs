namespace HaoKao.CourseService.Domain.CourseVideoNoteModule;

/// <summary>
/// 课程视频笔记
/// </summary>
public class CourseVideoNote : AggregateRoot<Guid>,
                               IIncludeCreateTime,
                               IIncludeUpdateTime,
                               IIncludeCreatorName,
                               IIncludeCreatorId<Guid>,
                               IIncludeMultiTenant<Guid>,
                               ITenantShardingTable
{
    /// <summary>
    /// 视频id
    /// </summary>
    public string VideoId { get; set; }

    /// <summary>
    /// 视频时间节点
    /// </summary>
    public decimal TimeNode { get; set; }

    /// <summary>
    /// 笔记类型
    /// </summary>
    public CourseVideoNoteType CourseVideoNoteType { get; set; }

    /// <summary>
    /// 笔记内容
    /// </summary>
    public string NoteContent { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 创建者id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }
}