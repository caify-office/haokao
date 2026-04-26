namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 注册用户
/// </summary>
public class RegisterUser : AggregateRoot<Guid>, IIncludeCreateTime
{
    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

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

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 其它平台中的身份
    /// </summary>
    public List<ExternalIdentity> ExternalIdentities { get; set; } = [];

    /// <summary>
    /// 客户端Id
    /// </summary>
    public string ClientId { get; set; }
}