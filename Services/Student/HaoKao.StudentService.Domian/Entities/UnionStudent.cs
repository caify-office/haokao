namespace HaoKao.StudentService.Domain.Entities;

public class UnionStudent : AggregateRoot<Guid>
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
}