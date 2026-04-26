namespace HaoKao.DataDictionaryService.Domain.Entities;

public class Dictionaries : AggregateRoot<Guid>, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 分组编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 值名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父节点Id
    /// </summary>
    public Guid? Pid { get; set; }

    /// <summary>
    /// 父节点名称
    /// </summary>
    public string PName { get; set; }

    /// <summary>
    /// 节点全路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool? State { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; } = Guid.Empty;

    /// <summary>
    /// 子节点集合
    /// </summary>
    [ForeignKey("Pid")]
    public virtual List<Dictionaries> Children { get; set; } = [];
}