namespace HaoKao.DrawPrizeService.Domain.Entities;

/// <summary>
/// 抽奖
/// </summary>
public class DrawPrize : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeUpdateTime, IIncludeMultiTenant<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 活动背景图
    /// </summary>
    public string BackgroundImageUrl { get; set; }

    /// <summary>
    /// 抽奖范围
    /// </summary>
    public DrawPrizeRange? DrawPrizeRange { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public DrawPrizeType? DrawPrizeType { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 中奖概率
    /// </summary>
    public double? Probability { get; set; }

    /// <summary>
    /// 奖品池
    /// </summary>
    public List<Prize> Prizes { get; set; }

    /// <summary>
    /// 抽奖记录
    /// </summary>
    public List<DrawPrizeRecord> DrawPrizeRecords { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}