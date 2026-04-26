namespace HaoKao.OrderService.Application.PayHandler.WechatPay.JSAPI;

public class WechatJsapiPayCreateOrderReturn : IOrderReturn
{
    /// <summary>
    /// 小程序Id
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 获取微信应答时间戳。
    /// </summary>
    public string TimeStamp { get; set; }

    /// <summary>
    /// 获取微信应答随机串。
    /// </summary>
    public string NonceStr { get; set; }

    /// <summary>
    /// 订单详情扩展字符串
    /// </summary>
    public string Package { get; set; }

    /// <summary>
    /// 签名方式
    /// </summary>
    public string SignType { get; set; }

    /// <summary>
    /// 签名
    /// </summary>
    public string PaySign { get; set; }

    public string CurrentPayNotifySign { get; set; }

    public Guid OrderId { get; set; }

    public string OrderNumber { get; set; }
}