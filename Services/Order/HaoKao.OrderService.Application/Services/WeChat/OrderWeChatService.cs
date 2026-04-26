using HaoKao.OrderService.Application.Services.Web;
using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.WeChat;

/// <summary>
/// 订单服务--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OrderWeChatService(IOrderWebService orderWebService) : IOrderWeChatService
{
    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Guid> Create([FromBody] CreateOrderViewModel model)
    {
        return orderWebService.Create(model);
    }

    /// <summary>
    /// 我的订单
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<OrderQueryViewModel> Get([FromQuery] OrderQueryViewModel queryViewModel)
    {
        return orderWebService.Get(queryViewModel);
    }

    /// <summary>
    /// 是否付费用户
    /// </summary>
    [HttpGet]
    public  Task<bool> IsPaidUser()
    {
        return orderWebService.IsPaidUser();
    }

    /// <summary>
    /// 根据订单Id，获取订单详情
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId:guid}")]
    public Task<BrowseOrderViewModel> Get(Guid orderId)
    {
        return orderWebService.Get(orderId);
    }
}