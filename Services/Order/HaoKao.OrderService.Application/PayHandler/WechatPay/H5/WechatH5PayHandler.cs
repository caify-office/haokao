using HaoKao.OrderService.Application.PayHandler.WechatPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.H5;

public class WechatH5PayHandler : WechatPayHandler<WechatPayHandlerConfig>
{
    public override Guid PayHandlerId => new("BA070CBE-3F74-44C3-80F0-5C734751B30B");

    public override string PayHandlerName => "微信H5支付";

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.WebSite;

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        //获取请求头信息
        var attachment = PayTokenHandler.GetAttachment();

        var orderNumber = await GenerateOrderNumber(order);

        var request = new CreatePayTransactionH5Request
        {
            AppId = Config.AppId,
            MerchantId = Config.MerchantId,
            Description = order.PurchaseName,
            OutTradeNumber = $"{orderNumber}_{DateTime.Now.Millisecond}" ,
            NotifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id),
            ExpireTime = DateTimeOffset.Now.AddHours(2),
            Attachment = attachment,

            Amount = new CreatePayTransactionH5Request.Types.Amount { Total = (int)(order.ActualAmount * 100) },
            Scene = new CreatePayTransactionH5Request.Types.Scene
            {
                ClientIp = EngineContext.Current.HttpContext.Request.GetUserIpAddress(),
                H5 = new CreatePayTransactionH5Request.Types.Scene.Types.H5
                {
                    Type = "Wap",
                }
            }
        };

        var response = await WechatTenpayClient.ExecuteCreatePayTransactionH5Async(request);
        if (!response.IsSuccessful())
        {
            throw new GirvsException(
                $"下单失败（状态码：{response.RawStatus}，错误代码：{response.ErrorCode}，错误描述：{response.ErrorMessage}）。");
        }

        var result = new WechatH5PayCreateOrderReturn
        {
            H5Url = response.H5Url,
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderId = order.Id,
            OrderNumber = order.OrderSerialNumber
        };

        return result;
    }
}