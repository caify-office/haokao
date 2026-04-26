namespace HaoKao.ProductService.Domain.Entities;

public class ProductPermission : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// 对应的科目Id
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 对应科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限名称(课程名称、题库分类名称、资料名称)
    /// </summary>
    public string PermissionName { get; set; }

    /// <summary>
    /// 权限Id(课程ID、题库分类ID、资料ID)
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// 所属分类(为课程时是阶段分类)
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    public Guid TenantId { get; set; }
}