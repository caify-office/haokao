using SKIT.FlurlHttpClient.Wechat.TenpayV3.Utilities;
using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.App;

public class WechatAppCallPayCreateOrderReturn : IOrderReturn
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }

    /// <summary>
    /// 商户ID
    /// </summary>
    [JsonProperty("partnerid")]
    [JsonPropertyName("partnerid")]
    public string PartnerId { get; set; }

    /// <summary>
    /// 预支付会话Id
    /// </summary>
    [JsonProperty("prepayid")]
    [JsonPropertyName("prepayid")]
    public string PrepayId { get; set; }

    /// <summary>
    /// 订单详情扩展字符串
    /// </summary>
    [JsonProperty("package")]
    [JsonPropertyName("package")]
    public string Package { get; set; } = "Sign=WXPay";

    /// <summary>
    /// 随机字符串
    /// </summary>
    [JsonProperty("noncestr")]
    [JsonPropertyName("noncestr")]
    public string NonceStr { get; set; } = Guid.NewGuid().ToString().ToMd5();

    /// <summary>
    /// 时间戳
    /// </summary>
    [JsonProperty("timestamp")]
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

    /// <summary>
    /// 签名
    /// </summary>
    [JsonProperty("sign")]
    [JsonPropertyName("sign")]
    public string Sign { get; set; }

    public string GenerateSignature(string privateKey)
    {
        var message = GenerateMessageForSignature();
        Sign = RSAUtility.SignWithSHA256(privateKey, message);
        return Sign;
    }

    private string GenerateMessageForSignature()
    {
        return $"{AppId}\n{Timestamp}\n{NonceStr}\n{PrepayId}\n";
    }

    public string CurrentPayNotifySign { get; set; }

    public Guid OrderId { get; set; }

    public string OrderNumber { get; set; }
}