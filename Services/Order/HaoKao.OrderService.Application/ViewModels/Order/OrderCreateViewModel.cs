using HaoKao.OrderService.Application.PayHandler;
using HaoKao.OrderService.Domain.Commands.Order;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.Order;

[AutoMapTo(typeof(CreateOrderCommand))]
public class CreateOrderViewModel : IDto
{
    /// <summary>
    /// 签名
    /// </summary>
    public string Sign { get; set; }
    /// <summary>
    /// 订单流水号
    /// </summary>
    [DisplayName("订单流水号")]
    public string OrderSerialNumber { get; } = OrderNumberGenerator.Generate("");

    /// <summary>
    /// 购买的产品包Id
    /// </summary>
    [DisplayName("购买的产品包Id")]
    public Guid PurchaseProductId { get; set; }

    /// <summary>
    /// 购买产品包名称
    /// </summary>
    [DisplayName("购买产品包名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    [MinLength(2, ErrorMessage = "{0}长度不能小于{1}")]
    [MaxLength(50, ErrorMessage = "{0}长度不能大于{1}")]
    public string PurchaseName { get; set; }

    /// <summary>
    /// 产品包类型
    /// </summary>
    [DisplayName("产品包类型")]
    public PurchaseProductType PurchaseProductType { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    [DisplayName("订单金额")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal OrderAmount { get; set; }

    /// <summary>
    /// 实际金额
    /// </summary>
    [DisplayName("实际金额")]
    [Required(ErrorMessage = "{0}不能为空")]
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [DisplayName("手机号")]
    public string Phone { get; set; }

    /// <summary>
    /// 直播id（可为空）
    /// </summary>
    [DisplayName("直播id（可为空）")]
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 购买产品内容
    /// </summary>
    [DisplayName("购买产品内容")]
    [Required(ErrorMessage = "{0}不能为空")]
    public List<PurchaseProductContent> PurchaseProductContents { get; set; } = [];

    /// <summary>
    /// 下单用户Id
    /// </summary>
    [DisplayName("下单用户Id")]
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 下单用户
    /// </summary>
    [DisplayName("下单用户")]
    public string CreatorName { get; set; }

    /// <summary>
    /// 下单时间
    /// </summary>
    [DisplayName("下单时间")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    [DisplayName("支付时间")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 客户端Id
    /// </summary>
    [DisplayName("客户端Id")]
    public string ClientId { get; set; }

    /// <summary>
    /// 客户端名称
    /// </summary>
    [DisplayName("客户端名称")]
    public string ClientName { get; set; }
}

[AutoMapTo(typeof(Domain.Entities.Order))]
public class ManualCreateOrderViewModel : CreateOrderViewModel;