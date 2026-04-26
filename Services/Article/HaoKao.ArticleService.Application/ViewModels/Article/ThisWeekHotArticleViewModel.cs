namespace HaoKao.ArticleService.Application.ViewModels.Article;


[AutoMapFrom(typeof(Domain.Entities.Article))]
public class ThisWeekHotArticleViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 所属类别(字典Id)
    /// </summary>
    public Guid Category { get; set; }
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

}
