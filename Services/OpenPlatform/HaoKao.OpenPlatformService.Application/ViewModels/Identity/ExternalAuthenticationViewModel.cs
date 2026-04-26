using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;

namespace HaoKao.OpenPlatformService.Application.ViewModels.Identity;

public class ExternalAuthenticationViewModel
{
    /// <summary>
    /// 是否已注册
    /// </summary>
    public bool IsRegister { get; set; }
    
    /// <summary>
    /// 外部程序名称
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// 外部程序获取的用户信息
    /// </summary>
    public ExternalUser ExternalUser { get; set; }
}