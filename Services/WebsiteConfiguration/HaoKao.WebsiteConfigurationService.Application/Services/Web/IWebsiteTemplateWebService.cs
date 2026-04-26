using System.Collections.Generic;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;

public interface IWebsiteTemplateWebService : IAppWebApiService
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseWebsiteTemplateViewModel> Get(Guid id);

    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="model"></param>
    Task<List<BrowseWebsiteTemplateViewModel>> GetByDomainName(QueryByDomainNameViewModel model);

    /// <summary>
    /// 获取当前域名，当且英文名下的模板
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<WebsiteTemplateQueryWebListViewModel>> GetByDominNameAneEnglishName( GetWebsiteTemplateContentViewModel model);
}