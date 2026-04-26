namespace HaoKao.QuestionCategoryService.Domain.Enums;

/// <summary>
/// 显示条件
/// </summary>
public enum DisplayConditionEnum
{
    /// <summary>
    /// 总是显示
    /// </summary>
    AlwaysShow,

    /// <summary>
    /// 总是隐藏
    /// </summary>
    AlwaysHide,

    /// <summary>
    /// 有权限时显示
    /// </summary>
    WhenHasPermission
}