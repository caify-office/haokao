using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Queries;

namespace HaoKao.OrderService.Application.ViewModels.OrderInvoice;

[AutoMapFrom(typeof(OrderInvoiceQuery))]
[AutoMapTo(typeof(OrderInvoiceQuery))]
public class OrderInvoiceQueryViewModel : QueryDtoBase<BrowseOrderInvoiceViewModel>
{
    /// <summary>
    /// 订单编号
    /// </summary>
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 下单人名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 抬头类型
    /// </summary>
    public InvoiceType? InvoiceType { get; set; }

    /// <summary>
    /// 获取方式
    /// </summary>
    public InvoiceFormat? InvoiceFormat { get; set; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    public RequestState? RequestState { get; set; }
}