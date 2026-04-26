using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播回放
/// </summary>
public class LivePlayBack : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
{
    /// <summary>
    /// 所属视频直播
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public LiveVideo LiveVideo { get; set; }

    /// <summary>
    /// 所属视频直播Id
    /// </summary>
    public Guid LiveVideoId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 视频格式
    /// </summary>
    public VideoType VideoType { get; set; }

    /// <summary>
    /// 视频时长
    /// </summary>
    public decimal Duration { get; set; }

    /// <summary>
    /// 视频序号
    /// </summary>
    public string VideoNo { get; set; }

    /// <summary>
    /// 创建时间（添加时间）
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}