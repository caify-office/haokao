using HaoKao.OrderService.Application.PayHandler.Config;
using HaoKao.OrderService.Application.PayHandler.WechatPay.Base;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.JSAPI;

/// <summary>
/// 微信 JSAPI 调起支付配置
/// </summary>
public class WechatJsapiPayHandlerConfig : WechatPayHandlerConfig
{
    /// <summary>
    /// 微信小程序密钥
    /// </summary>
    [ConfigTip("微信小程序密钥"), ConfigType(ConfigType.String)]
    public string AppSecret { get; set; }
}