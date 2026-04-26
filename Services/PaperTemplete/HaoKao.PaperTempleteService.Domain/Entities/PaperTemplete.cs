namespace HaoKao.PaperTempleteService.Domain.Entities;

public class PaperTemplete : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 试卷模板名称
    /// </summary>
    public string TempleteName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 适用科目
    /// </summary>
    public string SuitableSubjects { get; set; }

    /// <summary>
    /// 模板结构
    /// </summary>
    public string TempleteStructDatas { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}