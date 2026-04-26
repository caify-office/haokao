using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Management;

public interface IColumnService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseColumnViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<ColumnQueryViewModel> Get(ColumnQueryViewModel queryViewModel);

    /// <summary>
    /// 创建栏目
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<Guid> Create( CreateColumnViewModel model);

    /// <summary>
    /// 根据主键删除指定栏目
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定栏目
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateColumnViewModel model);
    /// <summary>
    /// 根据域名获取指定栏目的直接子栏目
    /// </summary>
    /// <param name="model">域名模型</param>
    Task<List<ColumnTreeModel>> Tree(QueryColumnByDomainNameAndParentIdViewModel model);
}