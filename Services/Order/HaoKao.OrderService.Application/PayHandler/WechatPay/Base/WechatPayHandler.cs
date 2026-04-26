using HaoKao.OrderService.Application.PayHandler.WechatPay.App;
using Microsoft.Extensions.Logging;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Events;
using SKIT.FlurlHttpClient.Wechat.TenpayV3.Settings;
using System.Collections.Concurrent;
using System.Text;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.Base;

public abstract class WechatPayHandler<TConfig> : PayHandlerBase<TConfig> where TConfig : WechatPayHandlerConfig, new()
{
    private static ConcurrentDictionary<string, CertificateManager> CertificateManagerDict => new();

    private static ConcurrentDictionary<string, WechatTenpayClient> Clients => new();

    /// <summary>
    /// 获取客户端
    /// </summary>
    /// <returns></returns>
    protected WechatTenpayClient WechatTenpayClient => Clients.GetOrAdd(Config.MerchantId, CreateClient());

    private CertificateManager GetCertificateManager()
    {
        return CertificateManagerDict.GetOrAdd(Config.MerchantId, new InMemoryCertificateManager());
    }

    /// <summary>
    /// 创建客户端
    /// </summary>
    /// <returns></returns>
    private WechatTenpayClient CreateClient()
    {
        //设置客户端相关的参数
        var options = new WechatTenpayClientOptions
        {
            MerchantId = Config.MerchantId,
            MerchantV3Secret = Config.SecretV3,
            MerchantCertificateSerialNumber = Config.CertificateSerialNumber,
            MerchantCertificatePrivateKey = Config.CertificatePrivateKey,
            PlatformCertificateManager = GetCertificateManager(),
            AutoEncryptRequestSensitiveProperty = false,
            AutoDecryptResponseSensitiveProperty = false
        };

        //创建客户端
        var client = new WechatTenpayClient(options);
        const string algorithmType = "RSA";

        //获取服务器的证书
        var request = new QueryCertificatesRequest { AlgorithmType = algorithmType };

        var response = client.ExecuteQueryCertificatesAsync(request).Result;
        if (response.IsSuccessful())
        {
            // NOTICE:
            //   如果构造 Client 时启用了 `AutoDecryptResponseSensitiveProperty` 配置项，则无需再执行下面一行的手动解密方法：
            response = client.DecryptResponseSensitiveProperty(response);

            foreach (var certificate in response.CertificateList)
            {
                client.PlatformCertificateManager.AddEntry(new CertificateEntry(algorithmType, certificate));
            }
        }

        return client;
    }

    protected override string BuildNotifyUrl(Guid platformPayerId, Guid orderId)
    {
        return $"{Config.NotifyUrl.TrimEnd('/')}{base.BuildNotifyUrl(platformPayerId, orderId)}";
    }

    public override async Task<dynamic> ReceivePayNotifyMessage(Guid orderId)
    {
        var logger = EngineContext.Current.Resolve<ILogger<WechatAppCallPayHandler>>();

        logger.LogInformation("************************微信APP拉起支付，回调开始************************");
        var request = EngineContext.Current.HttpContext.Request;

        string timestamp = request.Headers["Wechatpay-Timestamp"];
        string nonce = request.Headers["Wechatpay-Nonce"];
        string signature = request.Headers["Wechatpay-Signature"];
        string serialNumber = request.Headers["Wechatpay-Serial"];

        logger.LogInformation("*******获取到的参数有：timestamp-{Timestamp};nonce-{Nonce};signature-{Signature};serialNumber-{SerialNumber}", timestamp, nonce, signature, serialNumber);
        if (timestamp.IsNullOrEmpty()
         || nonce.IsNullOrEmpty()
         || signature.IsNullOrEmpty()
         || serialNumber.IsNullOrEmpty()
           )
        {
            return new { code = "FAIL", message = "参数不正确" };
        }

        var content = await GetBodyAsync(request);
        logger.LogInformation("接收到微信支付推送的数据：{Content}", content);

        logger.LogInformation("*******获取到的Body参数为：{Content}", content);
        var valid = WechatTenpayClient.VerifyEventSignature(
            callbackTimestamp: timestamp,
            callbackNonce: nonce,
            callbackBody: content,
            callbackSignature: signature,
            callbackSerialNumber: serialNumber
        );

        logger.LogInformation("*******验证签名结果为：{Valid}", valid);
        if (!valid)
        {
            // NOTICE:
            //   需提前注入 CertificateManager、并下载平台证书，才可以使用扩展方法执行验签操作。
            //   请参考本示例项目 TenpayCertificateRefreshingBackgroundService 后台任务中的相关实现。
            //   有关 CertificateManager 的完整介绍请参阅《开发文档 / 基础用法 / 如何验证回调通知事件签名？》。
            //   后续如何解密并反序列化，请参阅《开发文档 / 基础用法 / 如何解密回调通知事件中的敏感数据？》。
            return new { code = "FAIL", message = "验签失败" };
        }

        var callbackModel = WechatTenpayClient.DeserializeEvent(content);
        var eventType = callbackModel.EventType.ToUpper();
        switch (eventType)
        {
            case "TRANSACTION.SUCCESS":
            {
                var callbackResource = WechatTenpayClient.DecryptEventResource<TransactionResource>(callbackModel);

                logger.LogInformation("*******获取到Attachment的值为：{CallbackResourceAttachment}", callbackResource.Attachment);
                var attachment = PayTokenHandler.ParseAttachment(callbackResource.Attachment);

                //从返回的数据中得到租户ID和当前用户Id
                EngineContext.Current.ClaimManager.SetFromDictionary(attachment);

                logger.LogInformation("*******获取到OutTradeNumber的值为：{CallbackResourceOutTradeNumber}", callbackResource.OutTradeNumber);

                // 得到订单流水号
                var outTradeNumber = callbackResource.OutTradeNumber;

                // 得到微信支付订单号
                var transcationId = callbackResource.TransactionId;

                //执行回调，更新当前系统的订单状态
                logger.LogInformation("*******执行回调，更新当前系统的订单状态");

                await PaySucceedUpdateLocalOrder(orderId, outTradeNumber, transcationId);

                //_logger.LogInformation("接收到微信支付推送的订单支付成功通知，商户订单号：{0}", callbackResource.OutTradeNumber);
            }
                break;

            default:
            {
                // 其他情况略
            }
                break;
        }

        return new { code = "SUCCESS", message = "成功" };
    }

    private static async Task<string> GetBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await streamReader.ReadToEndAsync();

        // Reset the Body stream position to 0, so it's available for the next middleware in the pipeline
        request.Body.Position = 0;

        return body;
    }
}