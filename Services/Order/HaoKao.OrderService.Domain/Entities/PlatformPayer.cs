using HaoKao.OrderService.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HaoKao.OrderService.Domain.Entities;

/// <summary>
/// 平台配置的支付列表
/// </summary>
public class PlatformPayer : AggregateRoot<Guid>,
                             IIncludeCreateTime,
                             IIncludeUpdateTime,
                             IIncludeCreatorId<Guid>,
                             IIncludeCreatorName
{
    /// <summary>
    /// 支付名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 对应支付处理者Id
    /// </summary>
    public Guid PayerId { get; set; }

    /// <summary>
    /// 对应支付处理者名称
    /// </summary>
    public string PayerName { get; set; }

    /// <summary>
    /// 使用场景
    /// </summary>
    public PlatformPayerScenes PlatformPayerScenes { get; set; }

    /// <summary>
    /// 支付方式归类
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Ios是否开启
    /// </summary>
    public bool IosIsOpen { get; set; }

    /// <summary>
    /// 支付相关配置
    /// </summary>
    public Dictionary<string, string> Config { get; set; }

    /// <summary>
    /// 创建的时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 数据安全码
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string SecurityCode { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool UseState { get; set; }

    /// <summary>
    /// 生成数据安全码
    /// </summary>
    /// <returns></returns>
    public string BuildSecurityCode()
    {
        if (Config == null) return string.Empty;

        var thisJson = JsonSerializer.Serialize(Config);
        return thisJson.ToMd5();
    }

    /// <summary>
    /// 校验数据的合法性，有没有通过数据直接对数据进行修改
    /// </summary>
    /// <returns></returns>
    public bool VerificationDataSecurity()
    {
        var code = BuildSecurityCode();
        return code == SecurityCode;
    }
}