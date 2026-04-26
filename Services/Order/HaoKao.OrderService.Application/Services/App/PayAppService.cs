using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.App;

/// <summary>
/// 发起支付服务--App
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PayAppService(IPayService payService) : IPayAppService
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

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改），或者扫码支付成功，前端页面回调成功
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<bool> CurrentPayNotify([FromBody] CurrentPayNotifyViewModel model)
    {
        //获取支付者对应的支付处理器，并且设置相关配置信息
        return _payService.CurrentPayNotify(model);
    }
}