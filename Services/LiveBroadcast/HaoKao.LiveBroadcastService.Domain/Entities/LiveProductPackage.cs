using HaoKao.LiveBroadcastService.Domain.Enums;
using System.Text.Json.Serialization;

namespace HaoKao.LiveBroadcastService.Domain.Entities;

/// <summary>
/// 直播产品包
/// </summary>
public class LiveProductPackage : AggregateRoot<Guid>, IIncludeCreateTime, IIncludeMultiTenant<Guid>, ITenantShardingTable
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
    /// 产品包Id
    /// </summary>
    public Guid ProductPackageId { get; set; }

    /// <summary>
    /// 产品包名称
    /// </summary>
    public string ProductPackageName { get; set; }

    /// <summary>
    /// 创建时间（上架时间）
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 产品包类别
    /// </summary>
    public ProductType ProductType { get; set; }
    /// <summary>
    /// 是否上架
    /// </summary>
    public bool IsShelves { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid TenantId { get; set; }
}