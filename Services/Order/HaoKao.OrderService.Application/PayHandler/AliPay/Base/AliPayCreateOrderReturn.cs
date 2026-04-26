using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Application.PayHandler.AliPay.Base;

public class AliPayCreateOrderReturn : IOrderReturn
{
    public string CurrentPayNotifySign { get; set; }

    /// <summary>
    /// 订单Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 应用ID
    /// </summary>
    [JsonProperty("appid")]
    [JsonPropertyName("appid")]
    public string AppId { get; set; }

    /// <summary>
    /// body
    /// </summary>
    [JsonProperty("body")]
    [JsonPropertyName("body")]
    public string Body { get; set; }

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
}