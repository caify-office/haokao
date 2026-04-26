using HaoKao.OrderService.Application.PayHandler.AliPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.AliPay.App;

public class AliPayAppHandler : AliPayHandler<AliPayAppHandlerConfig>
{
    public override string PayHandlerName => "支付宝App 拉起支付";

    public override Guid PayHandlerId => new("703B5422-3232-45AD-90A2-B0C966300238");

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.App;

    public override async Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        // 实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
        var request = new AlipayTradeAppPayRequest();

        var orderNumber = await GenerateOrderNumber(order);

        // SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
        request.SetBizModel(new AlipayTradeAppPayModel
        {
            // 订单标题
            Subject = order.PurchaseName,
            // 订单附加信息
            Body = PayHandlerName,
            // 订单号
            OutTradeNo = $"{orderNumber}_{DateTime.Now.Millisecond}",
            // 商品代码
            ProductCode = order.PurchaseProductId.ToString(),
            // 金额 传入实际金额，而不是订单金额
            TotalAmount = order.ActualAmount.ToString(),
            // 过期时间
            TimeoutExpress = "120m",

            // 用于回调时获取的自定义数据
            PassbackParams = PayTokenHandler.GetAttachment(),
        });

        // 设置异步通知地址
        var notifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id);
        request.SetNotifyUrl(notifyUrl);

        var response = Client.SdkExecute(request);
        if (response.IsError)
        {
            // 错误处理
            throw new GirvsException($"下单失败（错误代码：{response.Code}，错误描述：{response.Msg}）。");
        }

        // 页面输出的response.Body就是orderString可以直接给客户端请求，无需再做处理 。
        return new AliPayAppCreateOrderReturn
        {
            AppId = Config.AppId,
            Body = response.Body,
            CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
            OrderId = order.Id,
            OrderNumber = order.OrderSerialNumber,
        };
    }
}