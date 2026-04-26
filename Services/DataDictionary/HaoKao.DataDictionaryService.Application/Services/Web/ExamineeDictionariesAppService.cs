using HaoKao.Common;
using HaoKao.DataDictionaryService.Application.Services.Management;

namespace HaoKao.DataDictionaryService.Application.Services.Web;

/// <summary>
/// 字典管理 - 考生端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class ExamineeDictionariesAppService(IDictionariesAppService service) : IExamineeDictionariesAppService
{
    private readonly IDictionariesAppService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 考生端 根据Id获取数据字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public Task<DictionariesDetailViewModel> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    /// 考生端 获取数据字典树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="name">根据名称搜索</param>
    /// <returns></returns>
    [HttpGet("tree")]
    public Task<List<DictionariesTreeViewModel>> GetTreeByQueryAsync(Guid? id, string name)
    {
        return _service.GetTreeByQueryAsync(id, name);
    }

    /// <summary>
    /// 获取自身和所有子字典
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <returns></returns>
    [HttpGet]
    public Task<List<DictionariesTreeViewModel>> GetAllAsync(Guid id)
    {
        return _service.GetAllAsync(id);
    }

    /// <summary>
    /// 考试端 根据id和名称补充树形直接子集字典数据
    /// </summary>
    /// <param name="id">字典id</param>
    /// <param name="name">字典名称</param>
    /// <returns></returns>
    [HttpGet("{id:guid}/{name}/SupplementaryChildren")]
    public Task<string> SupplementaryChildrenDictionaryDataAsync(Guid id, string name)
    {
        return _service.SupplementaryChildrenDictionaryDataAsync(id, name);
    }

    /// <summary>
    /// 考生端 通过用户信息项保存的值中的字典id补充字典的名称
    /// </summary>
    /// <param name="modelJsonStr"></param>
    /// <returns></returns>
    [HttpPost("SupplementaryDictionaryName")]
    public Task<List<UserInformationListViewModel>> SupplementaryDictionaryName([FromForm] string modelJsonStr)
    {
        return _service.SupplementaryDictionaryName(modelJsonStr);
    }

    /// <summary>
    /// 考生端 补充字典数据
    /// </summary>
    /// <param name="modelJsonStr">json字符串</param>
    /// <returns></returns>
    [HttpPost("Supplementary")]
    public Task<string> SupplementaryDictionaryDataAsync([FromForm] string modelJsonStr)
    {
        return _service.SupplementaryDictionaryDataAsync(modelJsonStr);
    }

    /// <summary>
    /// 考生端 补充字典数据
    /// </summary>
    /// <param name="models">json字符串</param>
    /// <returns></returns>
    [HttpPost("NewSupplementary")]
    public async Task<List<NewTemplateViewModel>> NewSupplementary([FromBody] List<NewTemplateViewModel> models)
    {
        return await _service.NewSupplementary(models);
    }

    /// <summary>
    /// 考生端 根据Id获取数据字典
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Dictionary<string, string>> GetByIdsAsync([FromForm] List<string> ids)
    {
        return _service.GetByIdsAsync(ids);
    }

    /// <summary>
    /// 通过获取字典ID数组获取数据字典和直接子字典列表
    /// </summary>
    /// <param name="ids">展开的节点</param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<DictionariesTreeHaveChildrenViewModel>> GetTreeByIdsAsync([FromBody] Guid[] ids)
    {
        return _service.GetTreeByIdsAsync(ids);
    }
}