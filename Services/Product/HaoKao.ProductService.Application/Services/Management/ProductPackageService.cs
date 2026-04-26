using Girvs.Driven.Extensions;
using HaoKao.ProductService.Application.ViewModels.Product;
using HaoKao.ProductService.Application.ViewModels.ProductPackage;
using HaoKao.ProductService.Domain.Commands.ProductPackage;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Extensions;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 产品包接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "产品包",
    "2f19cbb3-a6cf-edbb-a30c-7271fca7906e",
    "2048",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ProductPackageService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IProductPackageRepository repository,
    IProductRepository productRepository,
    IProductService productService) : IProductPackageService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IProductPackageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseProductPackageViewModel> Get(Guid id)
    {
        var productPackage = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductPackage>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的产品包不存在", StatusCodes.Status400BadRequest);

        return productPackage.MapToDto<BrowseProductPackageViewModel>();
    }

    /// <summary>
    /// 通过产品包id数组获取产品列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<ProductListViewModel>> GetProducts(Guid id)
    {
        //获取产品包详情
        var productPackage = await Get(id);
        var productIdList = productPackage?.ProductList ?? [];
        //获取产品列表详情
        var productList = await _cacheManager.GetAsync(
            productIdList.CreateProductIdsCacheKey(),
            () => _productRepository.GetWhereAsync(x => productIdList.Contains(x.Id))
        );
        var productDictionary = productList.ToDictionary(x => x.Id);
        //调整排序
        var result2 = new List<Product>();
        productPackage.ProductList.ForEach(x =>
        {
            if (productDictionary.TryGetValue(x, out Product p))
            {
                result2.Add(p);
            }
        });
        return result2.MapTo<List<ProductListViewModel>>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryProductPackageViewModel> Get([FromQuery] QueryProductPackageViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<ProductPackageQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductPackage>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryProductPackageViewModel, ProductPackage>();
    }

    /// <summary>
    /// 通过产品包id数组获取产品包列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseProductPackageViewModel>> GetProductPackageByIds([FromBody] Guid[] ids)
    {
        var tempQuery = await _cacheManager.GetAsync(ids.CreateProductPackageIdsCacheKey(),
                                                     async () =>
                                                     {
                                                         var list = await _repository.GetWhereAsync(x => ids.Contains(x.Id));
                                                         return AdjustOrder();

                                                         List<ProductPackage> AdjustOrder()
                                                         {
                                                             var productDic = list.ToDictionary(x => x.Id, x => x);
                                                             var result = new List<ProductPackage>();
                                                             foreach (var item in ids)
                                                             {
                                                                 if (productDic.TryGetValue(item, out var product))
                                                                 {
                                                                     result.Add(product);
                                                                 }
                                                             }
                                                             return result;
                                                         }
                                                     }
        );

        return tempQuery.MapTo<List<BrowseProductPackageViewModel>>();
    }

    /// <summary>
    ///  通过产品包id拿这些产品包下面所有产品的权限id
    /// </summary>
    /// <param name="ids">产品包ids</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<IEnumerable<Guid>> GetProductPermissionId([FromBody] Guid[] ids)
    {
        var productPackages = await GetProductPackageByIds(ids);
        var productIds = productPackages.SelectMany(x => x.ProductList).Distinct().ToArray();
        var products = await _productService.GetByIds(productIds);
        var productPermissions = products.SelectMany(x => x.ProductPermissions).Select(x => x.PermissionId).Distinct();
        return productPermissions;
    }

    /// <summary>
    /// 创建产品包
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateProductPackageViewModel model)
    {
        var command = model.MapToCommand<CreateProductPackageCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定产品包
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteProductPackageCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定产品包
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateProductPackageViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateProductPackageCommand>();

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
    /// <param name="id">主键</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("启用", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Enable(Guid id)
    {
        var command = new SetProductPackageEnableCommand(id, true);

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
    /// <param name="id">主键</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task DisEnable(Guid id)
    {
        var command = new SetProductPackageEnableCommand(id, false);

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
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("上架", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesUp(Guid id)
    {
        var command = new SetProductPackageShelvesCommand(id, true);

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
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("下架", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesDown(Guid id)
    {
        var command = new SetProductPackageShelvesCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置产品
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model"></param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设置产品", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ProductList(Guid id, SetProductPackageProductListViewModel model)
    {
        var command = new SetProductPackageProductListCommand(id, model.ProductList);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion

    [HttpGet]
    [ServiceMethodPermissionDescriptor("生成购课链接", Permission.Post_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void GeneratePurchaseLink() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("复制", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void Copy() { }
}