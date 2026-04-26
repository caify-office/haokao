using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;

namespace HaoKao.ProductService.Application.Services.App;

/// <summary>
/// 产品App端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductAppService(
    IProductService productService,
    IProductWebService productWebService) : IProductAppService
{
    private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    private readonly IProductWebService _productWebService = productWebService ?? throw new ArgumentNullException(nameof(productWebService));

    /// <summary>
    /// 根据Id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public Task<BrowseProductViewModel> Get(Guid id)
    {
        return _productService.Get(id);
    }

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public  Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        return _productService.GetByIds(ids);
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
        return _productService.GetFreeProductsByIds(ids);
    }
    /// <summary>
    /// 通过产品包id和产品id数组获取产品价格列表
    /// </summary>
    /// <param name="productPackageId"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost("{productPackageId}")]
    [AllowAnonymous]
    public  Task<List<BrowseProductPriceViewModel>> GetPriceByIds(Guid productPackageId, [FromBody] Guid[] ids)
    {
        
        return _productWebService.GetPriceByIds(productPackageId, ids);
    }
    /// <summary>
    /// 获取我的产品
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public  Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType)
    {
        return _productWebService.GetMyProduct(productType);
    }
}