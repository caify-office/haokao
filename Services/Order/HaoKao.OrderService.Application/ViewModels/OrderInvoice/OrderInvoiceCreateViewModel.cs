using HaoKao.OrderService.Domain.Commands.OrderSqInvoice;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.OrderInvoice;

[AutoMapTo(typeof(CreateOrderInvoiceCommand))]
public class CreateOrderInvoiceViewModel : IDto
{
    /// <summary>
    /// 订单Id
    /// </summary>
    [DisplayName("订单Id")]
    [Required(ErrorMessage = "{0}不能为空")]
    public Guid OrderId { get; set; }

    /// <summary>
    /// 发票类型
    /// </summary>
    [DisplayName("发票类型")]
    public InvoiceType InvoiceType { get; set; }

    /// <summary>
    /// 增值税发票类型
    /// </summary>
    [DisplayName("增值税发票类型")]
    public VatInvoiceType VatInvoiceType { get; set; }

    /// <summary>
    /// 发票抬头
    /// </summary>
    [DisplayName("发票抬头")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string InvoiceTitle { get; set; }

    /// <summary>
    /// 税号
    /// </summary>
    [DisplayName("税号")]
    public string TaxNo { get; set; }

    /// <summary>
    /// 注册地址
    /// </summary>
    [DisplayName("注册地址")]
    public string RegistryAddress { get; set; }

    /// <summary>
    /// 注册电话
    /// </summary>
    [DisplayName("注册电话")]
    public string RegistryTel { get; set; }

    /// <summary>
    /// 开户银行
    /// </summary>
    [DisplayName("开户银行")]
    public string BankName { get; set; }

    /// <summary>
    /// 银行账号
    /// </summary>
    [DisplayName("银行账号")]
    public string BankAccount { get; set; }

    /// <summary>
    /// 发票形式(获取方式)
    /// </summary>
    [DisplayName("获取方式)")]
    public InvoiceFormat InvoiceFormat { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    [DisplayName("电子邮箱")]
    public string Email { get; set; }

    /// <summary>
    /// 所在地区
    /// </summary>
    [DisplayName("所在地区")]
    public string ShippingArea { get; set; }

    /// <summary>
    /// 收件人地址
    /// </summary>
    [DisplayName("收件人地址")]
    public string ShippingAddress { get; set; }

    /// <summary>
    /// 收件人姓名
    /// </summary>
    [DisplayName("收件人姓名")]
    public string ReceiverName { get; set; }

    /// <summary>
    /// 收件人联系电话
    /// </summary>
    [DisplayName("收件人联系电话")]
    public string ReceiverPhone { get; set; }

    /// <summary>
    /// 发票备注
    /// </summary>
    [DisplayName("发票备注")]
    public string Remark { get; set; }
}