using System.ComponentModel;

namespace HaoKao.Common.Enums;

/// <summary>
/// 有效期类型
/// </summary>
[Description("有效期类型")]
public enum ExpiryTimeTypeEnum
{
    /// <summary>
    /// 按日期
    /// </summary>
    [Description("按日期")]
    Date = 0,

    /// <summary>
    /// 按天数
    /// </summary>
    [Description("按天数")]
    Day = 1,
}