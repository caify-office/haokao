using HaoKao.ProductService.Application.ViewModels.RelatedProduct;
using HaoKao.ProductService.Domain.Commands.RelatedProduct;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 关联产品接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class RelatedProductService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRelatedProductRepository repository
) : IRelatedProductService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRelatedProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public async Task<BrowseRelatedProductViewModel> Get(Guid id)
    {
        var relatedProduct = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<RelatedProduct>.ByIdCacheKey.Create(id.ToString()),
              async () => await _repository.GetByIdAsync(id)
            ) ?? throw new GirvsException("对应的关联产品不存在", StatusCodes.Status404NotFound);
        return relatedProduct.MapToDto<BrowseRelatedProductViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<RelatedProductQueryViewModel> Get([FromQuery]RelatedProductQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<RelatedProductQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RelatedProduct>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<RelatedProductQueryViewModel, RelatedProduct>();
    }
            
    /// <summary>
    /// 批量创建关联产品
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] IList<CreateRelatedProductViewModel>  model)
    {
        var models = model.MapTo<IList<CreateRelatedProductModel>>();
        var command = new CreateRelatedProductCommand(models);
        
        await _bus.SendCommand(command);
        
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定关联产品
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpPost("delete")]
    public async Task Delete(Guid[] ids)
    {
        var command = new DeleteRelatedProductCommand(ids);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    

    #endregion
}