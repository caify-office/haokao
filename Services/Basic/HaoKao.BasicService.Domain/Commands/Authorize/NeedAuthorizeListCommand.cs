namespace HaoKao.BasicService.Domain.Commands.Authorize;

/// <summary>
/// 初始化服务需要授权列表
/// </summary>
/// <param name="ServicePermissionCommandModels">操作权限</param>
/// <param name="ServiceDataRuleCommandModels">数据权限</param>
public record NeedAuthorizeListCommand(
    List<ServicePermissionCommandModel> ServicePermissionCommandModels,
    List<ServiceDataRuleCommandModel> ServiceDataRuleCommandModels
) : Command("初始化服务需要授权列表");

public class ServicePermissionCommandModel
{
    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// 服务Id
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// 所属标签
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 所属的子系统模块
    /// </summary>
    public SystemModule FuncModule { get; set; }

    /// <summary>
    /// 其它相关参数
    /// </summary>
    public string[] OtherParams { get; set; }

    /// <summary>
    /// 方法权限
    /// </summary>
    public List<OperationPermissionModel> OperationPermissionModels { get; set; }

    /// <summary>
    /// 枚举值 (这个没用上可以忽略)
    /// </summary>
    public Dictionary<string, string> Permissions { get; set; }
}

public class ServiceDataRuleCommandModel
{
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
    /// 用户类型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 所属标签
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }
}