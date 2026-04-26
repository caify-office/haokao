using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Domain.Entities;

public class StudentPermissionOperateLog : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeCreatorId<Guid>, IIncludeCreateTime
{
    /// <summary>
    /// 学员权限Id
    /// </summary>
    public Guid StudentPermissionId { get; init; }

    /// <summary>
    /// 学员昵称(即用户昵称)
    /// </summary>
    public string StudentName { get; init; }

    /// <summary>
    /// 学员ID（即用户ID）
    /// </summary>
    public Guid StudentId { get; init; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; init; }

    /// <summary>
    /// 产品类别
    /// </summary>
    public ProductType ProductType { get; init; }

    /// <summary>
    /// 原到期时间
    /// </summary>
    public DateTime? OldExpiredTime { get; init; }

    /// <summary>
    /// 现到期时间
    /// </summary>
    public DateTime NewExpiredTime { get; init; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}