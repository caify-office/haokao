using Girvs.BusinessBasis.Queries;
using Girvs.Extensions;
using Girvs.Extensions.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HaoKao.WebsiteConfigurationService.Infrastructure.Repositories;

public class ColumnRepository : Repository<Column>, IColumnRepository
{
    /// <summary>
    /// 根据域名/父Id获取指定栏目的直接子栏目
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public async Task<List<ColumnTreeModel>> GetTreeChildren(string domainName,Guid? parentId)
    {

        var parentIds = from c in Queryable.Where(x => x.ParentId != null)
                        group c by c.ParentId into g
                        select new
                        {
                            ParentId = g.Key
                        };
        //方案一
        //var result = from c in Queryable.Where(x => x.ParentId == id)
        //             join c1 in parentIds on c.Id equals c1.ParentId
        //             into g
        //             from c1 in g.DefaultIfEmpty()
        //             select new Tuple<Guid, string,Guid?, bool>
        //                     ( c.Id,c.Name,c.ParentId,!c1.ParentId.HasValue);

        
        Expression<Func<Column, bool>> expression = x => x.ParentId == parentId;
        if (!string.IsNullOrEmpty(domainName))
        {
            expression = expression.And(x => x.DomainName==domainName);
        }
      
        var result = from c in Queryable
                     .Where(expression).OrderBy(x=>x.Sort).ThenBy(x=>x.CreateTime)
                     from c1 in parentIds
                     .Where(c1 => c.Id == c1.ParentId).DefaultIfEmpty()
                     select new ColumnTreeModel
                     {
                         Id = c.Id,
                         Name = c.Name,
                         EnglishName = c.EnglishName,
                         ParentId = c.ParentId,
                         Icon = c.Icon,
                         ActiveIcon = c.ActiveIcon,
                         IsHomePage = c.IsHomePage,
                         IsLeaf = !c1.ParentId.HasValue//是否存在有哪个栏目的ParentId等于当前栏目id
                     };

        return await result.ToListAsync();
    }

    /// <summary>
    /// 获取当前域名和英文名栏目下属子栏目的信息
    /// </summary>
    /// <param name="domainName"></param>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public Task<List<ColumnTreeModel>> GetChildrenColumnByDomainNameAndEnglishName(string domainName, string englishName)
    {
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().ToGuid();
        Expression<Func<Column, bool>> expression = c => c.TenantId==tenantId&& c.DomainName == domainName;

        expression = expression.And(c => c.EnglishName == englishName);
       
        var columns = Queryable
                            .Where(expression)
                            .Select(x => new
                            {
                                x.Id
                            });
        var result = from t in Queryable.OrderBy(x => x.Sort).ThenBy(x => x.CreateTime)
                     from c in columns.Where(c1 => t.ParentId == c1.Id)
                     select new ColumnTreeModel
                     {
                         Id = t.Id,
                         Name = t.Name,
                         EnglishName = t.EnglishName,
                         Icon = t.Icon,
                         ActiveIcon = t.ActiveIcon,
                     };


        return result.ToListAsync();
    }

    public override async Task<List<Column>> GetByQueryAsync(QueryBase<Column> query)
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
                               .OrderBy(x => x.Sort).ThenBy(x=>x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }
}
