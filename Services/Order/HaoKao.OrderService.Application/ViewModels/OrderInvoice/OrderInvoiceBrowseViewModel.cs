using HaoKao.OrderService.Application.ViewModels.Order;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.ViewModels.OrderInvoice;

[AutoMapFrom(typeof(Domain.Entities.OrderInvoice))]
public class BrowseOrderInvoiceViewModel : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 订单Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// 发票类型
    /// </summary>
    public InvoiceType InvoiceType { get; set; }

    /// <summary>
    /// 增值税发票类型
    /// </summary>
    public VatInvoiceType VatInvoiceType { get; set; }

    /// <summary>
    /// 发票抬头
    /// </summary>
    public string InvoiceTitle { get; set; }

    /// <summary>
    /// 税号
    /// </summary>
    public string TaxNo { get; set; }

    /// <summary>
    /// 注册地址
    /// </summary>
    public string RegistryAddress { get; set; }

    /// <summary>
    /// 注册电话
    /// </summary>
    public string RegistryTel { get; set; }

    /// <summary>
    /// 开户银行
    /// </summary>
    public string BankName { get; set; }

    /// <summary>
    /// 银行账号
    /// </summary>
    public string BankAccount { get; set; }

    /// <summary>
    /// 发票形式(获取方式)
    /// </summary>
    public InvoiceFormat InvoiceFormat { get; set; }

    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 所在地区
    /// </summary>
    public string ShippingArea { get; set; }

    /// <summary>
    /// 收件人地址
    /// </summary>
    public string ShippingAddress { get; set; }

    /// <summary>
    /// 收件人姓名
    /// </summary>
    public string ReceiverName { get; set; }

    /// <summary>
    /// 收件人联系电话
    /// </summary>
    public string ReceiverPhone { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    public RequestState RequestState { get; set; }

    /// <summary>
    /// 物流单号
    /// </summary>
    public string ShippingNumber { get; set; }

    /// <summary>
    /// 物流公司
    /// </summary>
    public string LogisticsCompany { get; set; }

    /// <summary>
    /// 发货时间
    /// </summary>
    public DateTime? ShippingTime { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 发票备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 订单
    /// </summary>
    public BrowseOrderViewModel Order { get; set; }
}