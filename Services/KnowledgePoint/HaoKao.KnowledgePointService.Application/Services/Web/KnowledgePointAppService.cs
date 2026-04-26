using HaoKao.Common;
using HaoKao.KnowledgePointService.Application.Services.App;
using HaoKao.KnowledgePointService.Application.Services.Management;
using HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

namespace HaoKao.KnowledgePointService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class KnowledgePointWebService(IKnowledgePointService service) : IKnowledgePointAppService
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
}