namespace HaoKao.StudentService.Domain.Models;

/// <summary>
/// 注册用户
/// </summary>
public record RegisterUser
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; init; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string Phone { get; init; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; init; }

    /// <summary>
    /// 用户性别
    /// </summary>
    public UserGender UserGender { get; init; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    public string NickName { get; init; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserState UserState { get; init; }

    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string EmailAddress { get; init; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadImage { get; init; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string LastLoginIp { get; init; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime LastLoginTime { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 其它平台中的身份
    /// </summary>
    public List<ExternalIdentity> ExternalIdentities { get; init; } = [];

    /// <summary>
    /// 客户端Id
    /// </summary>
    public string ClientId { get; init; }

    [NotMapped]
    public string UnionId => ExternalIdentities.FirstOrDefault(x => x.Scheme == "Weixin")?.UniqueIdentifier;
}

/// <summary>
/// 用户性别
/// </summary>
[Description("用户性别")]
public enum UserGender
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 男
    /// </summary>
    [Description("男")]
    Male = 1,

    /// <summary>
    /// 女
    /// </summary>
    [Description("女")]
    Female = 2,
}

/// <summary>
/// 用户状态
/// </summary>
[Description("用户状态")]
public enum UserState
{
    /// <summary>
    /// 禁用
    /// </summary>
    [Description("禁用")]
    Disable = 0,

    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable = 1,
}