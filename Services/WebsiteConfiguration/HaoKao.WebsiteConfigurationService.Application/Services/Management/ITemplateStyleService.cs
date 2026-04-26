using HaoKao.WebsiteConfigurationService.Application.ViewModels.TemplateStyle;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;

public interface ITemplateStyleService : IAppWebApiService, IManager
{
    // /// <summary>
    // /// 根据主键获取指定
    // /// </summary>
    // /// <param name="id">主键</param>
    //Task<BrowseTemplateStyleViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<TemplateStyleQueryViewModel> Get(TemplateStyleQueryViewModel queryViewModel);

    /// <summary>
    /// 创建模板
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateTemplateStyleViewModel model);

    /// <summary>
    /// 根据主键删除指定模板
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    // /// <summary>
    // /// 根据主键更新指定模板
    // /// </summary>
    // /// <param name="id">主键</param>
    // /// <param name="model">新增模型</param>
    //Task Update(Guid id, UpdateTemplateStyleViewModel model);
}