using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.H5;

public class WechatH5PayCreateOrderReturn : IOrderReturn
{
    /// <summary>
    /// 预支付会话Id
    /// </summary>
    [JsonProperty("prepayid")]
    [JsonPropertyName("prepayid")]
    public string H5Url { get; set; }

    public string CurrentPayNotifySign { get; set; }

    public Guid OrderId { get; set; }

    public string OrderNumber { get; set; }
}