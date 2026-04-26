using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.App;

public interface IPayAppService : IAppWebApiService
{
    /// <summary>
    /// 创建对应支付平台的订单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<IOrderReturn> CreateRemoteOrder(Guid platformPayerId, Guid orderId);

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改）
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> CurrentPayNotify(CurrentPayNotifyViewModel model);
}