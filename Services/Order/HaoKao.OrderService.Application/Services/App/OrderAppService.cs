using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.App;

/// <summary>
/// 订单服务--App
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OrderAppService(IOrderService orderService) : IOrderAppService
{
    private readonly IOrderService _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

    /// <summary>
    /// App 创建订单
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Guid> Create([FromBody] CreateOrderViewModel model)
    {
        return _orderService.Create(model);
    }

    /// <summary>
    /// 我的订单
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<OrderQueryViewModel> Get([FromQuery] OrderQueryViewModel queryViewModel)
    {
        queryViewModel.CreatorId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        return _orderService.Get(queryViewModel);
    }

    /// <summary>
    /// 根据订单Id，获取订单详情
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId:guid}")]
    public Task<BrowseOrderViewModel> Get(Guid orderId)
    {
        return _orderService.Get(orderId);
    }
}