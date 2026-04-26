namespace HaoKao.TenantService.Domain.Enums;

/// <summary>
/// 租户启用状态
/// </summary>
public enum EnableState
{
    [Description("已启用")]
    Enable,

    [Description("禁用")]
    NotEnabled
}