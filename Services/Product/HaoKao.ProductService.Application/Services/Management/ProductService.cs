using Girvs.Driven.Extensions;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Domain.Commands.Product;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Enums;
using HaoKao.ProductService.Domain.Extensions;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 构造函数
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "产品管理",
    "3f38164c-85f8-078e-d92c-4f565f37e891",
    "2048",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ProductService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IProductRepository repository,
    IProductPermissionRepository productPermissionRepository
) : IProductService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IProductPermissionRepository _productPermissionRepository = productPermissionRepository ?? throw new ArgumentNullException(nameof(productPermissionRepository));

    #endregion

    /// <summary>
    /// 根据Id 获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseProductViewModel> Get(Guid id)
    {
        var product = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Product>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdInclude(id)
        ) ?? throw new GirvsException("对应的产品不存在", StatusCodes.Status404NotFound);

        return product.MapToDto<BrowseProductViewModel>();
    }

    /// <summary>
    /// 根据查询条件获取
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryProductViewModel> Get([FromQuery] QueryProductViewModel queryModel)
    {
        var query = queryModel.MapToQuery<ProductQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Product>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryProductViewModel, Product>();
    }

    /// <summary>
    /// 通过产品id数组获取产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseProductViewModel>> GetByIds([FromBody] Guid[] ids)
    {
        var list = await _cacheManager.GetAsync(
            ids.CreateProductIdsIncludeCacheKey(),
            async () =>
            {
                var list = await _repository.GetWhereInclude(ids);
                var productDic = list.ToDictionary(x => x.Id, x => x);
                var result = new List<Product>();
                foreach (var item in ids)
                {
                    if (productDic.TryGetValue(item, out var product))
                    {
                        result.Add(product);
                    }
                }
                return result;
            }
        );

        return list.MapTo<List<BrowseProductViewModel>>();
    }

    /// <summary>
    /// 通过产品id数组获取其中免费产品列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseProductViewModel>> GetFreeProductsByIds([FromBody] Guid[] ids)
    {
        var list = await _cacheManager.GetAsync(
            ids.CreateFreeProductIdsCacheKey(),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id) && x.DiscountedPrice == 0)
        );

        return list.MapTo<List<BrowseProductViewModel>>();
    }

    /// <summary>
    /// 通过直播id数组获取产品列表
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseProductLiveViewModel>> GetByLiveIds([FromBody] List<QueryProductByLiveIdViewModel> models)
    {
        var modelsDictionary = models.ToDictionary(x => x.LiveId, x => x.LiveStatus);
        var ids = models.Select(x => x.LiveId).ToList();
        var products = await _repository.GetByLiveIdsAsync(ids);
        var result = products.MapTo<List<BrowseProductLiveViewModel>>();
        foreach (var x in result)
        {
            var liveId = x.ProductPermissions.FirstOrDefault().PermissionId;
            x.LiveStatus = modelsDictionary.TryGetValue(liveId, out var liveStatus) ? liveStatus : LiveStatus.NotStarted;
        }
        //按照查询直播Id对应排序
        var resultOrder = new List<BrowseProductLiveViewModel>();
        foreach (var x in models)
        {
            var list = result.Where(y => y.LiveId == x.LiveId);
            if (list.Any())
            {
                resultOrder.AddRange(list);
            }
        }
        result = resultOrder;
        return result;
    }

    /// <summary>
    /// 获取我的产品
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<List<MyProductViewModel>> GetMyProduct(ProductType? productType, Guid? userId)
    {
        return GetMyAllProduct(productType, userId, PermissionExpiryType.NotExpired);
    }

    /// <summary>
    /// 获取我的过期产品，未过期产品，所有产品
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<MyProductViewModel>> GetMyAllProduct(ProductType? productType, Guid? userId, PermissionExpiryType permissionExpiryType)
    {
        if (!userId.HasValue) return [];
        var products = await _repository.GetMyAllProduct(productType, userId.Value, permissionExpiryType);
        var result = products.Select(x =>
        {
            var viewModel = x.Item1.MapToDto<MyProductViewModel>();
            viewModel.MyExpireTime = x.Item2;
            viewModel.MyEarliestBuyTime = x.Item3;
            return viewModel;
        }).ToList();

        return result;
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateProductViewModel model)
    {
        var command = model.MapToCommand<CreateProductCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateProductViewModel model)
    {
        var command = model.MapToCommand<UpdateProductCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量开启答疑授权
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("开启答疑", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task OpenAnswering([FromBody] Guid[] ids)
    {
        var command = new SetProductAnsweringCommand(ids, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量关闭答疑授权
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("关闭答疑", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task CloseAnswering([FromBody] Guid[] ids)
    {
        var command = new SetProductAnsweringCommand(ids, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete([FromBody] DeleteProductViewModel model)
    {
        var command = new DeleteProductCommand(model.Ids);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("启用", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Enable([FromBody] ChangeProductStateViewModel model)
    {
        var command = new SetProductEnableCommand(model.Ids, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Disable([FromBody] ChangeProductStateViewModel model)
    {
        var command = new SetProductEnableCommand(model.Ids, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("上架", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesUp(Guid[] id)
    {
        var command = new SetProductShelvesCommand(id, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("下架", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesDown([FromBody] Guid[] id)
    {
        var command = new SetProductShelvesCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 查询复合条件的产品id
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<Guid>> GetProductIds(Guid? subjectId, Guid? permissionId)
    {
        var cacheKeyStr = $"{subjectId}_{permissionId}_productids";
        var productIds = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Product>.QueryCacheKey.Create(cacheKeyStr),
            () => _productPermissionRepository.GetProductIdsBy(subjectId, permissionId)
        );

        return productIds;
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("关联产品", Permission.Relation, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void RelateProduct() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("复制", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void Copy() { }
}