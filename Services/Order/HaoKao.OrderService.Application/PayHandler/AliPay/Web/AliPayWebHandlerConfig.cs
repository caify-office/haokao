using HaoKao.OrderService.Application.PayHandler.AliPay.Base;
using HaoKao.OrderService.Application.PayHandler.Config;

namespace HaoKao.OrderService.Application.PayHandler.AliPay.Web;

/// <summary>
/// 支付宝电脑网站支付配置
/// </summary>
public class AliPayWebHandlerConfig : AliPayHandlerConfig
{
    /// <summary>
    /// 支付成功返回地址
    /// </summary>
    [ConfigTip("支付成功返回地址"), ConfigType(ConfigType.String)]
    public string ReturnUrl { get; set; }
}

public class HbPayWebHandlerConfig : AliPayWebHandlerConfig
{
    /// <summary>
    /// 分期配置(json)
    /// </summary>
    [ConfigTip("分期配置(json)"), ConfigType(ConfigType.String)]
    public string InstallmentConfig { get; set; }
}