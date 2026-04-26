using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Queries;

public class OrderInvoiceQuery : QueryBase<OrderInvoice>
{
    /// <summary>
    /// 订单编号
    /// </summary>
    [QueryCacheKey]
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <summary>
    /// 下单人名称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }

    /// <summary>
    /// 抬头类型
    /// </summary>
    [QueryCacheKey]
    public InvoiceType? InvoiceType { get; set; }

    /// <summary>
    /// 获取方式
    /// </summary>
    [QueryCacheKey]
    public InvoiceFormat? InvoiceFormat { get; set; }

    /// <summary>
    /// 申请开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 申请结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 申请状态
    /// </summary>
    [QueryCacheKey]
    public RequestState? RequestState { get; set; }

    public override Expression<Func<OrderInvoice, bool>> GetQueryWhere()
    {
        Expression<Func<OrderInvoice, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(OrderSerialNumber))
        {
            expression = expression.And(x => x.Order.OrderSerialNumber == OrderSerialNumber);
        }
        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Order.Phone.Contains(Phone));
        }
        if (!string.IsNullOrEmpty(CreatorName))
        {
            expression = expression.And(x => x.Order.CreatorName.Contains(CreatorName));
        }
        if (InvoiceType.HasValue)
        {
            expression = expression.And(x => x.InvoiceType == InvoiceType);
        }
        if (InvoiceFormat.HasValue)
        {
            expression = expression.And(x => x.InvoiceFormat == InvoiceFormat);
        }
        if (StartTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartTime);
        }
        if (EndTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime <= EndTime);
        }
        if (RequestState.HasValue)
        {
            expression = expression.And(x => x.RequestState == RequestState);
        }
        return expression;
    }
}