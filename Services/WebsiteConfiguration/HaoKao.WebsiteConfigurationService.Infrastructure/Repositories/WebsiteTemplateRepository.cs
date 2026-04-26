using Girvs.BusinessBasis.Queries;
using Girvs.Extensions;
using Girvs.Extensions.Collections;
using HaoKao.WebsiteConfigurationService.Domain.Enumerations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Repositories;

public class WebsiteTemplateRepository : Repository<WebsiteTemplate>, IWebsiteTemplateRepository
{
    /// <summary>
    /// 获取当前域名，当且英文名下的模板
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public Task<List<Tuple<Guid, string, WebsiteTemplateType, string, Guid, string, bool>>>  GetWebsiteTemplateByDominNameAneEnglishName(string domainName, string englishName)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().ToGuid();
        var dbContext = EngineContext.Current.Resolve<WebsiteConfigurationDbContext>();
        Expression<Func<Column, bool>> expression = c => c.TenantId==tenantId&&c.DomainName == domainName;
        if (string.IsNullOrEmpty(englishName))
        {
            expression = expression.And(c => c.IsHomePage == true);
        }
        else
        {
            expression = expression.And(c => c.EnglishName == englishName);
        }
        var colums =  dbContext.Columns
                            .Where(expression)
                            .Select(x=>new 
                            {
                               x.Id
                            });
        var result = from t in Queryable
                     from c in colums.Where(c1=>t.ColumnId==c1.Id)
                     select new Tuple<Guid,string, WebsiteTemplateType, string,Guid,string, bool>
                     (
                         t.Id,
                         t.Name,
                         t.WebsiteTemplateType,
                         t.Desc,
                         t.ColumnId,
                         t.ColumnName,
                         t.IsDefault
                         ) ;
        return result.ToListAsync();
    }
    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="domainName"></param>
    /// <returns></returns>
    public Task<List<WebsiteTemplate>> GetByDomainNameAsync(string domainName)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().ToGuid();
        var dbContext = EngineContext.Current.Resolve<WebsiteConfigurationDbContext>();
        Expression<Func<Column, bool>> expression = c => c.TenantId == tenantId && c.DomainName == domainName;

        var colums = dbContext.Columns
                            .Where(expression)
                            .Select(x => new
                            {
                                x.Id
                            });
        var result = from t in Queryable
                     from c in colums.Where(c1 => t.ColumnId == c1.Id)
                     select t;
        return result.ToListAsync();
    }
    public override async Task<List<WebsiteTemplate>> GetByQueryAsync(QueryBase<WebsiteTemplate> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}
