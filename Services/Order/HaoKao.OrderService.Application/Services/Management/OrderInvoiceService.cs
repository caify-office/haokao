using HaoKao.OrderService.Application.ViewModels.OrderInvoice;
using HaoKao.OrderService.Domain.Commands.OrderSqInvoice;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.Repositories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 发票管理
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "发票管理",
    "08db9272-0aed-4ab8-81dc-ace77bea8494",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    1
)]
public class OrderInvoiceService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IOrderInvoiceRepository repository,
    IOptions<JsonSerializerOptions> jsonOptions
) : IOrderInvoiceService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseOrderInvoiceViewModel> Get(Guid id)
    {
        var entityJson = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<OrderInvoice>.ByIdCacheKey.Create(id.ToString()),
            async () =>
            {
                var entity = await repository.GetByIdAsync(id);
                return JsonSerializer.Serialize(entity, jsonOptions.Value);
            });

        if (entityJson == null) throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        var orderInvoice = JsonSerializer.Deserialize<OrderInvoice>(entityJson, jsonOptions.Value);

        return orderInvoice.MapToDto<BrowseOrderInvoiceViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<OrderInvoiceQueryViewModel> Get([FromQuery] OrderInvoiceQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<OrderInvoiceQuery>();

        var queryJson = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<OrderInvoice>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await repository.GetByQueryAsync(query);
                return JsonSerializer.Serialize(query, jsonOptions.Value);
            });

        query = JsonSerializer.Deserialize<OrderInvoiceQuery>(queryJson, jsonOptions.Value);

        return query.MapToQueryDto<OrderInvoiceQueryViewModel, OrderInvoice>();
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateOrderInvoiceViewModel model)
    {
        var command = model.MapToCommand<UpdateOrderInvoiceCommand>();

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateOrderInvoiceViewModel model)
    {
        var command = model.MapToCommand<CreateOrderInvoiceCommand>();

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("导出", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void Export() { }
}