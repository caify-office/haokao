namespace HaoKao.BasicService.Application.ViewModels.User;

/// <summary>
/// 用户修改登陆密码模型
/// </summary>
public class UserEditPasswordViewModel
{
    /// <summary>
    /// 新的用户密码
    /// </summary>
    [Required(ErrorMessage = "用户新密码不能为空")]
    public string NewPassword { get;  set; }

    /// <summary>
    /// 旧的用户密码
    /// </summary>
    [Required(ErrorMessage = "用户旧密码不能为空")]
    public string OldPassword { get;  set; }
}