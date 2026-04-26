namespace HaoKao.ProductService.Domain.Entities;

/// <summary>
/// 班级督学
/// </summary>
public class SupervisorClass : AggregateRoot<Guid>,
                               IIncludeMultiTenant<Guid>,
                               IIncludeCreateTime
{
    /// <summary>
    /// 班级名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 产品包id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 产品id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 营销人员Id
    /// </summary>
    public Guid SalespersonId { get; set; }

    /// <summary>
    /// 营销人员名称
    /// </summary>
    public string SalespersonName { get; set; }

    public List<SupervisorStudent> SupervisorStudents { get; set; } = [];

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}