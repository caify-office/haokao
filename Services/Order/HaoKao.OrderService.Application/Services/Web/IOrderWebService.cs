using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.Web;

public interface IOrderWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据Id获取订单详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowseOrderViewModel> Get(Guid id);

    /// <summary>
    /// 我的订单
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<OrderQueryViewModel> Get([FromQuery] OrderQueryViewModel queryViewModel);

    /// <summary>
    /// 是否付费用户
    /// </summary>
    Task<bool> IsPaidUser();

    /// <summary>
    /// 创建订单表
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<Guid> Create(CreateOrderViewModel model);

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    Task Cancel(Guid id);
}