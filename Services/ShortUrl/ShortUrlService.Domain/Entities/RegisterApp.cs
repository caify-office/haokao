using Girvs.BusinessBasis.Entities;
using System.Text.Json.Serialization;

namespace ShortUrlService.Domain.Entities;

/// <summary>
/// 注册应用
/// </summary>
public class RegisterApp : AggregateRoot<long>, IIncludeCreateTime, IIncludeUpdateTime
{
    /// <summary>
    /// 应用名称
    /// </summary>
    public string AppName { get; set; } = null!;

    /// <summary>
    /// 应用编码
    /// </summary>
    public string AppCode { get; set; } = null!;

    /// <summary>
    /// 应用密钥
    /// </summary>
    public string AppSecret { get; set; } = null!;

    /// <summary>
    /// 应用描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 应用域名
    /// </summary>
    public List<string> AppDomains { get; set; } = [];

    /// <summary>
    /// 应用的短链
    /// </summary
    [JsonIgnore]
    public IReadOnlyList<ShortUrl> ShortUrls { get; set; } = [];
}