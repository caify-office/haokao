using Girvs.BusinessBasis.Entities;
using System.Text.Json.Serialization;

namespace ShortUrlService.Domain.Entities;

public class ShortUrl : AggregateRoot<long>, IIncludeCreateTime, IIncludeDeleteField
{
    /// <summary>
    /// 注册应用的Id
    /// </summary>
    public long RegisterAppId { get; set; }

    /// <summary>
    /// 短链接后缀
    /// </summary>
    public string ShortKey { get; set; } = null!;

    /// <summary>
    /// 原始链接
    /// </summary>
    public string OriginUrl { get; set; } = null!;

    /// <summary>
    /// 可访问次数
    /// </summary>
    public int AccessLimit { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpiredTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 是否删除标识
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 短链接访问记录
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<AccessLog> AccessLogs { get; set; } = new List<AccessLog>();
}