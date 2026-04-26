using ShortUrlService.Domain.Entities;

namespace ShortUrlService.WebApi.Models;

[AutoMapFrom(typeof(RegisterApp))]
public record RegisterAppDto : IDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public required string AppName { get; init; }

    /// <summary>
    /// 应用编码
    /// </summary>
    public required string AppCode { get; init; }

    /// <summary>
    /// 应用密钥
    /// </summary>
    public required string AppSecret { get; init; }

    /// <summary>
    /// 应用描述
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable { get; init; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; }

    /// <summary>
    /// 应用域名
    /// </summary>
    public List<string> AppDomains { get; init; } = [];
}

public record PagedRegisterAppDto(int TotalCount, IReadOnlyList<RegisterAppDto> Data);