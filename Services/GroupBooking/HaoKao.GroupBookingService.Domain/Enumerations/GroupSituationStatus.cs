
using System.ComponentModel;

namespace HaoKao.GroupBookingService.Domain.Enumerations;

/// <summary>
/// 拼团状态
/// </summary>
[Description("拼团状态")]
public enum GroupSituationStatus
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    UnKnow = 0,
    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 1,

    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Faild = 2,

    /// <summary>
    /// 拼团中
    /// </summary>
    [Description("拼团中")]
    InGroup = 3,
}