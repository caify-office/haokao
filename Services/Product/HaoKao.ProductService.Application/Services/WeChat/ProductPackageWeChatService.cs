using HaoKao.ProductService.Application.Services.App;
using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.WeChat;

/// <summary>
/// 产品包WeChat端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductPackageWeChatService(IProductPackageWebService productPackageWebService, IProductPackageAppService productPackageAppService) : IProductPackageWeChatService
{
    private readonly IProductPackageWebService _productPackageWebService = productPackageWebService ?? throw new ArgumentNullException(nameof(productPackageWebService));
    private readonly IProductPackageAppService _productPackageAppService = productPackageAppService ?? throw new ArgumentNullException(nameof(productPackageAppService));

    /// <summary>
    /// 通过产品包id数组获取每个产品包的复合数据
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost]
    [AllowAnonymous]
    public Task<List<ProductPackageCompositeAttribute>> GetCompositeAttributeByIds([FromBody] List<Guid> ids)
    {
        return _productPackageWebService.GetCompositeAttributeByIds(ids);
    }

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public Task<BrowseProductPackageViewModel> Get(Guid id)
    {
        return _productPackageAppService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet, AllowAnonymous]
    public Task<QueryProductPackageViewModel> Get([FromQuery] QueryProductPackageViewModel queryViewModel)
    {
        return _productPackageAppService.Get(queryViewModel);
    }

    /// <summary>
    /// 通过产品包id数组获取产品包列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost, AllowAnonymous]
    public Task<List<BrowseProductPackageViewModel>> GetProductPackageByIds([FromBody] Guid[] ids)
    {

        return _productPackageAppService.GetProductPackageByIds(ids);
    }

    /// <summary>
    ///  通过产品包id拿这些产品包下面所有产品的权限id
    /// </summary>
    /// <param name="ids">产品包ids</param>
    [HttpPost]
    [AllowAnonymous]
    public Task<IEnumerable<Guid>> GetProductPermissionId([FromBody] Guid[] ids)
    {
        return _productPackageAppService.GetProductPermissionId(ids);
    }
}