using System.Collections.Generic;
using HaoKao.WebsiteConfigurationService.Application.ViewModels.WebsiteTemplate;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;

public interface IWebsiteTemplateService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseWebsiteTemplateViewModel> Get(Guid id);

    /// <summary>
    /// 根据域名获取符合条件得模板
    /// </summary>
    /// <param name="model">域名</param>
    Task<List<BrowseWebsiteTemplateViewModel>> GetByDomainName(QueryByDomainNameViewModel model);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<WebsiteTemplateQueryViewModel> Get(WebsiteTemplateQueryViewModel queryViewModel);

    /// <summary>
    /// 创建模板
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<Guid> Create(CreateWebsiteTemplateViewModel model);

    /// <summary>
    /// 根据主键删除指定模板
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定模板
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateWebsiteTemplateViewModel model);

    /// <summary>
    /// 设置模板内容
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="content">模板内容</param>
    Task Content(Guid id, SetWebsiteTemplateContentViewModel content);

    /// <summary>
    /// 启用
    /// </summary>
    Task Enable(Guid id);

    /// <summary>
    /// 禁用
    /// </summary>
    Task DisEnable(Guid id);
    /// <summary>
    /// 设置为默认
    /// </summary>
    Task Default(Guid id);


    /// <summary>
    /// 取消默认
    /// </summary>
    Task CancelDefault(Guid id);
}