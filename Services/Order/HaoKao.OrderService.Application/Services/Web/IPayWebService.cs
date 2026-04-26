using HaoKao.OrderService.Application.PayHandler;

namespace HaoKao.OrderService.Application.Services.Web;

public interface IPayWebService : IAppWebApiService
{
    /// <summary>
    /// 创建对应支付平台的订单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    Task<IOrderReturn> CreateRemoteOrder(Guid platformPayerId, Guid orderId);
}