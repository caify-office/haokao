namespace HaoKao.CourseService.Domain.VideoStorageModule;

public class VideoStorage : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 视频存储器ID
    /// </summary>
    public Guid VideoStorageHandlerId { get; set; }

    /// <summary>
    /// 视频存储器名称
    /// </summary>
    public string VideoStorageHandlerName { get; set; }

    /// <summary>
    /// 相关的配置参数
    /// </summary>
    public string ConfigParameter { get; set; }

    /// <summary>
    /// 所属租户
    /// </summary>
    public Guid TenantId { get; set; }
}