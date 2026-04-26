using HaoKao.Common.Enums;
using HaoKao.Common.Events.OpenPlatform;
using HaoKao.OrderService.Application.ViewModels.Order;
using HaoKao.OrderService.Domain.Commands.Order;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.Repositories;
using HaoKao.OrderService.Domain.Statistics;
using HaoKao.OrderService.Domain.Works;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 订单表接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "订单明细",
    "595f86f1-4f45-4f92-aecf-77f522fce8fa",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    1
)]
public class OrderService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    IEventBus eventBus,
    INotificationHandler<DomainNotification> notifications,
    IOrderRepository repository,
    ISalesStatisticsFactory factory,
    IMapper mapper,
    IOptions<JsonSerializerOptions> jsonOptions
) : IOrderService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseOrderViewModel> Get(Guid id)
    {
        var orderJson = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Order>.ByIdCacheKey.Create(id.ToString()),
            async () =>
            {
                var entity = await repository.GetByIdAsync(id);
                return System.Text.Json.JsonSerializer.Serialize(entity, jsonOptions.Value);
            });

        if (orderJson == null) throw new GirvsException("对应的订单表不存在", StatusCodes.Status404NotFound);

        var order = System.Text.Json.JsonSerializer.Deserialize<Order>(orderJson, jsonOptions.Value);

        return order.MapToDto<BrowseOrderViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<OrderQueryViewModel> Get([FromQuery] OrderQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<OrderQuery>();

        var queryJson = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Order>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await repository.GetByQueryAsync(query);
                return System.Text.Json.JsonSerializer.Serialize(query, jsonOptions.Value);
            });

        query = System.Text.Json.JsonSerializer.Deserialize<OrderQuery>(queryJson, jsonOptions.Value);

        return query.MapToQueryDto<OrderQueryViewModel, Order>();
    }

    /// <summary>
    /// 创建订单表
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateOrderViewModel model)
    {
     
        //签名计算
        var sign = string.Format("{0}-{1}-{2}-{3}", model.ActualAmount, model.OrderAmount,model.Phone, "zhuofan168").ToMd5().ToLower();
        if (model.Sign != sign)
        {
            throw new GirvsException("数据异常，请重新下单");
        }
        if (model.PurchaseProductContents?.Count == 0)
        {
            throw new GirvsException("缺少产品购买明细，无法下单");
        }
        //判定是否存在过期商品
        if (model.PurchaseProductContents.Any(x => x.ExpiryTime <= DateTime.Now && x.ExpiryTimeTypeEnum == (int)ExpiryTimeTypeEnum.Date))
        {
            throw new GirvsException("存在过期商品，无法下单");
        }
        var id = Guid.NewGuid();
        var command = new CreateOrderCommand(
            id,
            model.OrderSerialNumber,
            model.PurchaseProductId,
            model.PurchaseName,
            model.PurchaseProductType,
            model.OrderAmount,
            model.ActualAmount,
            model.Phone,
            model.LiveId,
            model.PurchaseProductContents,
            model.CreatorId,
            model.CreatorName,
            model.CreateTime,
            model.UpdateTime,
            model.ClientId,
            model.ClientName
        );

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return id;
    }

    /// <summary>
    /// 管理端手动录入订单
    /// </summary>
    /// <param name="model"></param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("手动录入订单", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ManualCreate([FromBody] ManualCreateOrderViewModel model)
    {
        if (model.CreatorId == Guid.Empty)
        {
            model.CreatorId = Guid.NewGuid();
            model.CreatorName = model.CreatorName;
            await eventBus.PublishAsync(new CreateRegisterUserEvent(model.CreatorId, model.Phone, model.CreatorName));
        }
        await Create(model);
    }

    /// <summary>
    /// 根据主键删除指定订单表
    /// </summary>
    /// <param name="id">主键</param>
    [NonAction]
    public async Task Delete(Guid id)
    {
        var command = new DeleteOrderCommand(id);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public async Task Cancel(Guid id)
    {
        var command = new CancelOrderCommand(id);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 修改订单价格
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("调价", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ChangeOrderAmount([FromBody] ChangeOrderAmountViewModel model)
    {
        var command = new ChangeOrderAmountCommand(
            model.OrderId,
            model.ProductContents
        );

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 完成支付订单状态
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("修改状态", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task FinishPayOrder([FromBody] FinishPayOrderViewModel model)
    {
        var command = new FinishPayOrderCommand(
            model.Id,
            model.PlatformPayerId,
            model.PlatformPayerName,
            model.OrderSerialNumber,
            model.OrderNumber,
            model.OrderState,
            model.IosRestorePurchase
        );

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取销售统计列表
    /// </summary>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("销售统计", Permission.Read, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<SalesStatQueryViewModel> GetSalesStatList([FromQuery] SalesStatQueryViewModel model)
    {
        model.TenantIds ??= [EngineContext.Current.ClaimManager.GetTenantId().To<Guid>()];

        var result = await factory.Create(model.Dimension).GetSalesStatList(
            model.StartDate,
            model.EndDate,
            model.PageIndex, model.PageSize,
            [.. model.TenantIds]
        );

        model.RecordCount = result.RecordCount;
        model.Result = mapper.Map<List<SalesStatListViewModel>>(result.Items);

        return model;
    }

    /// <summary>
    /// 获取销售统计详情列表
    /// </summary>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("销售统计详情", Permission.Read_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<SalesStatDetailQueryViewModel> GetSalesStatDetailList([FromQuery] SalesStatDetailQueryViewModel model)
    {
        model.TenantIds ??= [EngineContext.Current.ClaimManager.GetTenantId().To<Guid>()];
        var result = await factory.Create(model.Dimension).GetSalesStatListDetail(
            model.StartDate,
            model.EndDate,
            model.PageIndex,
            model.PageSize,
            [.. model.TenantIds]
        );

        model.RecordCount = result.RecordCount;
        model.Result = mapper.Map<List<SalesStatDetailListViewModel>>(result.Items);

        return model;
    }

    /// <summary>
    /// 获取本场直播间购物车入口购买产品的订单金额合计金额
    /// </summary>
    [HttpGet("{liveId:guid}")]
    [ServiceMethodPermissionDescriptor("直播销售金额统计", Permission.Post_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<decimal> GetLiveTotalAmount(Guid liveId)
    {
        return repository.GetLiveTotalAmount(liveId);
    }

    /// <summary>
    /// 获取本场直播间成交人数
    /// </summary>
    [HttpGet("{liveId:guid}")]
    [ServiceMethodPermissionDescriptor("直播销售金额统计", Permission.Post_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<int> GetLiveTransactionPeopleNumber(Guid liveId)
    {
        return repository.GetLiveTransactionPeopleNumber(liveId);
    }

    [HttpGet, AllowAnonymous]
    public Task UpdateIsPaidStudent([FromServices] IUpdateIsPaidOrderWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task InitProductSales()
    {
        return repository.InitProductSales();
    }
}