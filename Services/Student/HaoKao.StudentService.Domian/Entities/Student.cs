using HaoKao.StudentService.Domain.Models;

namespace HaoKao.StudentService.Domain.Entities;

/// <summary>
/// 学员
/// </summary>
public class Student : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeCreateTime, ITenantShardingTable
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid RegisterUserId { get; set; }

    /// <summary>
    /// 是否付费学员
    /// </summary>
    public bool IsPaidStudent { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 注册用户
    /// </summary>
    [NotMapped]
    public RegisterUser RegisterUser { get; init; }

    /// <summary>
    /// 已加销售
    /// </summary>
    [NotMapped]
    public IReadOnlyList<StudentFollow> StudentFollows { get; init; }
}