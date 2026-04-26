namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;

/// <summary>
/// 外部认证用户信息
/// </summary>
public class ExternalUser
{
    /// <summary>
    /// 外部认证程序名称
    /// </summary>
    public string Schemem { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NikeName { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender UserGender { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; set; }

    /// <summary>
    /// 唯一标识符
    /// </summary>
    public string UniqueIdentifier { get; set; }

    /// <summary>
    /// 其它信息
    /// </summary>
    public Dictionary<string, string> OtherInformation { get; set; }
}