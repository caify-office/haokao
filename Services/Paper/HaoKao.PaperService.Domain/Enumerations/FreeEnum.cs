namespace HaoKao.PaperService.Domain.Enumerations;

/// <summary>
/// 是否限免
/// </summary>
[Description("是否限免")]
public enum FreeEnum
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 不限免
    /// </summary>
    [Description("不限免")]
    NoFree = 1,

    /// <summary>
    /// 限免
    /// </summary>
    [Description("限免")]
    Free = 2,
}