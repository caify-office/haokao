using HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;

public interface ITemplateStyleWebService : IAppWebApiService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<TemplateStyleQueryViewModel> Get(TemplateStyleQueryViewModel queryViewModel);
}