using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Domain.Queries;

/// <inheritdoc />
public class OrderQuery : QueryBase<Order>
{
    /// <summary>
    /// 使用的平台配置的支付者的Id
    /// </summary>
    [QueryCacheKey]
    public Guid? PlatformPayerId { get; set; }

    /// <summary>
    /// 订单号(模糊搜索)
    /// </summary>
    [QueryCacheKey]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 订单号(精确搜索)
    /// </summary>
    [QueryCacheKey]
    public string OrderNumber2 { get; set; }

    /// <summary>
    /// 订单流水号(模糊搜索)
    /// </summary>
    [QueryCacheKey]
    public string OrderSerialNumber { get; set; }

    /// <summary>
    /// 订单流水号(精确搜索)
    /// </summary>
    [QueryCacheKey]
    public string OrderSerialNumber2 { get; set; }

    /// <summary>
    /// 购买产品名称
    /// </summary>
    [QueryCacheKey]
    public string PurchaseName { get; set; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    [QueryCacheKey]
    public string CreatorName { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    [QueryCacheKey]
    public OrderState? OrderState { get; set; }

    /// <summary>
    /// 下单开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartCreateTime { get; set; }

    /// <summary>
    /// 下单结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndCreateTime { get; set; }

    /// <summary>
    /// 支付开始时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? StartUpdateTime { get; set; }

    /// <summary>
    /// 支付结束时间
    /// </summary>
    [QueryCacheKey]
    public DateTime? EndUpdateTime { get; set; }

    [QueryCacheKey]
    public Guid? CreatorId { get; set; }

    /// <summary>
    /// 直播id（可为空）
    /// </summary>
    [QueryCacheKey]
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 苹果内购恢复购买的
    /// </summary>
    [QueryCacheKey]
    public bool? IosRestorePurchase { get; set; }

    /// <summary>
    /// 是否付费订单（true:返回实缴金额大于0订单，false：返回实缴金额等于0订单，不传都返回）
    /// </summary>
    [QueryCacheKey]
    public bool? IsPaidOrder { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [QueryCacheKey]
    public string Phone { get; set; }

    /// <inheritdoc />
    public override Expression<Func<Order, bool>> GetQueryWhere()
    {
        Expression<Func<Order, bool>> expression = x => true;

        if (IosRestorePurchase.HasValue)
        {
            expression = expression.And(x => x.IosRestorePurchase == IosRestorePurchase);
        }

        if (CreatorId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == CreatorId);
        }
        else
        {
            expression = expression.And(x => x.OrderState != Enums.OrderState.Cancel)
                                   .And(x => x.OrderState != Enums.OrderState.Expired);
        }

        if (PlatformPayerId.HasValue)
        {
            expression = expression.And(x => x.PlatformPayerId == PlatformPayerId);
        }

        if (!OrderSerialNumber.IsNullOrEmpty())
        {
            expression = expression.And(x => x.OrderSerialNumber.Contains(OrderSerialNumber));
        }

        if (!OrderSerialNumber2.IsNullOrEmpty())
        {
            expression = expression.And(x => x.OrderSerialNumber == OrderSerialNumber2);
        }

        if (!OrderNumber.IsNullOrEmpty())
        {
            expression = expression.And(x => x.OrderNumber.Contains(OrderNumber));
        }

        if (!OrderNumber2.IsNullOrEmpty())
        {
            expression = expression.And(x => x.OrderNumber == OrderNumber2);
        }

        if (!PurchaseName.IsNullOrEmpty())
        {
            expression = expression.And(x => x.PurchaseName.Contains(PurchaseName));
        }

        if (!CreatorName.IsNullOrEmpty())
        {
            expression = expression.And(x => x.CreatorName.Contains(CreatorName));
        }

        if (StartCreateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime >= StartCreateTime);
        }

        if (EndCreateTime.HasValue)
        {
            expression = expression.And(x => x.CreateTime < EndCreateTime.Value.AddDays(1));
        }

        if (StartUpdateTime.HasValue)
        {
            expression = expression.And(x => x.UpdateTime >= StartUpdateTime);
        }

        if (EndUpdateTime.HasValue)
        {
            expression = expression.And(x => x.UpdateTime < EndUpdateTime.Value.AddDays(1));
        }

        if (OrderState.HasValue)
        {
            expression = expression.And(x => x.OrderState == OrderState);
        }

        if (LiveId.HasValue)
        {
            expression = expression.And(x => x.LiveId == LiveId);
        }

        if (IsPaidOrder.HasValue)
        {
            if (IsPaidOrder.Value)
            {
                expression = expression.And(x => x.ActualAmount > 0);
            }
            else
            {

                expression = expression.And(x => x.ActualAmount == 0);
            }
        }

        if (!string.IsNullOrEmpty(Phone))
        {
            expression = expression.And(x => x.Phone.Contains(Phone));
        }

        return expression;
    }
}