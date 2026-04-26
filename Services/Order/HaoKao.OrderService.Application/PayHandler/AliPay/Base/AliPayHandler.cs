namespace HaoKao.OrderService.Application.PayHandler.AliPay.Base;

public abstract class AliPayHandler<TConfig> : PayHandlerBase<TConfig> where TConfig : AliPayHandlerConfig, new()
{
    protected IAopClient Client =>
        new DefaultAopClient(new AlipayConfig
        {
            AppId = Config.AppId,
            AlipayPublicKey = Config.PublicKey,
            PrivateKey = Config.PrivateKey,
            Charset = "UTF-8",
            SignType = "RSA2",
            ServerUrl = "https://openapi.alipay.com/gateway.do", // 正式地址网关
            // ServerUrl = "https://openapi.alipaydev.com/gateway.do", // 沙盒测试网关
        });

    protected override string BuildNotifyUrl(Guid platformPayerId, Guid orderId)
    {
        return $"{Config.NotifyUrl.TrimEnd('/')}{base.BuildNotifyUrl(platformPayerId, orderId)}";
    }

    public override async Task<dynamic> ReceivePayNotifyMessage(Guid orderId)
    {
        // 获取支付宝 POST 过来通知消息，并以“参数名 = 参数值”的形式组成数组
        var form = EngineContext.Current.HttpContext.Request.Form;
        var dict = form.Keys.ToDictionary<string, string, string>(key => key, key => form[key]);

        // 切记 AliPayPublicKey 是支付宝的公钥，请去 open.alipay.com 对应应用下查看。
        var flag = AlipaySignature.RSACheckV2(dict, Config.PublicKey, "UTF-8", "RSA2", false);
        if (flag) // 验证签名
        {
            return new { code = "FAIL", message = "验签失败" };
        }

        // 交易状态
        var tradeStatus = dict["trade_status"];
        // 在支付宝的业务通知中，只有交易通知状态为TRADE_SUCCESS或TRADE_FINISHED时，才是买家付款成功。
        if (tradeStatus != "TRADE_SUCCESS")
        {
            return new { code = "FAIL", message = "付款失败" };
        }

        // 原支付请求的商家订单号
        var outTradeNo = dict["out_trade_no"];

        // 支付宝交易凭证号
        var tradeNo = dict["trade_no"];

        // 实际支付金额
        // var amount = dict["total_amount"];

        // 获取自定义数据
        var passbackParams = dict["passback_params"];
        var attachment = PayTokenHandler.ParseAttachment(passbackParams);
        EngineContext.Current.ClaimManager.SetFromDictionary(attachment);

        // 更新本地订单状态
        await PaySucceedUpdateLocalOrder(orderId, outTradeNo, tradeNo);

        return new { code = "SUCCESS", message = "成功" };
    }
}