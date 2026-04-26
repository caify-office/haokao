using HaoKao.OrderService.Application.PayHandler.Config;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.ApplePay;

public class AppleInAppPurchaseHandler : PayHandlerBase<AppleInAppPurchaseHanlderConfig>
{
    public override PlatformPayerScenes Scenes => PlatformPayerScenes.App;

    public override Guid PayHandlerId => new("E9BBAEDB-771D-4F67-B4FD-949325E0057C");

    public override string PayHandlerName => "苹果应用内支付";

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        var orderNumber = await GenerateOrderNumber(order);
        var result = new AppleInAppPurchaseCreateOrderReturn
        {
            OrderId = order.Id,
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderNumber = order.OrderSerialNumber,
        } as IOrderReturn;

        return result;
    }

    public override Task<dynamic> ReceivePayNotifyMessage(Guid orderId)
    {
        throw new GirvsException("苹果内购没有回调");
    }
}