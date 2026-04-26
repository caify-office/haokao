using System.ComponentModel;

namespace HaoKao.LiveBroadcastService.Domain.Enums;

/// <summary>
/// 优惠券类型
/// </summary>
[Description("优惠券类型")]
public enum CouponTypeEnum
{
    /// <summary>
    /// 抵用券
    /// </summary>
    [Description("抵用券")]
    DeductionVoucher = 1,

    /// <summary>
    /// 折扣券
    /// </summary>
    [Description("折扣券")]
    DisCount = 2,

    /// <summary>
    /// 实名券
    /// </summary>
    [Description("实名券")]
    SmCoupon = 3,
}