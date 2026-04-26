namespace HaoKao.BasicService.Domain.Enumerations;

/// <summary>
/// 授权类型
/// </summary>
public enum AuthorizeType
{
    /// <summary>
    /// 考试管理授权
    /// </summary>
    [Description("管理员授权")]
    AdminUser,

    /// <summary>
    /// 机构授权
    /// </summary>
    [Description("普通用户授权")]
    GeneralUser
}