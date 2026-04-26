namespace HaoKao.StudyMaterialService.Domain.Entities;

/// <summary>
/// 学习资料
/// </summary>
[Comment("学习资料")]
public class StudyMaterial : AggregateRoot<Guid>,
                             ITenantShardingTable,
                             IIncludeMultiTenant<Guid>,
                             IIncludeCreateTime,
                             IIncludeUpdateTime,
                             IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 资料名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 资料内容
    /// </summary>
    public List<Material> Materials { get; set; }

    /// <summary>
    /// 科目
    /// </summary>
    public string Subjects { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public Guid CreatorId { get; set; }
}

public class Material
{
    /// <summary>
    /// 资料地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 资料名称
    /// </summary>
    public string Name { get; set; }
}