using HaoKao.OpenPlatformService.Application.IdentityServerExtensions;
using System.Text.Json.Serialization;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Account;

/// <summary>
/// 用户登陆模型
/// </summary>
public class LoginInput
{
    public LoginType LoginType { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 登陆密码或者手机验证码
    /// </summary>
    public string PasswordCode { get; set; }

    /// <summary>
    /// 是否记住用户名
    /// </summary>
    public bool RememberLogin { get; set; }

    /// <summary>
    /// 返回Url
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// 用户取消
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// 客户端Id
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public string ClientId { get; set; }

    /// <summary>
    /// 微信UnionId
    /// </summary>
    public string UnionId { get; set; }
}