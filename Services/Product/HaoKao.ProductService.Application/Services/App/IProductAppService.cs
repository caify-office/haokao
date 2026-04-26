using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.App;

public interface IProductAppService : IAppWebApiService
{
    /// <summary>
    /// 根据Id 获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<BrowseProductViewModel> Get(Guid id);

    /// <summary>
    ///  获取我的产品
    /// </summary>
    /// <returns></returns>
    Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType);

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
    /// 通过产品包id和产品id数组获取产品价格列表
    /// </summary>
    /// <param name="productPackageId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<BrowseProductPriceViewModel>> GetPriceByIds(Guid productPackageId, Guid[] ids);
}