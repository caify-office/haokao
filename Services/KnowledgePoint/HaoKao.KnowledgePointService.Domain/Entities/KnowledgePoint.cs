using System;

namespace HaoKao.KnowledgePointService.Domain.Entities;

/// <summary>
/// 知识点
/// </summary>
public class KnowledgePoint : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 知识点名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 科目名称
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 章节id
    /// </summary>
    public Guid? ChapterNodeId { get; set; }

    /// <summary>
    /// 章节名称
    /// </summary>
    public string ChapterNodeName { get; set; }

    /// <summary>
    /// 知识点备注信息
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    public Guid TenantId { get; set; }
}