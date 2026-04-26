using HaoKao.OrderService.Application.PayHandler.Config;

namespace HaoKao.OrderService.Application.PayHandler.WechatPay.Base;

public class WechatPayHandlerConfig : ConfigHandler, IPayHandlerConfig
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

    /// <summary>
    /// 微信开放平台审核通过的移动应用appid string[1,32]
    /// </summary>
    [ConfigTip("移动应用appid"), ConfigType(ConfigType.String)]
    public string AppId { get; set; }

    /// <summary>
    /// 请填写商户号mchid对应的值 string[1,32]
    /// </summary>
    [ConfigTip("商户号"), ConfigType(ConfigType.String)]
    public string MerchantId { get; set; }

    /// <summary>
    /// Api V3 密钥
    /// </summary>
    [ConfigTip("Api V3 密钥"), ConfigType(ConfigType.String)]
    public string SecretV3 { get; set; }

    /// <summary>
    /// 支付通知地址
    /// </summary>
    [ConfigTip("支付通知地址"), ConfigType(ConfigType.String)]
    public string NotifyUrl { get; set; }

    /// <summary>
    /// 证书编码
    /// </summary>
    [ConfigTip("证书编码"), ConfigType(ConfigType.String)]
    public string CertificateSerialNumber { get; set; }

    /// <summary>
    /// 证书私钥
    /// </summary>
    [ConfigTip("证书私钥"), ConfigType(ConfigType.String)]
    public string CertificatePrivateKey { get; set; }
}