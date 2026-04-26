using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaoKao.WebsiteConfigurationService.Domain.Models;

/// <summary>
/// 栏目
/// </summary>
public class Column : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>
{
    public Column()
    {
        Children = [];
    }
    /// <summary>
    /// 域名
    /// </summary>
    public string DomainName { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 别名
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// 英文名
    /// </summary>
    public string EnglishName { get; set; }

    /// <summary>
    /// 父节点id
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 当前图标
    /// </summary>
    public string ActiveIcon { get; set; }

    /// <summary>
    /// 子节点集合
    /// </summary>
    [ForeignKey("ParentId")]
    public virtual List<Column> Children { get; set; }
    /// <summary>
    /// 是否首页
    /// </summary>
    public bool IsHomePage { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

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
