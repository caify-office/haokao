using Girvs.Extensions;
using HaoKao.CouponService.Domain.Models;
using System;

namespace HaoKao.CouponService.Domain.Queries;

public class UserCouponQuery : QueryBase<UserCoupon>
{
    [QueryCacheKey]
    public string NickName { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [QueryCacheKey]
    public string StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [QueryCacheKey]
    public string EndTime { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    [QueryCacheKey]
    public Guid? UserId { get; set; }

    /// <summary>
    /// 优惠券id
    /// </summary>
    [QueryCacheKey]
    public Guid? CouponId { get; set; }

    /// <summary>
    /// 是否使用
    /// </summary>
    [QueryCacheKey]
    public bool? IsUse { get; set; }


    /// <summary>
    /// 是否锁定
    /// </summary>
    [QueryCacheKey]
    public bool? IsLock { get; set; }

    /// <summary>
    /// 是否需要过滤过期优惠卷
    /// </summary>
    [QueryCacheKey]
    public bool? IsFilterExpired { get; set; }

    public override Expression<Func<UserCoupon, bool>> GetQueryWhere()
    {
        Expression<Func<UserCoupon, bool>> expression = x => true;
        if (IsUse.HasValue)
        {
            expression = expression.And(x => x.IsUse == IsUse);
        }

        if (IsLock.HasValue)
        {
            expression = expression.And(x => x.IsLock == IsLock);
        }
        if (IsFilterExpired.HasValue && IsFilterExpired.Value)
        {
            //过滤到已过期用户优惠券(前端搜索未使用优惠卷才需要过滤已过期的优惠券)
            var now = DateTime.Now;
            expression = expression.And(x => x.EndTime > now);
        }
        if (CouponId.HasValue)
        {
            expression = expression.And(x => x.CouponId == CouponId);
        }
        if (UserId.HasValue)
        {
            expression = expression.And(x => x.CreatorId == UserId);
        }
        if (!string.IsNullOrEmpty(StartTime))
        {
            expression = expression.And(x => x.CreateTime >= DateTime.Parse(StartTime));
        }
        if (!string.IsNullOrEmpty(EndTime))
        {
            expression = expression.And(x => x.CreateTime <= DateTime.Parse(EndTime));
        }
        if (!string.IsNullOrEmpty(NickName))
        {
            expression = expression.And(x => x.NickName.Contains(NickName));
        }
        return expression;
    }
}