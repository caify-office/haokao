namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播在线用户表
/// </summary>
public class LiveOnlineUser : AggregateRoot<Guid>,
                              ITenantShardingTable,
                              IIncludeMultiTenant<Guid>,
                              IIncludeCreatorId<Guid>,
                              IIncludeCreatorName,
                              IIncludeCreateTime
{
    /// <summary>
    /// 直播Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// 累计在线时长
    /// </summary>
    public int OnlineDuration { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 首次上线时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 最后上线时间
    /// </summary>
    public DateTime LastOnlineTime { get; set; }
}


/*

INSERT INTO `LiveOnlineUserTrend`(`LiveId`, `Interval`, `TotalCount`, `OnlineCount`, `CreateTime`, `TenantId`)
SELECT `t0`.`Id`, 2, `t1`.`TotalCount`, `t1`.`OnlineCount`, NOW(), `t0`.`TenantId`
FROM `LiveVideo` `t0`
JOIN (
  SELECT `LiveId`, COUNT(1) `TotalCount`, SUM(CASE WHEN `IsOnline` THEN 1 ELSE 0 END) `OnlineCount`
  FROM `LiveOnlineUser`
  GROUP BY `LiveId`
) `t1` ON `t1`.`LiveId` = `t0`.`Id`
WHERE `t0`.`LiveStatus` = 1

 */

/// <summary>
/// 在线用户走势图
/// </summary>
public class LiveOnlineUserTrend : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 直播Id
    /// </summary>
    public Guid LiveId { get; set; }

    /// <summary>
    /// 统计间隔(分钟)
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    /// 累计在线人数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 当前在线人数
    /// </summary>
    public int OnlineCount { get; set; }

    /// <summary>
    /// 统计时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}