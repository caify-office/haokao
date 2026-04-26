using HaoKao.StudyMaterialService.Application.Interfaces;
using HaoKao.StudyMaterialService.Domain.Entities;

namespace HaoKao.StudyMaterialService.Application.Services;

/// <summary>
/// 学习资料接口服务-小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class StudyMaterialWeChatService(IStudyMaterialWebService service) : IStudyMaterialWeChatService
{
    private readonly IStudyMaterialWebService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据ids获取资料列表
    /// </summary>
    /// <param name="ids">ids</param>
    /// <returns></returns>
    [HttpPost]
    public  Task<List<Material>> Get([FromBody] List<Guid> ids)
    {
       return _service.Get(ids);
    }
}