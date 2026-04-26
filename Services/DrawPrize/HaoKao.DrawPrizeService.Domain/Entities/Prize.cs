using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HaoKao.DrawPrizeService.Domain.Entities;

/// <summary>
/// 奖品
/// </summary>
public class Prize : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>
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
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 奖品数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 显示数量
    /// </summary>
    public int DisplayCount { get; set; }

    /// <summary>
    /// 中奖范围
    /// </summary>
    public WinningRange WinningRange { get; set; }

    /// <summary>
    /// 指定学员
    /// </summary>
    public List<DesignatedStudent> DesignatedStudents { get; set; }

    /// <summary>
    /// 是否保底
    /// </summary>
    public bool IsGuaranteed { get; set; }

    /// <summary>
    /// 已颁发的奖品数量
    /// </summary>
    [Range(0, int.MaxValue)]
    public int AwardedQuantity { get; set; }

    /// <summary>
    /// 乐观锁版本标记
    /// </summary>
    public DateTime Version { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}

public class DesignatedStudent
{
    public Guid UserId { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }
}