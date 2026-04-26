using System.ComponentModel;

namespace HaoKao.CouponService.Domain.Enumerations;

/// <summary>
/// 有效期类型
/// </summary>
[Description("有效期类型")]
public enum TimeTypeEnum
{
    /// <summary>
    /// 按日期
    /// </summary>
    [Description("按日期")]
    Date = 1,

    /// <summary>
    /// 按小时
    /// </summary>
    [Description("按小时")]
    Hour = 2,
}