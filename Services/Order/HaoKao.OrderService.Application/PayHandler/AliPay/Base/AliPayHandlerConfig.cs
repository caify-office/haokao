using HaoKao.OrderService.Application.PayHandler.Config;

namespace HaoKao.OrderService.Application.PayHandler.AliPay.Base;

public class AliPayHandlerConfig : ConfigHandler, IPayHandlerConfig
{
    /// <summary>
    /// APPID 相当于应用的身份证号，是支付宝分配给开发者的应用 ID，只有应用创建后才可以获取。
    /// </summary>
    [ConfigTip("应用Id"), ConfigType(ConfigType.String)]
    public string AppId { get; set; }

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

    /// <summary>
    /// 支付宝公钥，由支付宝生成
    /// </summary>
    [ConfigTip("支付宝公钥"), ConfigType(ConfigType.String)]
    public string PublicKey { get; set; }

    /// <summary>
    /// 开发者应用私钥，由开发者自己生成
    /// </summary>
    [ConfigTip("开发者应用私钥"), ConfigType(ConfigType.String)]
    public string PrivateKey { get; set; }

    /// <summary>
    /// 支付通知地址
    /// </summary>
    [ConfigTip("支付通知地址"), ConfigType(ConfigType.String)]
    public string NotifyUrl { get; set; }

    // /// <summary>
    // /// 花呗分期数，仅支持 3、6、12
    // /// </summary>
    // [ConfigTip("花呗分期数，仅支持 3、6、12"), ConfigType(ConfigType.Int)]
    // public string HbFqNum { get; set; }
    //
    // /// <summary>
    // /// 卖家承担收费比例，商家承担手续费填 100，用户承担手续费填 0，仅支持 100、0 两种
    // /// </summary>
    // [ConfigTip("卖家承担收费比例，商家承担手续费填 100，用户承担手续费填 0，仅支持填 100、0 两种"), ConfigType(ConfigType.Int)]
    // public string HbFqSellerPercent { get; set; }
}