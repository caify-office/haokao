using System.Text.Json.Serialization;

namespace HaoKao.DrawPrizeService.Domain.Entities;

/// <summary>
/// 抽奖记录
/// </summary>
public class DrawPrizeRecord : AggregateRoot<Guid>,
                               IIncludeCreateTime,
                               IIncludeCreatorId<Guid>,
                               IIncludeCreatorName,
                               IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 所属抽奖活动
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public DrawPrize DrawPrize { get; set; }

    /// <summary>
    /// 所属抽奖活动Id
    /// </summary>
    public Guid DrawPrizeId { get; set; }

    /// <summary>
    /// 创建者名称
    /// </summary>
    public string CreatorName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 中奖奖品Id
    /// </summary>
    public Guid PrizeId { get; set; }

    /// <summary>
    /// 奖品名称
    /// </summary>
    public string PrizeName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建者id
    /// </summary>
    public Guid CreatorId { get; set; }
}