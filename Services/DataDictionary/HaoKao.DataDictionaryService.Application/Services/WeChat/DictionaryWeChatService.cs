using HaoKao.Common;
using HaoKao.DataDictionaryService.Application.Services.Management;

namespace HaoKao.DataDictionaryService.Application.Services.WeChat;

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DataDictionaryWeChatService(IDictionariesAppService service) : IDictionaryWeChatService
{
    private readonly IDictionariesAppService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 获取自身和所有子字典
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <returns></returns>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public Task<List<DictionariesTreeViewModel>> Get(Guid id)
    {
        return _service.GetAllAsync(id);
    }

    /// <summary>
    /// 获取数据字典树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="name">根据名称搜索</param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public Task<List<DictionariesTreeViewModel>> GetTree(Guid? id, string name)
    {
        return _service.GetTreeByQueryAsync(id, name);
    }
}