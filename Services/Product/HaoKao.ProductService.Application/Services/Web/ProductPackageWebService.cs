using HaoKao.Common.Enums;
using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Extensions;
using HaoKao.ProductService.Domain.Repositories;
using System.Linq.Expressions;

namespace HaoKao.ProductService.Application.Services.Web;

/// <summary>
/// 产品包web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductPackageWebService(
    IProductService productService,
    IProductPackageService productPackageService,
    IProductPackageRepository productPackageRepository,
    IStaticCacheManager cacheManager,
    IProductRepository productRepository,
    IStudentPermissionRepository studentPermissionRepository)
    : IProductPackageWebService
{
    private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    private readonly IProductPackageService _productPackageService = productPackageService ?? throw new ArgumentNullException(nameof(productPackageService));
    private readonly IProductPackageRepository _productPackageRepository = productPackageRepository ?? throw new ArgumentNullException(nameof(productPackageRepository));
    private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IStudentPermissionRepository _studentPermissionRepository = studentPermissionRepository ?? throw new ArgumentNullException(nameof(studentPermissionRepository));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    /// <summary>
    /// 根据主键获取指定产品包
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<BrowseProductPackageViewModel> Get(Guid id)
    {
        var productPackage = await _productPackageService.Get(id);
        var productList = await _productService.GetByIds(productPackage.ProductList.ToArray());
        var dict = productList.ToDictionary(x => x.Id, x => x);
        //过滤过期产品
        var filter = from pid in productPackage.ProductList
                     where dict.TryGetValue(pid, out var p)
                        && p.IsShelves
                        && (p.ExpiryTimeTypeEnum==ExpiryTimeTypeEnum.Day||p.ExpiryTime >= DateTime.Now)
                     select pid;
        productPackage.ProductList = filter.ToList();
        return productPackage;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页（产品包列表和产品包对比使用）
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [AllowAnonymous]
    public async Task<QueryProductPackageViewModel> Get([FromQuery] QueryProductPackageViewModel queryViewModel)
    {
        queryViewModel.Enable = true;
        queryViewModel.Shelves = true;
        queryViewModel.IsRemoveExpiry = true;
        queryViewModel.IsExperience = queryViewModel.IsExperience.HasValue ? queryViewModel.IsExperience.Value : false;
        var query = await _productPackageService.Get(queryViewModel);
        return query;
    }

    /// <summary>
    /// 通过产品包id数组获取产品包列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public  Task<List<BrowseProductPackageViewModel>> GetProductPackageByIds([FromBody] Guid[] ids)
    {
        
        return _productPackageService.GetProductPackageByIds(ids);
    }
    /// <summary>
    ///  通过产品包id拿这些产品包下面所有产品的权限id
    /// </summary>
    /// <param name="ids">产品包ids</param>
    [HttpPost]
    [AllowAnonymous]
    public  Task<IEnumerable<Guid>> GetProductPermissionId([FromBody] Guid[] ids)
    {
        return _productPackageService.GetProductPermissionId(ids);
    }

    /// <summary>
    /// 通过产品包id数组获取每个产品包的复合数据
    /// </summary>
    /// <param name="ids"></param>
    [HttpPost]
    [AllowAnonymous]
    public async Task<List<ProductPackageCompositeAttribute>> GetCompositeAttributeByIds([FromBody] List<Guid> ids)
    {
        //获取所有产品，并按照优惠价格排序
        var productsByDiscountedPrice = await GetAllProductOrderByDiscountedPrice();
        //获取所有产品，并按照实际价格排序
        var productsOrderByPrice = productsByDiscountedPrice.OrderBy(x => x.Price).ToList();

        
        //获取对应的产品包信息
        var productPackages = await GetProductPackageInfo();
        //整理数据返回
        return OrganizeData();

        async Task<List<Product>> GetAllProductOrderByDiscountedPrice()
        {
            var result = await _cacheManager.GetAsync(
                GirvsEntityCacheDefaults<Product>.QueryCacheKey.Create("AllOrderByDiscountedPrice"),
                () =>
                {
                    var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
                    Expression<Func<Product, bool>> expression = x => true;
                    if (tenantId.HasValue && tenantId.Value != Guid.Empty)
                    {
                        expression = expression.And(x => x.TenantId == tenantId);
                    }
                    return _productRepository.GetWhereAsync(expression);
                });

            return result.OrderBy(x => x.DiscountedPrice).ToList();
        }

        Task<Dictionary<Guid, int>> GetAllProductBuyerCount()
        {
            //计算每个产品的购买人数(同一个人同一个产品购买两次，按一次计算)
            return _cacheManager.GetAsync(
                GirvsEntityCacheDefaults<StudentPermission>.QueryCacheKey.Create("AllProductBuyerCount"),
                () => _studentPermissionRepository.GetAllProductBuyerCount()
            );
        }

        Task<List<ProductPackage>> GetProductPackageInfo()
        {
            //查询需要的产品包信息
            return _cacheManager.GetAsync(ids.CreateProductPackageIdsCacheKey(), () =>
            {
                var tenantId = EngineContext.Current.ClaimManager.GetTenantId()?.ToGuid();
                Expression<Func<ProductPackage, bool>> expression = x => ids.Contains(x.Id);
                if (tenantId.HasValue && tenantId.Value != Guid.Empty)
                {
                    expression = expression.And(x => x.TenantId == tenantId);
                }
                return _productPackageRepository.GetWhereAsync(expression);
            });
        }

        List<ProductPackageCompositeAttribute> OrganizeData()
        {
            var result = new List<ProductPackageCompositeAttribute>(ids.Capacity);

            foreach (var id in ids)
            {
                var productPackage = productPackages.FirstOrDefault(p => p.Id == id);
                if (productPackage is null) continue;

                var productIds = productPackage.ProductList ?? [];
                var isExpire = productPackage.PreferentialExpiryTime < DateTime.Now;
                //获取当前产品包中价格最低的产品
                var lowPricedProducts = isExpire ? productsOrderByPrice.FirstOrDefault(p => productIds.Contains(p.Id)) : productsByDiscountedPrice.FirstOrDefault(p => productIds.Contains(p.Id));
               
                result.Add(new ProductPackageCompositeAttribute
                {
                    ProductPackageId = id,
                    Price = lowPricedProducts?.Price ?? 0,
                    DiscountedPrice = lowPricedProducts?.DiscountedPrice ?? 0,
                    LecturerList = productPackage.LecturerList,
                    PreferentialExpiryTime = productPackage.PreferentialExpiryTime,
                    CardImage = productPackage.CardImage,
                    DetailImage = productPackage.DetailImage,
                    NumberOfBuyers = productPackage.NumberOfBuyers,
                });
            }

            return result;
        }
    }
}