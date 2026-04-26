using HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.ViewModels;

namespace HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.Interfaces;

/// <summary>
/// 课程续读策略接口服务
/// </summary>
public interface IProductExtensionPolicyService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定策略
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<BrowseProductExtensionPolicyViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取策略列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    /// <returns></returns>
    Task<QueryProductExtensionPolicyViewModel> Get(QueryProductExtensionPolicyViewModel viewModel);

    /// <summary>
    /// 创建课程续读策略
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    Task Create(CreateProductExtensionPolicyViewModel model);

    /// <summary>
    /// 更新课程续读策略
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    Task Update(UpdateProductExtensionPolicyViewModel model);

    /// <summary>
    /// 删除课程续读策略
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task Delete(Guid id);

    /// <summary>
    /// 启用/禁用课程续读策略
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="isEnable">是否启用</param>
    /// <returns></returns>
    Task Enable(Guid id, bool isEnable);
}