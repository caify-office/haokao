namespace HaoKao.OpenPlatformService.Application.IdentityServerExtensions;

/// <summary>
/// 登录的方式
/// </summary>
public enum LoginType
{
    /// <summary>
    /// 本机号码一键登录
    /// </summary>
    LocalPhoneLogin,
    
    /// <summary>
    /// 手机号码加密码登录
    /// </summary>
    PhonePasswordLogin,
    
    /// <summary>
    /// 手机号加验证码登录
    /// </summary>
    PhoneCodeLogin,
    
    /// <summary>
    /// 外部认证登录，提前绑定了用户注册
    /// </summary>
    ExternalIdentityLogin,
    
    /// <summary>
    /// 外部认证后，绑定用户手机号并登录
    /// </summary>
    ExternalIdentityPhoneCodeLogin,
    //
    // /// <summary>
    // /// 其它设备上，外部用户信息登陆提前绑定了手机号码
    // /// </summary>
    // ExternalIdentityDeviceLogin,
    //
    // /// <summary>
    // /// 其它设备上，外部用户信息注册并登录
    // /// </summary>
    // ExternalIdentityDevicePhoneCodeLogin
}