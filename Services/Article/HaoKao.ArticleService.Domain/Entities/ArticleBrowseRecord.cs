namespace HaoKao.ArticleService.Domain.Entities;

/// <summary>
/// 文章浏览记录
/// </summary>
public class ArticleBrowseRecord : AggregateRoot<Guid>, IIncludeCreateTime,IIncludeMultiTenant<Guid>,ITenantShardingTable
{
    /// <summary>
    /// 文章Id
    /// </summary>
    public Guid ArticleId { get; set; }
    /// <summary>
    /// 客户端唯一识别号
    /// </summary>
    public Guid ClientUniqueId { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}
