namespace HaoKao.ArticleService.Application.ViewModels.Article;


[AutoMapFrom(typeof(ArticleQuery))]
[AutoMapTo(typeof(ArticleQuery))]
public class ArticleQueryViewModel : QueryDtoBase<ArticleQueryListViewModel>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 所属栏目(字典Id)
    /// </summary>
    public Guid? Column { get; set; }
    /// <summary>
    /// 所属类别
    /// </summary>
    public Guid? Category { get; set; }

    /// <summary>
    /// 是否首页展示
    /// </summary>
    public bool? IsDisplayedOnHomepage { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool? IsPublish { get; set; }
}

[AutoMapFrom(typeof(Domain.Entities.Article))]
[AutoMapTo(typeof(Domain.Entities.Article))]
public class ArticleQueryListViewModel : IDto
{
    public Guid Id { get; set; } 
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
}