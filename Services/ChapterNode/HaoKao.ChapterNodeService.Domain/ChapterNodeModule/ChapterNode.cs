using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

/// <summary>
/// 章节
/// </summary>
public class ChapterNode : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 所属科目
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 父级名称
    /// </summary>
    public string ParentName { get; set; }

    /// <summary>
    /// 父级Id集合
    /// </summary>
    public List<Guid> ParentIds { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 从题库迁移的数据
    /// </summary>
    public string PropertyValueID { get; set; }

    /// <summary>
    /// 子章节
    /// </summary>
    [NotMapped]
    public List<ChapterNode> Children { get; set; }

    /// <summary>
    /// 章节下知识点树
    /// </summary>
    [NotMapped]
    public List<KnowledgePoint> KnowledgePoints { get; set; }
}