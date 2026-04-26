using Girvs.Extensions;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

public class ModifyPasswordViewModel
{
    private string _oldPassword;

    /// <summary>
    /// 旧密码  Base64加密
    /// </summary>
    [Required(ErrorMessage = "旧密码不能为空")]
    public string OldPassword
    {
        get => _oldPassword?.Base64Decode();
        set => _oldPassword = value;
    }

    private string _newPassword;

    /// <summary>
    /// 新密码  Base64加密
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword
    {
        get => _newPassword?.Base64Decode();
        set => _newPassword = value;
    }
}