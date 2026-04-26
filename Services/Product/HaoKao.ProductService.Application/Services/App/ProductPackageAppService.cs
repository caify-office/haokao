using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.Services.Web;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.App;

/// <summary>
/// 产品包接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductPackageAppService(
    IProductService productservice,
    IStudentPermissionRepository studentPermissionRepository,
    IProductWebService productWebService,
    IStaticCacheManager cacheManager,
    IProductPackageWebService _productPackageWebService)
    : IProductPackageAppService
{
    #region 初始参数

    private readonly IProductWebService _productWebService = productWebService ?? throw new ArgumentNullException(nameof(productWebService));
    private readonly IProductPackageWebService _productPackageWebService = _productPackageWebService ?? throw new ArgumentNullException(nameof(_productPackageWebService));
    private readonly IProductService _productservice = productservice ?? throw new ArgumentNullException(nameof(productservice));
    private readonly IStudentPermissionRepository _studentPermissionRepository = studentPermissionRepository ?? throw new ArgumentNullException(nameof(studentPermissionRepository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    #endregion

    #region 构造函数

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseProductPackageViewModel> Get(Guid id)
    {
        var productPackge = await _productPackageWebService.Get(id);
        var myProduct = await GetMyProduct(ProductType.QuestionBlank);
        var myProductId = myProduct?.Select(x => x.Id)?.ToList() ?? [];
        productPackge.IsOpen = productPackge.ProductList.Exists(x => myProductId.Contains(x));
        if (productPackge.IsOpen)
        {
            productPackge.OpenTime = await GetOpenTime(productPackge.ProductList, myProductId.ToDictionary(x => x, x => x));
        }
        return productPackge;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryProductPackageViewModel> Get([FromQuery] QueryProductPackageViewModel queryViewModel)
    {
        var result = await _productPackageWebService.Get(queryViewModel);
        var userProducts = await GetMyProduct(ProductType.QuestionBlank);
        var myProductIdDic = userProducts.ToDictionary(x => x.Id, x => x.Id);
        var userProductIds = userProducts?.Select(p => p.Id)?.ToList() ?? [];
        result.Result.ForEach(p => { p.IsOpen = userProductIds.Any(x => p.ProductList.Contains(x)); });

        result.Result.ForEach(productPackage =>
        {
            if (productPackage.IsOpen)
            {
                productPackage.OpenTime = GetOpenTime(productPackage.ProductList, myProductIdDic).Result;
            }
        });
        return result;
    }

    /// <summary>
    /// 通过产品包id数组获取产品包列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<List<BrowseProductPackageViewModel>> GetProductPackageByIds([FromBody] Guid[] ids)
    {

        return _productPackageWebService.GetProductPackageByIds(ids);
    }

    /// <summary>
    ///  通过产品包id拿这些产品包下面所有产品的权限id
    /// </summary>
    /// <param name="ids">产品包ids</param>
    [HttpPost]
    [AllowAnonymous]
    public Task<IEnumerable<Guid>> GetProductPermissionId([FromBody] Guid[] ids)
    {
        return _productPackageWebService.GetProductPermissionId(ids);
    }


    private async Task<DateTime> GetOpenTime(List<Guid> productIds, Dictionary<Guid, Guid> myProductIdDic)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        //读取符合情况的产品id集合
        var productids = productIds
                         .Where(myProductIdDic.ContainsKey)
                         .ToList();
        var cacheKey = GirvsEntityCacheDefaults<StudentPermission>.ByIdsCacheKey.Create($"{userId}_{string.Join("_", productids.OrderBy(x => x))}_MinCreateTime".ToMd5());
        var studentpermisssion = await _cacheManager.GetAsync(cacheKey, async () => await _studentPermissionRepository.GetWhereInclude(userId, productids));
        if (studentpermisssion != null) return studentpermisssion.CreateTime;

        return DateTime.Now;
    }

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        var products = await _productservice.GetByIds(ids);
        var userProduct = await GetMyProduct(ProductType.QuestionBlank);
        if (userProduct.Any() == false) return products;
        products.ForEach(x => { x.IsBuy = userProduct.Any(p => p.Id == x.Id); });
        return products;
    }

    public Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType)
    {
        return _productWebService.GetMyProduct(productType);
    }

    /// <summary>
    /// 验证当前用户在当前科目下是否具有有产品的权限
    /// </summary>
    /// <param name="Subjectid"></param>
    /// <param name="Categoryid"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<bool> HasAuthority(Guid Subjectid, Guid Categoryid)
    {
        //写sql查询出科目下面的产品ids
        var productIds = await _studentPermissionRepository.GetProductIdsBy(Subjectid, Categoryid);
        //验证取到的产品ids是否在myproduct中是否存在
        var userProducts = await GetMyProduct(ProductType.QuestionBlank);
        return productIds.Any(x => userProducts.Select(x => x.Id).Contains(x));
    }

    #endregion
}