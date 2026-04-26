using System.Collections.Generic;

namespace HaoKao.WebsiteConfigurationService.Application.Services.Web;

public interface IColumnWebService : IAppWebApiService
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
    Task<ColumnQueryViewModel> Get([FromQuery] ColumnQueryViewModel queryViewModel);

    /// <summary>
    ///   /// <summary>
    /// 根据域名和英文名查询符合条件栏目下面的子栏目信息
    /// </summary>
    /// <param name="model"></param>
    /// </summary>
    Task<List<SimpleColumnQueryListViewModel>> GetChildrenColumn([FromBody] GetChildrenColumnViewModel model);
}