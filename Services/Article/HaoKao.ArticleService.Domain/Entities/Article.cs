namespace HaoKao.ArticleService.Domain.Entities;

/// <summary>
/// 文章
/// </summary>
public class Article : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// 所属栏目(字典Id)
    /// </summary>
    public Guid Column { get; set; }
    /// <summary>
    /// 所属类别(字典Id)
    /// </summary>
    public Guid Category { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTopping { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否首页展示
    /// </summary>
    public bool IsDisplayedOnHomepage { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublish { get; set; }
    /// <summary>
    /// 预览图
    /// </summary>

    public string PreviewUrl { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
    /// <summary>
    /// 是否外部链接
    /// </summary>
    public bool IsExternalURL { get; set; }
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
