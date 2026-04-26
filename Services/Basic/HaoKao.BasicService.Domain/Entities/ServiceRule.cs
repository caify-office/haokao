namespace HaoKao.BasicService.Domain.Entities;

/// <summary>
/// 服务可用授权列表 (目前没用上)
/// </summary>
public class ServiceDataRule : AggregateRoot<Guid>
{
    /// <summary>
    /// 用户类型（为扩展而用）
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 实体说明
    /// </summary>
    public string EntityDesc { get; set; }

    /// <summary>
    /// 服务名称
    /// </summary>
    public string EntityTypeName { get; set; }

    /// <summary>
    /// 字段名称
    /// </summary>
    public string FieldName { get; set; }

    /// <summary>
    /// 字段的说明
    /// </summary>
    public string FieldDesc { get; set; }

    /// <summary>
    /// 字段类型（预留）
    /// </summary>
    public string FieldType { get; set; }

    /// <summary>
    /// 字段赋值
    /// </summary>
    public string FieldValue { get; set; }

    /// <summary>
    /// 表达式运算符
    /// </summary>
    public ExpressionType ExpressionType { get; set; }

    /// <summary>
    /// 所属标签
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }
}