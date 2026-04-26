using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.Services.Management;

namespace HaoKao.OrderService.Application.Services.Web;

/// <summary>
/// 发起支付服务--Web
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PayWebService(IPayService payService) : IPayWebService
{
    private readonly IPayService _payService = payService ?? throw new ArgumentNullException(nameof(payService));

    /// <summary>
    /// 创建对应支付平台的订单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("{platformPayerId:guid}/{orderId:guid}")]
    public Task<IOrderReturn> CreateRemoteOrder(Guid platformPayerId, Guid orderId)
    {
        return _payService.CreateRemoteOrder(platformPayerId, orderId);
    }
}