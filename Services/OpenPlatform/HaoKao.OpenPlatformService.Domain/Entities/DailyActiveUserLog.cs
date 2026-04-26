using Microsoft.EntityFrameworkCore;

namespace HaoKao.OpenPlatformService.Domain.Entities;

/// <summary>
/// 用户每日活跃记录
/// </summary>
[Comment("用户每日活跃记录")]
public class DailyActiveUserLog : AggregateRoot<Guid>, IIncludeCreateTime
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 活跃的客户端Id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// 活跃的日期(yyyy-MM-dd)
    /// </summary>
    public string CreateDate { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}