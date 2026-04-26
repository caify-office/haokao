using Girvs.Extensions;
using Girvs.Infrastructure;

namespace HaoKao.WebsiteConfigurationService.Domain.Queries;

public class TemplateStyleQuery : QueryBase<TemplateStyle>
{   /// <summary>
    /// 域名
    /// </summary>
    [QueryCacheKey]   
    public string DomainName { get; set; }
    /// <summary>
    /// 租户Id
    /// </summary>
    [QueryCacheKey]
    public Guid? TenantId 
    {
        get
        {
            //前端查询时，不需要登录，这里需要获取租户Id执行过滤
           return  EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
        }
    }
    public override Expression<Func<TemplateStyle, bool>> GetQueryWhere()
    {

        Expression<Func<TemplateStyle, bool>> expression = x => true;
        if (TenantId.HasValue)
        {
            expression = expression.And(x => x.TenantId == TenantId);
        }
        if (!string.IsNullOrEmpty(DomainName))
        {
            expression = expression.And(x => x.DomainName == DomainName);
        }
        return expression;
    }
}
