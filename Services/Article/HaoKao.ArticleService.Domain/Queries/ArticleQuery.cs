using Girvs.Extensions;
using HaoKao.ArticleService.Domain.Entities;

namespace HaoKao.ArticleService.Domain.Queries;

public class ArticleQuery : QueryBase<Article>
{
    /// <summary>
    /// 标题
    /// </summary>
    [QueryCacheKey]
    public string Title { get; set; }

    /// <summary>
    /// 所属栏目(字典Id)
    /// </summary>
    [QueryCacheKey]
    public Guid? Column { get; set; }
    /// <summary>
    /// 所属类别
    /// </summary>
    [QueryCacheKey]
    public Guid? Category { get; set; }

    /// <summary>
    /// 是否首页展示
    /// </summary>
    [QueryCacheKey]
    public bool? IsDisplayedOnHomepage { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    [QueryCacheKey]
    public bool? IsPublish { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [QueryCacheKey]
    public Guid? TenantId { get; set; }
    public override Expression<Func<Article, bool>> GetQueryWhere()
    {
        Expression<Func<Article, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Title))
        {
            expression = expression.And(x => x.Title.Contains(Title));
        }

        if (Column.HasValue)
        {
            expression = expression.And(x => x.Column == Column);
        }

        if (Category.HasValue)
        {
            expression = expression.And(x => x.Category==Category);
        }
        if (IsDisplayedOnHomepage.HasValue)
        {
            expression = expression.And(x => x.IsDisplayedOnHomepage == IsDisplayedOnHomepage);
        }
        if (IsPublish.HasValue)
        {
            expression = expression.And(x => x.IsPublish == IsPublish);
        }
        if (TenantId.HasValue)
        {
            expression = expression.And(x => x.TenantId == TenantId);
        }
        return expression;
    }
}
