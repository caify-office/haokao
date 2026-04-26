namespace HaoKao.AgreementService.Domain.Entities;

/// <summary>
/// 课程协议
/// </summary>
[Comment("课程协议")]
public class CourseAgreement : AggregateRoot<Guid>,
                               ITenantShardingTable,
                               IIncludeMultiTenant<Guid>,
                               IIncludeCreateTime,
                               IIncludeUpdateTime,
                               IIncludeCreatorId<Guid>
{
    /// <summary>
    /// 协议名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 协议内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 续读次数
    /// </summary>
    public int Continuation { get; set; }

    /// <summary>
    /// 协议类型
    /// </summary>
    public AgreementType AgreementType { get; set; }

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

/// <summary>
/// 协议类型
/// </summary>
public enum AgreementType
{
    /// <summary>
    /// 公共协议
    /// </summary>
    [Description("公共协议")]
    Standard,

    /// <summary>
    /// 续读协议
    /// </summary>
    [Description("续读协议")]
    Continuation,
}