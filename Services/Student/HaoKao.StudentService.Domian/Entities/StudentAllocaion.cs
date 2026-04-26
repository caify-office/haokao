namespace HaoKao.StudentService.Domain.Entities;

/// <summary>
/// 学员分配
/// </summary>
public class StudentAllocation : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, ITenantShardingTable
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
    /// 分配销售Id
    /// </summary>
    public Guid SalespersonId { get; set; }

    /// <summary>
    /// 分配销售姓名
    /// </summary>
    public string SalespersonName { get; set; }

    /// <summary>
    /// 企业微信Id
    /// </summary>
    public string EnterpriseWeChatId { get; set; }

    /// <summary>
    /// 分配时间
    /// </summary>
    public DateTime AllocationTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 学员
    /// </summary>
    [NotMapped]
    public Student Student { get; init; }
}