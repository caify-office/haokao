using HaoKao.CouponService.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace HaoKao.CouponService.Domain.Models;

public class Coupon : AggregateRoot<Guid>,
                      IIncludeCreateTime,
                      IIncludeMultiTenant<Guid>,
                      ITenantShardingTable,
                      IIncludeCreatorId<Guid>
{
    public Coupon()
    {
        UserCoupons = [];
    }

    /// <summary>
    /// 优惠券卡号
    /// </summary>
    public string CouponCode { get; set; }

    /// <summary>
    /// 优惠券名称
    /// </summary>
    public string CouponName { get; set; }

    /// <summary>
    /// 优惠券说明
    /// </summary>
    public string CouponDesc { get; set; }

    /// <summary>
    /// 优惠券类型 1-抵用券 2-折扣券
    /// </summary>
    public CouponTypeEnum CouponType { get; set; }

    /// <summary>
    /// 适用范围
    /// </summary>
    public ScopeEnum Scope { get; set; }

    /// <summary>
    /// 适用产品集合
    /// </summary>
    public List<Guid> ProductIds { get; set; }

    /// <summary>
    /// 产品包id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// 金额/折扣--合并一个字段  折扣85折显示0.85
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 门槛金额
    /// </summary>
    public decimal ThresholdAmount { get; set; }

    /// <summary>
    /// 是否实名优惠券 true-实名优惠券 false--非实名优惠券即活动优惠券
    /// </summary>
    public bool IsOnlyName { get; set; }

    /// <summary>
    /// 助教实名名称
    /// </summary>
    public string PersonName { get; set; }

    /// <summary>
    /// 营销助教userid
    /// </summary>
    public Guid PersonUserId { get; set; }

    /// <summary>
    /// 1-按日期 2-按小时
    /// </summary>
    public TimeTypeEnum TimeType { get; set; }

    /// <summary>
    /// 按小时
    /// </summary>
    public int Hour { get; set; }

    /// <summary>
    /// 有效期-开始时间
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    ///  有效期-结束时间
    /// </summary>
    public DateTime BeginDate { get; set; }

    /// <summary>
    /// 对应的优惠券使用记录
    /// </summary>
    public virtual List<UserCoupon> UserCoupons { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建userid
    /// </summary>
    public Guid CreatorId { get; set; }
}