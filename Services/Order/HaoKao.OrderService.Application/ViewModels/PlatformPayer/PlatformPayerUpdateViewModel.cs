using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.PlatformPayer;

[AutoMapTo(typeof(Domain.Entities.PlatformPayer))]
public class UpdatePlatformPayerViewModel : IDto
{
    /// <summary>
    /// 支付名称
    /// </summary>
    [DisplayName("支付名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string Name { get; set; }

    /// <summary>
    /// 对应支付处理者Id
    /// </summary>
    [DisplayName("对应支付处理者Id")]
    public Guid PayerId { get; set; }

    /// <summary>
    /// 对应支付处理者名称
    /// </summary>
    [DisplayName("对应支付处理者名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string PayerName { get; set; }

    /// <summary>
    /// 使用场景
    /// </summary>
    [DisplayName("使用场景")]
    [Required(ErrorMessage = "{0}不能为空")]
    public PlatformPayerScenes PlatformPayerScenes { get; set; }

    /// <summary>
    /// 支付方式归类
    /// </summary>
    [DisplayName("支付方式归类")]
    [Required(ErrorMessage = "{0}不能为空")]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Ios是否开启
    /// </summary>
    [DisplayName("Ios是否开启")]
    [Required(ErrorMessage = "{0}不能为空")]
    public bool IosIsOpen { get; set; }

    /// <summary>
    /// 支付相关配置
    /// </summary>
    [DisplayName("支付相关配置")]
    public Dictionary<string, string> Config { get; set; }
}