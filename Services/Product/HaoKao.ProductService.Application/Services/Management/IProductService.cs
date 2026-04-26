using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.Management;

public interface IProductService : IAppWebApiService, IManager
{
    Task<BrowseProductViewModel> Get(Guid id);

    Task<QueryProductViewModel> Get(QueryProductViewModel queryModel);

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<List<BrowseProductViewModel>> GetByIds(Guid[] ids);


    /// <summary>
    /// 通过产品id数组获取其中免费产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<BrowseProductViewModel>> GetFreeProductsByIds(Guid[] ids);

    /// <summary>
    /// 通过直播id数组获取产品列表
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    Task<List<BrowseProductLiveViewModel>> GetByLiveIds(List<QueryProductByLiveIdViewModel> models);
    /// <summary>
    /// 获取我的产品
    /// </summary>
    /// <returns></returns>
    Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType, Guid? userId);
    /// <summary>
    /// 获取我的过期产品，未过期产品，所有产品
    /// </summary>
    /// <returns></returns>
    Task<List<MyProductViewModel>> GetMyAllProduct(ProductType? productType, Guid? userId, PermissionExpiryType permissionExpiryType);
    Task Create(CreateProductViewModel model);

    Task Update(UpdateProductViewModel model);

    /// <summary>
    /// 批量开启答疑授权
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task OpenAnswering(Guid[] id);

    /// <summary>
    /// 批量关闭答疑授权
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task CloseAnswering(Guid[] id);

    Task Delete(DeleteProductViewModel model);

    Task Enable(ChangeProductStateViewModel model);

    Task Disable(ChangeProductStateViewModel model);



    /// <summary>
    /// 查询复合条件的产品id
    /// </summary>
    /// <returns></returns>
    Task<List<Guid>> GetProductIds(Guid? subjectId, Guid? permissionId);
 
}