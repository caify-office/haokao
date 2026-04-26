namespace HaoKao.DataDictionaryService.Application.Services.App;

public interface IDictionaryAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取自身和所有子字典
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <returns></returns>
    Task<List<DictionariesTreeViewModel>> Get(Guid id);

    /// <summary>
    /// 获取数据字典树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="name">根据名称搜索</param>
    /// <returns></returns>
    Task<List<DictionariesTreeViewModel>> GetTree(Guid? id, string name);
}