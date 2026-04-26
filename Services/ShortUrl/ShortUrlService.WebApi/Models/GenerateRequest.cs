using ShortUrlService.Domain.Commands;

namespace ShortUrlService.WebApi.Models;

[AutoMapTo(typeof(CreateShortUrlCommand))]
public record GenerateRequest(string OriginUrl, int AccessLimit, DateTime ExpiredTime, string AppCode, string AppSecret) : IDto;

public record CreateShortUrlRequest(string OriginUrl, int AccessLimit, DateTime ExpiredTime, long RegisterAppId) : IDto;

public record UpdateShortUrlRequest(long Id, int AccessLimit, DateTime ExpiredTime);