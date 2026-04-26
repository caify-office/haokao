using HaoKao.DataDictionaryService.Domain.Entities;

namespace HaoKao.DataDictionaryService.Domain.Repositories;

public interface IDictionariesRepository : IRepository<Dictionaries>
{
    /// <summary>
    /// 获取直接子字典
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<List<Dictionaries>> GetDictionariesTreeByQueryAsync(Guid? id, string name);

    /// <summary>
    /// 获取自身和所有子字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<Dictionaries>> GetAllAsync(Guid id);

    /// <summary>
    /// 查询字典分类
    /// </summary>
    /// <param name="query">名称或者编码</param>
    /// <param name="fields"></param>
    /// <returns></returns>
    Task<List<Dictionaries>> GetDictionariesCategoryListByQueryAsync(string query, params string[] fields);

    /// <summary>
    /// 通过id查询字典树
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Dictionaries> GetByIncludeChildrenIdAsync(Guid id);

    /// <summary>
    /// 通过id查询字典树(子字典带过滤租户逻辑)
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<Dictionaries>> GetByIncludeChildrenIdsAsync(Guid[] ids);

    Task<Dictionaries> GetByIncludeOneLevelChildrenIdAsync(Guid id);

    Task<List<Dictionaries>> GetByIncludeChildrenIdsAsync(Guid?[] ids);

    Task<List<Dictionaries>> GetByIncludeChildrenAsync();

    /// <summary>
    /// 忽略租户id的过滤，在整个数据库范围内找到符合条件的数据
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    Task<List<Dictionaries>> GetNoTenantIdWhereAsync(Expression<Func<Dictionaries, bool>> predicate, params string[] fields);

    Task<List<Dictionaries>> GetWhereIncludeChildrenAsync(Expression<Func<Dictionaries, bool>> predicate, params string[] fields);
}