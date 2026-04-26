using System.ComponentModel;

namespace HaoKao.OpenPlatformService.Domain.Enumerations;

/// <summary>
/// 用户性别
/// </summary>
[Description("用户性别")]
public enum UserGender
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [Description("男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Description("女")]
    Female = 2,
}