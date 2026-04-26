using HaoKao.ProductService.Application.Services.Management;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.RelatedProduct;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Web;

/// <summary>
/// 关联产品接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]

public class RelatedProductWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRelatedProductRepository repository,
    IRelatedProductService relatedProductService) : IRelatedProductWebService
{
    #region 初始参数
    private readonly IRelatedProductService _relatedProductService = relatedProductService ?? throw new ArgumentNullException(nameof(relatedProductService));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IRelatedProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public  Task<BrowseRelatedProductViewModel> Get(Guid id)
    {
        return _relatedProductService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public  Task<RelatedProductQueryViewModel> Get([FromQuery]RelatedProductQueryViewModel queryViewModel)
    {
        return _relatedProductService.Get(queryViewModel);
    }

    /// <summary>
    /// 通过产品id数组获取关联了这些产品的产品信息
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<List<ProductListViewModel>> GetRelatedProductByIds([FromBody] Guid[] ids)
    {
        var idsKey=string.Join(",", ids).ToMd5();
        var key = GirvsEntityCacheDefaults<RelatedProduct>.QueryCacheKey.Create(idsKey);
        var relatedProductList = await _cacheManager.GetAsync(key, async () =>
        {
           return await _repository.GetRelatedProductByIds(ids);
        }); 
        
        return relatedProductList.MapTo<List<ProductListViewModel>>();
    }
    #endregion
}