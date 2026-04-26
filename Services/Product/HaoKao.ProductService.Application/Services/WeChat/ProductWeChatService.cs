using HaoKao.ProductService.Application.Services.App;
using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.WeChat;

/// <summary>
/// 产品WeChat端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductWeChatService(
    IProductWebService productWebService,
    IProductPackageAppService productPackageAppService) : IProductWeChatService
{
    private readonly IProductWebService _productWebService = productWebService ?? throw new ArgumentNullException(nameof(productWebService));
    private readonly IProductPackageAppService _productPackageAppService = productPackageAppService ?? throw new ArgumentNullException(nameof(productPackageAppService));

    /// <summary>
    /// 根据Id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public Task<BrowseProductViewModel> Get(Guid id)
    {
        return _productWebService.Get(id);
    }

    // /// <summary>
    // /// 通过产品id数组获取产品列表
    // /// </summary>
    // /// <param name="ids"></param>
    // /// <returns></returns>
    //[HttpPost]
    //[AllowAnonymous]
    //public  Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    //{
    //    return _productService.GetByIds(ids);
    //}

    /// <summary>
    /// 通过产品包id和产品id数组获取产品价格列表
    /// </summary>
    /// <param name="productPackageId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost("{productPackageId:guid}")]
    [AllowAnonymous]
    public Task<List<BrowseProductPriceViewModel>> GetPriceByIds(Guid productPackageId, [FromBody] Guid[] ids)
    {
        return _productWebService.GetPriceByIds(productPackageId, ids);
    }

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        return _productPackageAppService.GetByIds(ids);
    }

    /// <summary>
    /// 通过产品id数组获取其中免费产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public Task<List<BrowseProductViewModel>> GetFreeProductsByIds([FromBody] Guid[] ids)
    {
        return _productWebService.GetFreeProductsByIds(ids);
    }

    /// <summary>
    /// 获取我的产品(包含过期时间和最早购买时间)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType)
    {
        return _productWebService.GetMyProduct(productType);
    }

    /// <summary>
    /// 获取我的过期产品，未过期产品，所有产品(包含过期时间和最早购买时间)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<MyProductViewModel>> GetMyAllProduct(ProductType? productType,  PermissionExpiryType permissionExpiryType)
    {

        return _productWebService.GetMyAllProduct(productType,  permissionExpiryType);
    }

    /// <summary>
    /// 获取我的产品权限
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <param name="subjectId">科目id（不传取所有科目）</param>
    /// <returns></returns>
    [HttpGet]
    public  Task<List<BrowseProductPermissionViewModel>> GetMyProductPermisson(ProductType? productType, Guid? subjectId)
    {
        return _productWebService.GetMyProductPermisson(productType,  subjectId);
    }

    /// <summary>
    /// 获取我的产品所属科目
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public  Task<List<MyProductSubjectViewModel>> GetMyProductSubject(ProductType? productType)
    {
        return _productWebService.GetMyProductSubject(productType);
    }

    /// <summary>
    /// 通过直播id数组获取产品列表
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public  Task<List<BrowseProductLiveViewModel>> GetByLiveIds([FromBody] List<QueryProductByLiveIdViewModel> models)
    {
        return _productWebService.GetByLiveIds(models);
    }
}