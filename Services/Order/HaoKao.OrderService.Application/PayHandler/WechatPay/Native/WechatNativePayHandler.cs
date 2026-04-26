using HaoKao.OrderService.Application.PayHandler.WechatPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.Native;

public class WechatNativePayHandler : WechatPayHandler<WechatNativePayHandlerConfig>
{
    public override Guid PayHandlerId => new("95119518-75ae-489f-9b63-3eac2ba8e00f");

    public override string PayHandlerName => "微信Native支付";

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.WebSite;

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        //获取请求头信息
        var attachment = PayTokenHandler.GetAttachment();

        var orderNumber = await GenerateOrderNumber(order);

        var request = new CreatePayTransactionNativeRequest
        {
            AppId = Config.AppId,
            MerchantId = Config.MerchantId,
            Description = order.PurchaseName,
            OutTradeNumber = $"{orderNumber}_{DateTime.Now.Millisecond}" ,
            NotifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id),
            ExpireTime = DateTimeOffset.Now.AddHours(2),
            Attachment = attachment,

            Amount = new CreatePayTransactionNativeRequest.Types.Amount { Total = (int)(order.ActualAmount * 100) },
        };

        var response = await WechatTenpayClient.ExecuteCreatePayTransactionNativeAsync(request);
        if (!response.IsSuccessful())
        {
            throw new GirvsException($"下单失败（状态码：{response.RawStatus}，错误代码：{response.ErrorCode}，错误描述：{response.ErrorMessage}）。");
        }

        var result = new WechatNativePayCreateOrderReturn
        {
            QrcodeUrl = response.QrcodeUrl,
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderId = order.Id,
            OrderNumber = order.OrderSerialNumber
        };

        return result;
    }
}