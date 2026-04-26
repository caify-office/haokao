using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Repositories;
using System.Linq.Expressions;

namespace HaoKao.ProductService.Application.Services.Web;

/// <summary>
/// 产品web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ProductWebService(
    IProductRepository repository,
    IProductService productService,
    IProductPackageService productPackageService)
    : IProductWebService
{
    private readonly IProductPackageService _productPackageService = productPackageService ?? throw new ArgumentNullException(nameof(productPackageService));
    private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    private readonly IProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    /// <summary>
    /// 根据Id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<BrowseProductViewModel> Get(Guid id)
    {
        var result= await _productService.Get(id);
        var userProduct = await GetMyProduct(ProductType.Live);
        if (userProduct.Any() == false) return result;
        result.IsBuy = userProduct.Any(p => p.Id == result.Id);
        return result;
    }

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public  async Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        var result =await  _productService.GetByIds(ids);
        var userProduct = await GetMyProduct(ProductType.Live);
        if (userProduct.Any() == false) return result;
        result.ForEach(x => { x.IsBuy = userProduct.Any(p => p.Id == x.Id); });
        return result;
    }
    /// <summary>
    /// 通过产品id数组获取其中免费产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public  Task<List<BrowseProductViewModel>> GetFreeProductsByIds([FromBody] Guid[] ids)
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
    public async Task<List<BrowseProductPriceViewModel>> GetPriceByIds(Guid productPackageId, [FromBody] Guid[] ids)
    {
        //获取产品包信息
      var productPackge= await _productPackageService.Get(productPackageId) ?? throw new GirvsException(StatusCodes.Status400BadRequest, "对应的产品包不存在");
        //判定产品包是否过期
        var isExpire = productPackge.PreferentialExpiryTime < DateTime.Now;
        //获取产品信息数组
        var productList = await _productService.GetByIds(ids);
        //整理数据返回
        var result = new List<BrowseProductPriceViewModel>();
        productList.ForEach(x =>
        {
            result.Add(new BrowseProductPriceViewModel
            {
              Id = x.Id,
              IsExpire= isExpire,
              Price = x.Price,
              DiscountedPrice=x.DiscountedPrice
            });
        });
        return result;
    }

    /// <summary>
    /// 获取我的产品(包含过期时间和最早购买时间)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public  Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId()?.ToGuid();
        return _productService.GetMyProduct(productType, userId);
    }
    /// <summary>
    /// 获取我的过期产品，未过期产品，所有产品(包含过期时间和最早购买时间)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public  Task<List<MyProductViewModel>> GetMyAllProduct(ProductType? productType, PermissionExpiryType permissionExpiryType)
    {
       var userId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        return _productService.GetMyAllProduct(productType, userId, permissionExpiryType);
    }


    /// <summary>
    /// 获取我的产品权限
    /// </summary>
    /// <param name="productType">产品类型</param>
    /// <param name="subjectId">科目id（不传取所有科目）</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BrowseProductPermissionViewModel>> GetMyProductPermisson(ProductType? productType,Guid? subjectId)
    {
        Expression<Func<BrowseProductPermissionViewModel, bool>> expression = x => true;
        if (subjectId.HasValue)
        {
            expression = expression.And(x => x.SubjectId == subjectId);
        }
        var myProduct = await GetMyProduct(productType);
        var myProductPermisions = myProduct.SelectMany(x => x.ProductPermissions).AsQueryable().Where(expression)
            .GroupBy(x => new { x.SubjectId, x.SubjectName, x.PermissionId, x.PermissionName, x.Category })
            .Select(x => new BrowseProductPermissionViewModel
            {
                PermissionId = x.Key.PermissionId,
                PermissionName = x.Key.PermissionName,
                Category = x.Key.Category,
                SubjectName = x.Key.SubjectName,
                SubjectId = x.Key.SubjectId,
            }).ToList();
        return myProductPermisions;
    }

    /// <summary>
    /// 获取我的产品所属科目
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<MyProductSubjectViewModel>> GetMyProductSubject(ProductType? productType)
    {
        //获取我的产品
        var userId = EngineContext.Current.ClaimManager.GetUserId()?.ToGuid();
        if (!userId.HasValue) return [];
        var productPermission = await _repository.GetMyProductSubject(userId.Value, productType);

        //去重
        var result = productPermission.Select(x =>
        {
           return new MyProductSubjectViewModel
           {
                SubjectId = x.Item1,
                SubjectName = x.Item2,
            };
        });
        return result.ToList();
    }

    /// <summary>
    /// 通过直播id数组获取产品列表
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<List<BrowseProductLiveViewModel>> GetByLiveIds([FromBody] List<QueryProductByLiveIdViewModel> models)
    {
       var result=await _productService.GetByLiveIds(models);
        var userProduct = await GetMyProduct(ProductType.Live);
        if (userProduct.Any() == false) return result;
        result.ForEach(x => { x.IsBuy = userProduct.Any(p => p.Id == x.Id); });
        return result;
    }
}