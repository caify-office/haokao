namespace HaoKao.OrderService.Application.PayHandler.Config;

public class AppleInAppPurchaseHanlderConfig : ConfigHandler, IPayHandlerConfig
{
    /// <summary>
    /// 支付方式图标
    /// </summary>
    [ConfigTip("图标"), ConfigType(ConfigType.FilePath)]
    public string IconUrl { get; set; }
    
    /// <summary>
    /// 订单号前缀
    /// </summary>
    [ConfigTip("订单号前缀"), ConfigType(ConfigType.String)]
    public string OrderPrefix { get; set; }
}