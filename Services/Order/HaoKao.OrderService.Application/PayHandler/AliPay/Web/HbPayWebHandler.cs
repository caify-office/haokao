using HaoKao.OrderService.Application.PayHandler.AliPay.Base;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.PayHandler.AliPay.Web;

public class HbPayWebHandler : AliPayHandler<HbPayWebHandlerConfig>
{
    public override string PayHandlerName => "花呗电脑网站支付";

    public override Guid PayHandlerId => new("95e3a237-b106-4650-8408-ac272f9aedce");

    public override PlatformPayerScenes Scenes => PlatformPayerScenes.WebSite;

    public override Task<IOrderReturn> CreateOrder(Order order, Dictionary<string, string> extendParams)
    {
        {
            // 实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            var request = new AlipayTradePagePayRequest();

            var orderNumber = OrderNumberGenerator.Generate(Config.OrderPrefix);

            // SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            var bizModel = new AlipayTradePagePayModel
            {
                // 订单标题
                Subject = order.PurchaseName,
                // 订单附加信息
                Body = PayHandlerName,
                // 订单号
                OutTradeNo = $"{orderNumber}_{DateTime.Now.Millisecond}",
                // 销售产品码，与支付宝签约的产品码名称。注：目前电脑支付场景下仅支持FAST_INSTANT_TRADE_PAY
                ProductCode = "FAST_INSTANT_TRADE_PAY",
                // 金额 传入实际金额，而不是订单金额
                TotalAmount = order.ActualAmount.ToString(),
                // 过期时间
                TimeExpire = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"),

                // 用于回调时获取的自定义数据
                PassbackParams = PayTokenHandler.GetAttachment(),
            };

            // 读取花呗分期数据
            extendParams.TryGetValue("hb_fq_num", out var hbFqNum);
            extendParams.TryGetValue("hb_fq_seller_percent", out var hbFqSellerPercent);
            var installmentConfigs = JsonConvert.DeserializeObject<List<InstallmentConfig>>(Config.InstallmentConfig);
            var installmentConfig = installmentConfigs?.FirstOrDefault(x => x.HbFqNum == hbFqNum && x.HbFqSellerPercent == hbFqSellerPercent);
            if (installmentConfig != null)
            {
                // 花呗分期支持
                bizModel.ExtendParams = new ExtendParams
                {
                    HbFqNum = hbFqNum,
                    HbFqSellerPercent = hbFqSellerPercent,
                };
            }

            request.SetBizModel(bizModel);

            // 设置异步通知地址
            var notifyUrl = BuildNotifyUrl(order.PlatformPayerId, order.Id);
            request.SetNotifyUrl(notifyUrl);

            // 设置支付成功后的返回地址
            var returnUrl = $"{Config.ReturnUrl}?orderId={order.Id}&platformPayerId={order.PlatformPayerId}";
            request.SetReturnUrl(returnUrl);

            // 这里和普通的接口调用不同，使用的是sdkExecute
            var response = Client.SdkExecute(request);
            if (response.IsError)
            {
                // 错误处理
                throw new GirvsException($"下单失败（错误代码：{response.Code}，错误描述：{response.Msg}）。");
            }

            // 页面输出的response.Body就是orderString可以直接给客户端请求，无需再做处理 。
            return Task.FromResult<IOrderReturn>(
                new AliPayWebCreateOrderReturn
                {
                    AppId = Config.AppId,
                    Body = $"https://openapi.alipay.com/gateway.do?{response.Body}",
                    CurrentPayNotifySign = BuildCurrentPayNotifySign(order),
                    OrderId = order.Id,
                    OrderNumber = order.OrderSerialNumber
                });
        }
    }
}

public record InstallmentConfig
{
    [JsonProperty("hb_fq_num")]
    public string HbFqNum { get; set; }

    [JsonProperty("hb_fq_seller_percent")]
    public string HbFqSellerPercent { get; set; }
}