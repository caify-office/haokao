namespace HaoKao.BasicService.Domain.Entities;

/// <summary>
/// 系统服务可用操作权限列表
/// </summary>
public class ServicePermission : AggregateRoot<Guid>
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
    public List<OperationPermissionModel> OperationPermissions { get; set; }

    /// <summary>
    /// 枚举值 (这个没用上可以忽略)
    /// </summary>
    public Dictionary<string, string> Permissions { get; set; }
}