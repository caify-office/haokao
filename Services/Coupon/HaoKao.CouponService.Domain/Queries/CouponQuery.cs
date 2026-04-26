using Girvs.Extensions;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Models;
using System;
using System.Linq;

namespace HaoKao.CouponService.Domain.Queries;

public class CouponQuery : QueryBase<Coupon>
{
    /// <summary>
    /// 优惠券code
    /// </summary>
    [QueryCacheKey]
    public string CouponCode { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    [QueryCacheKey]
    public string CouponName { get; set; }

    /// <summary>
    /// 下单用户名称
    /// </summary>
    [QueryCacheKey]
    public string OrderUserName { get; set; }

    /// <summary>
    /// 类型
    /// </summary>

    [QueryCacheKey]
    public CouponTypeEnum? CouponType { get; set; }

    /// <summary>
    /// 过滤id数组
    /// </summary>
    [QueryCacheKey]
    public string ExcludeIds { get; set; }

    /// <summary>
    /// 是否过滤实名优惠卷
    /// </summary>
    [QueryCacheKey]
    public bool? IsFilterSmCoupon { get; set; }

    /// <summary>
    /// 是否过期
    /// </summary>
    [QueryCacheKey]
    public bool? IsExpired { get; set; }

    public override Expression<Func<Coupon, bool>> GetQueryWhere()
    {
        Expression<Func<Coupon, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(CouponCode))
        {
            expression = expression.And(x => x.CouponCode.Contains(CouponCode));
        }
        if (!string.IsNullOrEmpty(CouponName))
        {
            expression = expression.And(x => x.CouponName.Contains(CouponName));
        }
        if (!string.IsNullOrEmpty(OrderUserName))
        {
            expression = expression.And(x => x.UserCoupons.Where(x => x.CreatorName.Contains(OrderUserName)) != null);
        }
        if (CouponType != null)
        {
            expression = expression.And(x => x.CouponType == CouponType);
        }
        if (IsFilterSmCoupon.HasValue && IsFilterSmCoupon.Value)
        {
            expression = expression.And(x => x.CouponType != CouponTypeEnum.SmCoupon);
        }
        if (!string.IsNullOrEmpty(ExcludeIds))
        {
            expression = expression.And(x => !ExcludeIds.Contains(x.Id.ToString()));
        }
        if (IsExpired.HasValue)
        {
            var now = DateTime.Now;
            if (IsExpired.Value)
            {
                expression = expression.And(x => x.EndDate < now);
            }
            else
            {
                //小时卷永远属于未过期卷
                expression = expression.And(x => x.EndDate >= now||x.TimeType==TimeTypeEnum.Hour);
            }
        }
        return expression;
    }
}