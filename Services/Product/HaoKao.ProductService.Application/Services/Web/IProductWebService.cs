using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;
using System.Threading.Tasks;

namespace HaoKao.ProductService.Application.Services.Web;

public interface IProductWebService : IAppWebApiService, IManager
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
    /// 获取我的过期产品，未过期产品，所有产品
    /// </summary>
    /// <returns></returns>
    Task<List<MyProductViewModel>> GetMyAllProduct(ProductType? productType,  PermissionExpiryType permissionExpiryType);

    /// <summary>
    /// 获取我的产品权限
    /// </summary>
    /// <returns></returns>
     Task<List<BrowseProductPermissionViewModel>> GetMyProductPermisson(ProductType? productType, Guid? subjectId);

    /// <summary>
    /// 获取我的产品所属科目
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    Task<List<MyProductSubjectViewModel>> GetMyProductSubject(ProductType? productType);

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

    /// <summary>
    /// 通过直播id数组获取产品列表
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    Task<List<BrowseProductLiveViewModel>> GetByLiveIds(List<QueryProductByLiveIdViewModel> models);
}