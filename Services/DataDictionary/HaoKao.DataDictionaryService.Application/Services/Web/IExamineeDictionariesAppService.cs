namespace HaoKao.DataDictionaryService.Application.Services.Web;

public interface IExamineeDictionariesAppService : IAppWebApiService
{
    Task<DictionariesDetailViewModel> GetAsync(Guid id);
    Task<List<DictionariesTreeViewModel>> GetTreeByQueryAsync(Guid? id, string name);

    Task<List<NewTemplateViewModel>> NewSupplementary(List<NewTemplateViewModel> models);

    Task<string> SupplementaryDictionaryDataAsync(string modelJsonStr);

    Task<Dictionary<string, string>> GetByIdsAsync(List<string> ids);

    /// <summary>
    /// 通过获取字典ID数组获取数据字典和直接子字典列表
    /// </summary>
    /// <param name="ids">展开的节点</param>
    /// <returns></returns>
    Task<List<DictionariesTreeHaveChildrenViewModel>> GetTreeByIdsAsync([FromBody] Guid[] ids);
}