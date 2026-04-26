using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IOrderService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseOrderViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<OrderQueryViewModel> Get(OrderQueryViewModel queryViewModel);

    /// <summary>
    /// 创建订单表
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<Guid> Create(CreateOrderViewModel model);

    /// <summary>
    /// 管理端手动录入订单
    /// </summary>
    /// <param name="model"></param>
    Task ManualCreate(ManualCreateOrderViewModel model);

    /// <summary>
    /// 根据主键删除指定订单表
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task Cancel(Guid id);

    /// <summary>
    /// 修改订单价格
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task ChangeOrderAmount(ChangeOrderAmountViewModel model);

    /// <summary>
    /// 完成支付，修改订单
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task FinishPayOrder(FinishPayOrderViewModel model);

    /// <summary>
    /// 获取销售统计列表
    /// </summary>
    Task<SalesStatQueryViewModel> GetSalesStatList(SalesStatQueryViewModel model);

    /// <summary>
    /// 获取销售统计详情列表
    /// </summary>
    Task<SalesStatDetailQueryViewModel> GetSalesStatDetailList(SalesStatDetailQueryViewModel model);

    /// <summary>
    /// 获取本场直播间购物车入口购买产品的订单金额合计金额
    /// </summary>
    Task<decimal> GetLiveTotalAmount(Guid liveId);


    /// <summary>
    /// 获取本场直播间成交人数
    /// </summary>
    Task<int> GetLiveTransactionPeopleNumber(Guid liveId);
}