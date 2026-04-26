namespace HaoKao.TenantService.Domain.Enums;

/// <summary>
/// 租户发布状态
/// </summary>
public enum ReleaseState
{
    [Description("未发布")]
    NotRelease,

    [Description("已发布")]
    Released
}