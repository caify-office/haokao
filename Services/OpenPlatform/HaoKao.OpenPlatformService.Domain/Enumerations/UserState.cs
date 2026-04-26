using System.ComponentModel;

namespace HaoKao.OpenPlatformService.Domain.Enumerations;

/// <summary>
/// 用户状态
/// </summary>
[Description("用户状态")]
public enum UserState
{
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disable = 0,

    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable = 1,
}