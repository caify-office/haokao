namespace HaoKao.ProductService.Domain.Entities;

/// <summary>
/// 关联产品
/// </summary>
public class RelatedProduct : AggregateRoot<Guid>,
                              IIncludeMultiTenant<Guid>,
                              IIncludeCreatorName,
                              IIncludeCreateTime
{
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Product Product { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 关联对象产品id
    /// </summary>
    public Guid RelatedTargetProductId { get; set; }

    /// <summary>
    /// 关联对象产品名称
    /// </summary>
    public string RelatedTargetProducName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}