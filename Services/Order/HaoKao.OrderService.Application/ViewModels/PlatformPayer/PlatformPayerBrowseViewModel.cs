using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.PlatformPayer;

[AutoMapFrom(typeof(Domain.Entities.PlatformPayer))]
[AutoMapTo(typeof(Domain.Entities.PlatformPayer))]
public class BrowsePlatformPayerViewModel : IDto
{
    public Guid Id { get; set; }

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
}