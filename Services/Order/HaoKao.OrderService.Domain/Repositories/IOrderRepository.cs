using HaoKao.OrderService.Domain.Entities;

namespace HaoKao.OrderService.Domain.Repositories;

/// <summary>
/// 订单仓储
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// 更新订单
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    bool Update(Order order);

    /// <summary>
    /// 查询出所有租户下的过期订单
    /// </summary>
    /// <returns></returns>
    Task<List<Order>> QueryAllExpiredOrderList();

    /// <summary>
    /// 根据订单Ids更新为失效订单
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task SetOrderExpired(IEnumerable<Guid> ids);

    /// <summary>
    /// 获取本场直播间购物车入口购买产品的订单金额合计金额
    /// </summary>
    Task<decimal> GetLiveTotalAmount(Guid liveId);

    Task<int> GetLiveTransactionPeopleNumber(Guid liveId);

    Task InitProductSales();

    Task AddProductSales(Order order);
}