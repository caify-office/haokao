namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public class UpdateRegisterViewModel
{
    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender UserGender { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; set; }
}