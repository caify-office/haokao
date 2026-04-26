using HaoKao.ChapterNodeService.Application.KnowledgePointModule.Services.Management;
using HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class KnowledgePointWebService(IKnowledgePointService service) : IKnowledgePointWebService
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<KnowledgePointBrowseViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public  Task<KnowledgePointQueryViewModel> Get([FromQuery] KnowledgePointQueryViewModel queryViewModel)
    {
        return service.Get(queryViewModel);
    }

}