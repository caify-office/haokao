using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Application.ViewModels.Order;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 支付服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class PayService(IPayHandlerService payHandlerService, IOrderRepository orderRepository) : IPayService
{
    /// <summary>
    /// 创建远程订单，也就所谓的下单
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId">订单Id</param>
    /// <returns></returns>
    [HttpPost("{platformPayerId:guid}/{orderId:guid}")]
    public async Task<IOrderReturn> CreateRemoteOrder(Guid platformPayerId, Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order == null || order.OrderState == OrderState.PaymentSuccessful)
        {
            throw new GirvsException("当前订单不存在或者已成功支付，请勿重复支付");
        }

        var payerHandler = await payHandlerService.GetByPlatformPayerId(platformPayerId);
        order.PlatformPayerId = platformPayerId;

        // Read from http body
        Dictionary<string, string> extendParams = null;
        using var reader = new StreamReader(EngineContext.Current.HttpContext.Request.Body);
        var body = await reader.ReadToEndAsync();
        if (!string.IsNullOrEmpty(body))
        {
            extendParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
        }
        return await payerHandler.CreateOrder(order, extendParams);
    }

    /// <summary>
    /// 支付通知回调
    /// </summary>
    /// <param name="platformPayerId"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("{platformPayerId:guid}/{orderId:guid}")]
    [AllowAnonymous]
    public async Task<dynamic> PayNotifyMessage(Guid platformPayerId, Guid orderId)
    {
        //获取支付者对应的支付处理器，并且设置相关配置信息
        var payerHandler = await payHandlerService.GetByPlatformPayerId(platformPayerId);
        //处理回调
        return await payerHandler.ReceivePayNotifyMessage(orderId);
    }

    /// <summary>
    /// 当前回调（如App支付后，在客户端就已经返回支付成功的消息，对订单进行修改），或者扫码支付成功，前端页面回调成功
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CurrentPayNotify([FromBody] CurrentPayNotifyViewModel model)
    {
        //获取支付者对应的支付处理器，并且设置相关配置信息
        var payerHandler = await payHandlerService.GetByPlatformPayerId(model.PlatformPayerId);

        var order = await orderRepository.GetByIdAsync(model.OrderId);

        return await payerHandler.CurrentPayNotify(new CurrentPayNotifyModel
        {
            Order = order,
            OrderSerialNumber = model.OrderNumber,
            CurrentPayNotifySign = model.CurrentPayNotifySign,
            IosRestorePurchase = model.IosRestorePurchase,
        });
    }
}