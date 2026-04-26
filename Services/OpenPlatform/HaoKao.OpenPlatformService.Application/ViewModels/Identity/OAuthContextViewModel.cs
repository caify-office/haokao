namespace HaoKao.OpenPlatformService.Application.ViewModels.Identity;

/// <summary>
/// OAuth访问详细内容
/// </summary>
public class OAuthContextViewModel
{
    /// <summary>
    /// Scheme名称
    /// </summary>
    [Required]
    public string Scheme { get; set; }

    /// <summary>
    /// 回调的服务地址
    /// </summary>
    public string CallBackServerUrl { get; set; }

    /// <summary>
    /// 认证结束后，自动返回的地址
    /// </summary>
    [Required]
    public string ReturnUrl { get; set; }
}