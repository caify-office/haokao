using Girvs.Extensions;

namespace HaoKao.WebsiteConfigurationService.Domain.Queries;

public class WebsiteTemplateQuery : QueryBase<WebsiteTemplate>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [QueryCacheKey]
    public string Content { get; set; }

    /// <summary>
    /// 所属栏目Id
    /// </summary>
    [QueryCacheKey]
    public Guid? ColumnId { get; set; }

    /// <summary>
    /// 所属栏目名称
    /// </summary>
    [QueryCacheKey]
    public string ColumnName { get; set; }



    /// <summary>
    /// 是否默认
    /// </summary>
    [QueryCacheKey]
    public bool? IsDefault { get; set; }
    public override Expression<Func<WebsiteTemplate, bool>> GetQueryWhere()
    {
        Expression<Func<WebsiteTemplate, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }

        if (!string.IsNullOrEmpty(Content))
        {
            expression = expression.And(x => x.Content.Contains(Content));
        }

        //if (ColumnId.HasValue)
        {
            expression = expression.And(x => x.ColumnId == ColumnId);
        }

        if (!string.IsNullOrEmpty(ColumnName))
        {
            expression = expression.And(x => x.ColumnName.Contains(ColumnName));
        }

      

        if (IsDefault.HasValue)
        {
            expression = expression.And(x => x.IsDefault == IsDefault);
        }

        return expression;
    }
}
