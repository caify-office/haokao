using HaoKao.ContinuationService.Application.ProductExtensionModule.ViewModels;
using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;

namespace HaoKao.ContinuationService.Application.ProductExtensionModule.Interfaces;

public interface IProductExtensionWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定申请详情
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseProductExtensionRequestWebViewModel> Get(Guid id);

    /// <summary>
    /// 获取可申请列表
    /// </summary>
    /// <param name="productIds">产品Id集合</param>
    /// <returns></returns>
    Task<List<ProductExtensionServiceRequestViewModel>> GetRequestList(List<Guid> productIds);

    /// <summary>
    /// 获取申请记录列表
    /// </summary>
    /// <returns></returns>
    Task<List<QueryProductExtensionRequestListWebViewModel>> GetRequestRecord();

    /// <summary>
    /// 创建续读申请
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateProductExtensionRequestViewModel model);
}