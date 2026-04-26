namespace HaoKao.BasicService.Application.ViewModels.User;

/// <summary>
/// 修改用户登陆密码模型
/// </summary>
public class ChangeUserPassworkViewModel
{
    /// <summary>
    /// 用户新的登陆密码
    /// </summary>
    [Required(ErrorMessage ="用户新密码不能为空")]
    public string NewPassword { get; set; }
}