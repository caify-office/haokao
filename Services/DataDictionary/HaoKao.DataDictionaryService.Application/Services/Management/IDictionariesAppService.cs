namespace HaoKao.DataDictionaryService.Application.Services.Management;

public interface IDictionariesAppService : IAppWebApiService, IManager
{
    Task<DictionariesAddViewModel> CreateAsync(DictionariesAddViewModel model);

    Task<DictionariesEditViewModel> UpdateAsync(Guid id, DictionariesEditViewModel model);

    Task<DictionariesDetailViewModel> GetAsync(Guid id);

    Task DeleteAsync(Guid[] id);

    Task<List<DictionariesTreeViewModel>> GetTreeByQueryAsync(Guid? id, string name);

    Task<List<DictionariesTreeViewModel>> GetAllAsync(Guid id);

    Task<DictionariesQueryViewModel> GetByQueryAsync(DictionariesQueryViewModel queryModel);

    Task<List<DictionariesCategoryListViewModel>> GetCategorysByQueryAsync(string query);

    Task<string> SupplementaryChildrenDictionaryDataAsync(Guid id, string name);

    Task<string> SupplementaryDictionaryDataAsync(string modelJsonStr);

    Task<List<NewTemplateViewModel>> NewSupplementary(List<NewTemplateViewModel> models);

    Task<List<UserInformationListViewModel>> SupplementaryDictionaryName(string modelJsonStr);

    Task<Dictionary<string, string>> GetByIdsAsync(List<string> ids);

    /// <summary>
    /// 通过获取字典ID数组获取数据字典和直接子字典列表
    /// </summary>
    /// <param name="ids">展开的节点</param>
    /// <returns></returns>
    Task<List<DictionariesTreeHaveChildrenViewModel>> GetTreeByIdsAsync(Guid[] ids);
}