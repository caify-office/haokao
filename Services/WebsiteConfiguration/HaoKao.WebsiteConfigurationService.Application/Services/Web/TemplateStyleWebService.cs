using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.WebsiteConfigurationService.Application.Services.Management;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;


/// <summary>
/// 模板样式接口服务-Web端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class TemplateStyleWebService(ITemplateStyleService service) : ITemplateStyleWebService
{

    private readonly ITemplateStyleService _service = service ?? throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<TemplateStyleQueryViewModel> Get([FromQuery] TemplateStyleQueryViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }
}