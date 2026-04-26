namespace HaoKao.StudentService.Domain.Entities;

public class StudentFollow : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeCreateTime, ITenantShardingTable
{
    /// <summary>
    /// 学员Id
    /// </summary>
    public Guid StudentId { get; init; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid RegisterUserId { get; init; }

    /// <summary>
    /// 微信UnionId
    /// </summary>
    public string UnionId { get; init; }

    /// <summary>
    /// 销售Id
    /// </summary>
    public Guid SalespersonId { get; set; }

    /// <summary>
    /// 销售名称
    /// </summary>
    public string SalespersonName { get; set; }

    /// <summary>
    /// 销售企业微信Id
    /// </summary>
    public string EnterpriseWeChatId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}