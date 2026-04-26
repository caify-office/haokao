using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;

namespace HaoKao.ContinuationService.Application.ProductExtensionRequestModule.Interfaces;

/// <summary>
/// 课程续读申请接口服务
/// </summary>
public interface IProductExtensionRequestService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定申请
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<BrowseProductExtensionRequestViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取申请列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    /// <returns></returns>
    Task<QueryProductExtensionRequestViewModel> Get(QueryProductExtensionRequestViewModel viewModel);

    /// <summary>
    /// 创建课程续读申请
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    Task Create(CreateProductExtensionRequestViewModel model);

    /// <summary>
    /// 更新课程续读申请状态 (审核)
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    Task Update(UpdateProductExtensionRequestStateViewModel model);
}