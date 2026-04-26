using ShortUrlService.Domain.Entities;

namespace ShortUrlService.WebApi.Models;

[AutoMapFrom(typeof(ShortUrl))]
public record ShortUrlDto : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// 注册应用Id
    /// </summary>
    public long RegisterAppId { get; init; }

    /// <summary>
    /// 短链接后缀
    /// </summary>
    public required string ShortKey { get; init; }

    /// <summary>
    /// 原始链接
    /// </summary>
    public required string OriginUrl { get; init; }

    /// <summary>
    /// 可访问次数
    /// </summary>
    public int AccessLimit { get; init; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpiredTime { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }
}

public record ShortUrlPagingRequest : PagingRequest
{
    public long? RegisterAppId { get; init; }
}

public record ShortUrlPagingResult : ShortUrlDto
{
    /// <summary>
    /// 短链接全路径
    /// </summary>
    public string FullUrl { get; init; } = "";

    /// <summary>
    /// 已访问次数
    /// </summary>
    public int ConsumedCount { get; init; }

    /// <summary>
    /// 剩余访问次数
    /// </summary>
    public int RestOfCount => AccessLimit - ConsumedCount;

    /// <summary>
    /// 是否可用
    /// </summary>
    public bool Consumeable => ExpiredTime > DateTime.Now && RestOfCount > 0;
}