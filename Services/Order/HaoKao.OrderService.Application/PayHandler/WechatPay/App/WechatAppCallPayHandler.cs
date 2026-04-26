using HaoKao.OrderService.Application.PayHandler.WechatPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.App;

/// <summary>
/// 微信App 调起支付者
/// </summary>
public class WechatAppCallPayHandler : WechatPayHandler<WechatAppCallPayHandlerConfig>
{
    public override string PayHandlerName => "微信App拉取支付";

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.App;

    public override Guid PayHandlerId => "C4F9F5C0-085A-4C92-9FB8-858CF2B1FFCE".ToGuid();

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        //获取请求头信息
        var attachment = PayTokenHandler.GetAttachment();

        var orderNumber = await GenerateOrderNumber(order);

        var request = new CreatePayTransactionAppRequest
        {
            OutTradeNumber = $"{orderNumber}_{DateTime.Now.Millisecond}" ,
            AppId = Config.AppId,
            Description = order.PurchaseName,
            NotifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id),
            Attachment = attachment,

            Amount = new CreatePayTransactionJsapiRequest.Types.Amount { Total = (int)(order.ActualAmount * 100) },
        };

        var response = await WechatTenpayClient.ExecuteCreatePayTransactionAppAsync(request);
        if (!response.IsSuccessful())
        {
            throw new GirvsException(
                $"下单失败（状态码：{response.RawStatus}，错误代码：{response.ErrorCode}，错误描述：{response.ErrorMessage}）。");
        }

        // var callBackResult = await CreateLocalOrder(product);
        // if (!callBackResult)
        // {
        //     throw new GirvsException("创建订单失败，请重新尝试", 400);
        // }

        var result = new WechatAppCallPayCreateOrderReturn
        {
            AppId = Config.AppId,
            PartnerId = Config.MerchantId,
            PrepayId = response.PrepayId,
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderId = order.Id,
            OrderNumber = order.OrderSerialNumber
        };

        result.GenerateSignature(Config.CertificatePrivateKey);

        return result;
    }
}