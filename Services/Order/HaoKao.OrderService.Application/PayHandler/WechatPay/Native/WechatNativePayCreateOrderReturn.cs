using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.Native;

public class WechatNativePayCreateOrderReturn : IOrderReturn
{
    /// <summary>
    /// 二维码链接
    /// </summary>
    [JsonProperty("qrcodeUrl")]
    [JsonPropertyName("qrcodeUrl")]
    public string QrcodeUrl { get; set; }

    public string CurrentPayNotifySign { get; set; }

    public Guid OrderId { get; set; }

    public string OrderNumber { get; set; }
}