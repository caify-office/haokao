using Girvs.Extensions;

namespace HaoKao.WebsiteConfigurationService.Domain.Queries;

public class ColumnQuery : QueryBase<Column>
{
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string DomainName { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    [QueryCacheKey]
    public string Name { get; set; }
    public override Expression<Func<Column, bool>> GetQueryWhere()
    {
        Expression<Func<Column, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(DomainName))
        {
            expression = expression.And(x => x.DomainName.Contains(DomainName));
        }
        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.Name.Contains(Name));
        }
        return expression;
    }
}
