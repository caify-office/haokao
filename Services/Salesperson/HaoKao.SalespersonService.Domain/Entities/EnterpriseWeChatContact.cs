namespace HaoKao.SalespersonService.Domain.Entities;

/// <summary>
/// 企业微信联系人
/// </summary>
public class EnterpriseWeChatContact : AggregateRoot<Guid>, IIncludeCreateTime
{
    /// <summary>
    /// 添加了此外部联系人的企业成员Id
    /// </summary>
    public string FollowUserId { get; init; }

    /// <summary>
    /// 添加了此外部联系人的企业成员名称
    /// </summary>
    public string FollowUserName { get; init; }

    /// <summary>
    /// 联系人的类型，1表示该外部联系人是微信用户，2表示该外部联系人是企业微信用户
    /// </summary>
    public int Type { get; init; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 微信用户Id
    /// </summary>
    public string UserId { get; init; }

    /// <summary>
    /// 微信unionid
    /// </summary>
    public string UnionId { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}