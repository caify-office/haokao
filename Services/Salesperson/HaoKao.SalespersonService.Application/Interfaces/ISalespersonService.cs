using HaoKao.SalespersonService.Application.ViewModels;

namespace HaoKao.SalespersonService.Application.Interfaces;

/// <summary>
/// 销售人员管理端服务接口
/// </summary>
public interface ISalespersonService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<BrowseSalespersonViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    /// <returns></returns>
    Task<QuerySalespersonViewModel> Get(QuerySalespersonViewModel model);

    /// <summary>
    /// 创建销售人员
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    Task Create(CreateSalespersonViewModel model);

    /// <summary>
    /// 根据主键更新销售人员
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    Task Update(UpdateSalespersonViewModel model);

    /// <summary>
    /// 删除销售人员
    /// </summary>
    /// <param name="ids">主键</param>
    /// <returns></returns>
    Task Delete(IReadOnlyList<Guid> ids);
}