using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IPayService : IAppWebApiService, IManager
{
    /// <summary>
    /// 下单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<IOrderReturn> CreateRemoteOrder(Guid platformPayerId, Guid orderId);

    /// <summary>
    /// 远程服务器回调，防止掉单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<dynamic> PayNotifyMessage(Guid platformPayerId, Guid orderId);

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改）
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> CurrentPayNotify(CurrentPayNotifyViewModel model);
}