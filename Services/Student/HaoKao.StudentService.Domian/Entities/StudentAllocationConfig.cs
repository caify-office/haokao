namespace HaoKao.StudentService.Domain.Entities;

public class StudentAllocationConfig : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>, IIncludeUpdateTime
{
    /// <summary>
    /// 配置数据
    /// </summary>
    public HashSet<PercentageAllocation> Data { get; set; }

    /// <summary>
    /// 分配方式
    /// </summary>
    public WaysOfAllocation WaysOfAllocation { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}

public enum WaysOfAllocation
{
    /// <summary>
    /// 平均分配
    /// </summary>
    Average,

    /// <summary>
    /// 自定义分配
    /// </summary>
    Custom,
}

/// <summary>
/// 百分比分配
/// </summary>
public record PercentageAllocation
{
    /// <summary>
    /// 销售Id
    /// </summary>
    public Guid SalespersonId { get; init; }

    /// <summary>
    /// 销售名称
    /// </summary>
    public string SalespersonName { get; init; }

    /// <summary>
    /// 销售企业微信Id
    /// </summary>
    public string EnterpriseWeChatId { get; init; }

    /// <summary>
    /// 百分比
    /// </summary>
    public decimal Percentage { get; init; }
}