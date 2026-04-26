using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.WebsiteConfigurationService.Application.Services.Management;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;

/// <summary>
/// 模板接口服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class WebsiteTemplateWebService(
    IWebsiteTemplateRepository repository,
    IWebsiteTemplateService websiteTemplateService) : IWebsiteTemplateWebService
{
    #region 初始参数
    private readonly IWebsiteTemplateService _websiteTemplateService = websiteTemplateService ?? throw new ArgumentNullException(nameof(websiteTemplateService));
    private readonly IWebsiteTemplateRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public  Task<BrowseWebsiteTemplateViewModel> Get(Guid id)
    {
        return _websiteTemplateService.Get(id);
    }

    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    public Task<List<BrowseWebsiteTemplateViewModel>> GetByDomainName([FromBody] QueryByDomainNameViewModel model)
    {
        return _websiteTemplateService.GetByDomainName(model);
    }

    /// <summary>
    /// 获取当前域名，当且英文名下的模板
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    public async Task<List<WebsiteTemplateQueryWebListViewModel>> GetByDominNameAneEnglishName([FromBody] GetWebsiteTemplateContentViewModel model)
    {
        var query = await _repository.GetWebsiteTemplateByDominNameAneEnglishName(model.DomainName, model.EnglishName);
        var result=new List<WebsiteTemplateQueryWebListViewModel>();
        query.ForEach(x =>
        {
            result.Add(new WebsiteTemplateQueryWebListViewModel
            {
              Id=x.Item1,
              Name=x.Item2,
              WebsiteTemplateType=x.Item3,
              Desc=x.Item4,
              ColumnId=x.Item5,
              ColumnName=x.Item6,
              IsDefault=x.Item7
            });
        });
        return result;
    }
    #endregion
}