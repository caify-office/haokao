namespace HaoKao.DataStatisticsService.WebApi.Enums;

/// <summary>
/// 权限状态
/// </summary>
public enum PermissionExpiryType
{
    /// <summary>
    /// 未过期
    /// </summary>
    NotExpired = 1,

    /// <summary>
    /// 已过期
    /// </summary>
    Expired = 2,

    /// <summary>
    /// 全部
    /// </summary>
    All = 3,
}