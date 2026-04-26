namespace HaoKao.OpenPlatformService.Application.ViewModels.Identity;

[AutoMapFrom(typeof(Domain.Entities.RegisterUser))]
public class RgeisterUserViewModel : IDto
{
    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender UserGender { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserState UserState { get; set; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; set; }
}