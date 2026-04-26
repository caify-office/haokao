namespace HaoKao.BasicService.Application.ViewModels.User;

public class UserChangOldPhoneViewModel : IDto
{
    [Required(ErrorMessage = "用户旧手机号不能为空")]
    public string OldContactNumber { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public string OldPhone => OldContactNumber?.DecodeBase64();
    /// <summary>
    /// 验证码
    /// </summary>
    public string Code { get; set; }
}