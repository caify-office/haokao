using HaoKao.CouponService.Domain.Models;
using LogDashboard.Extensions;
using System;

namespace HaoKao.CouponService.Domain.Queries;

public class UserCouponPerformanceQuery : QueryBase<UserCouponPerformance>
{
    /// <summary>
    /// 订单编号
    /// </summary>
    [QueryCacheKey]
    public string OrderNo { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [QueryCacheKey]
    public string ProductName { get; set; }

    /// <summary>
    /// 销售人员名称
    /// </summary>
    [QueryCacheKey]
    public string PersonName { get; set; }

    /// <summary>
    /// 支付开始时间
    /// </summary>
    [QueryCacheKey]
    public string StartTime { get; set; }

    /// <summary>
    /// 支付结束时间
    /// </summary>
    [QueryCacheKey]
    public string EndTime { get; set; }

    /// <summary>
    /// 金额
    /// </summary>
    [QueryCacheKey]
    public decimal? Amount { get; set; }

    /// <summary>
    ///实际支付金额--冗余
    /// </summary>
    [QueryCacheKey]
    public decimal? FactAmount { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    [QueryCacheKey]
    public string Resource { get; set; }

    public override Expression<Func<UserCouponPerformance, bool>> GetQueryWhere()
    {
        Expression<Func<UserCouponPerformance, bool>> expression = x => true;
        if (!string.IsNullOrEmpty(ProductName))
        {
            expression = expression.And(x => x.ProductName.Contains(ProductName));
        }
        if (!string.IsNullOrEmpty(OrderNo))
        {
            expression = expression.And(x => x.OrderNo.Contains(OrderNo));
        }
        if (!string.IsNullOrEmpty(PersonName))
        {
            expression = expression.And(x => x.PersonName.Contains(PersonName));
        }
        if (!string.IsNullOrEmpty(StartTime))
        {
            expression = expression.And(x => x.PayTime >= DateTime.Parse(StartTime));
        }
        if (!string.IsNullOrEmpty(EndTime))
        {
            expression = expression.And(x => x.PayTime <= DateTime.Parse(EndTime));
        }
        if (!string.IsNullOrEmpty(Resource))
        {
            expression = expression.And(x => x.Remark.Contains(Resource.Trim()));
        }
        if (Amount != null)
        {
            expression = expression.And(x => x.Amount == Amount);
        }
        if (FactAmount.HasValue)
        {
            expression = expression.And(x => x.FactAmount == FactAmount);
        }
        return expression;
    }
}