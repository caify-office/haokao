using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Infrastructure.Repositories;

public class DictionariesRepository : Repository<Dictionaries>, IDictionariesRepository
{
    public async Task<List<Dictionaries>> GetDictionariesTreeByQueryAsync(Guid? id, string name)
    {
        var expression = OtherQueryCondition;
        if ((EngineContext.Current.ClaimManager.IdentityClaim?.TenantId.ToGuid() ?? Guid.Empty) == Guid.Empty)
        {
            //情况一：未登录且不传租户id  情况二：未登录但是传了00000000-0000-0000-0000-000000000000
            expression = expression.And(x => x.TenantId == Guid.Empty);
        }
        List<Dictionaries> resultTree;
        if (!name.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Path.Contains(name));
            var trees = await Queryable.Include(x => x.Children).Where(expression).ToListAsync();
            // 取出最顶级
            var pids = trees.Where(w => w.Name.Contains(name)).Select(s => s.Id).ToArray();
            resultTree = trees.Where(f => pids.Contains(f.Id)).ToList();
        }
        else
        {
            expression = expression.And(x => x.Pid.Equals(id));
            resultTree = await Queryable.Include(x => x.Children).Where(expression).ToListAsync();
        }

        FilterChildrenData(resultTree);
        return resultTree;
    }

    public async Task<List<Dictionaries>> GetAllAsync(Guid id)
    {
        var expression = OtherQueryCondition;

        if ((EngineContext.Current.ClaimManager.IdentityClaim?.TenantId.ToGuid() ?? Guid.Empty) == Guid.Empty)
        {
            //情况一：未登录且不传租户id  情况二：未登录但是传了00000000-0000-0000-0000-000000000000
            expression = expression.And(x => x.TenantId == Guid.Empty);
        }
        var currentDictionary = await Queryable.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (currentDictionary is null)
        {
            return [];
        }
        var name = currentDictionary.Name;
        expression = expression.And(x => x.Path.Contains(name));
        var trees = await Queryable.Include(x => x.Children).Where(expression).ToListAsync();
        // 当前字典
        var resultTree = trees.Where(f => id == f.Id).ToList();
        return resultTree;
    }

    /// <summary>
    /// 过滤子类数据
    /// </summary>
    /// <param name="resultTree"></param>
    private void FilterChildrenData(List<Dictionaries> resultTree)
    {
        var childrenOtherCondition = OtherQueryCondition.Compile();
        foreach (var item in resultTree)
        {
            if (!item.Children.Any(childrenOtherCondition))
            {
                item.Children = [];
            }
            else
            {
                FilterChildrenData(item.Children);
            }
        }
    }

    /// <summary>
    /// 查询字典分类
    /// </summary>
    /// <param name="query">名称或者编码</param>
    /// <returns></returns>
    public Task<List<Dictionaries>> GetDictionariesCategoryListByQueryAsync(string query, params string[] fields)
    {
        Expression<Func<Dictionaries, bool>> expression = x => !x.Pid.HasValue;
        if (!query.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(query) || x.Code.Contains(query));
        }

        return GetWhereAsync(expression);
    }

    /// <summary>
    /// 通过id查询字典树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Dictionaries> GetByIncludeChildrenIdAsync(Guid id)
    {
        var list = Queryable.Include(x => x.Children).ToList();

        var result = list.FirstOrDefault(t => t.Id.Equals(id));

        var childrenOtherCondition = OtherQueryCondition.Compile();

        if (!result.Children.Any(childrenOtherCondition))
        {
            result.Children = [];
        }
        else
        {
            FilterChildrenData(result.Children);
        }
        return Task.FromResult(result);
    }

    /// <summary>
    /// 通过id查询字典树(子字典带过滤租户逻辑)
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public Task<List<Dictionaries>> GetByIncludeChildrenIdsAsync(Guid[] ids)
    {
        //此种方式能获取所有子字典
        //var list = Queryable.Include(x => x.Children).ToList();
        //var result = list.Where(t => ids.Contains(t.Id)).ToList();

        //此种方式获取直接子字典
        var result = Queryable.Include(x => x.Children).Where(t => ids.Contains(t.Id)).ToList();
        //调整排序
        var resultDic = result.ToDictionary(x => x.Id, x => x);
        var resultOrder = new List<Dictionaries>();
        ids.ToList().ForEach(x =>
        {
            if (resultDic.TryGetValue(x, out var dictionaries))
            {
                resultOrder.Add(dictionaries);
            }
        });

        var childrenOtherCondition = OtherQueryCondition.Compile();
        resultOrder.ForEach(x =>
        {
            if (!x.Children.Any(childrenOtherCondition))
            {
                x.Children = [];
            }
            else
            {
                FilterChildrenData(x.Children);
            }
        });
        return Task.FromResult(resultOrder);
    }

    /// <summary>
    /// 通过id查询字典树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Dictionaries> GetByIncludeOneLevelChildrenIdAsync(Guid id)
    {
        return Queryable.Include(x => x.Children).FirstOrDefaultAsync(t => t.Id.Equals(id));
    }

    public Task<List<Dictionaries>> GetByIncludeChildrenIdsAsync(Guid?[] ids)
    {
        return Queryable.Where(w => ids.Contains(w.Id)).Include(x => x.Children).ThenInclude(x => x.Children).ToListAsync();
    }

    public Task<List<Dictionaries>> GetByIncludeChildrenAsync()
    {
        var result = Queryable.Include(x => x.Children).ToListAsync().Result;
        OtherQueryCondition.Compile();
        FilterChildrenData(result);
        return Task.FromResult(result);
    }

    public Task<List<Dictionaries>> GetNoTenantIdWhereAsync(Expression<Func<Dictionaries, bool>> predicate, params string[] fields)
    {
        return fields.Any()
            ? ExcludeOtherQueryCondition().Where(predicate).SelectProperties(fields).ToListAsync()
            : ExcludeOtherQueryCondition().Where(predicate).ToListAsync();
    }

    public override async Task<List<Dictionaries>> GetByQueryAsync(QueryBase<Dictionaries> query)
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
                               .OrderBy(x => x.Sort)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public Task<List<Dictionaries>> GetWhereIncludeChildrenAsync(Expression<Func<Dictionaries, bool>> predicate, params string[] fields)
    {
        return fields.Any()
            ? Queryable.Include(x => x.Children).Where(predicate).SelectProperties(fields).ToListAsync()
            : Queryable.Include(x => x.Children).Where(predicate).ToListAsync();
    }
}