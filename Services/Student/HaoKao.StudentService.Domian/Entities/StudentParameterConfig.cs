namespace HaoKao.StudentService.Domain.Entities;

/// <summary>
/// 学员参数设置
/// </summary>
public class StudentParameterConfig : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable, IIncludeMultiTenantName
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 设置值字段名称
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// 设置值类型
    /// </summary>
    public string PropertyType { get; set; }

    /// <summary>
    /// 设置值
    /// </summary>
    public string PropertyValue { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 租户名
    /// </summary>
    public string TenantName { get; set; }
}