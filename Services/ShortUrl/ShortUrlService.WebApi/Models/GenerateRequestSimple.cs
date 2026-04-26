using ShortUrlService.Domain.Commands;

namespace ShortUrlService.WebApi.Models;

[AutoMapTo(typeof(CreateShortUrlCommand))]
public record GenerateRequestSimple(string[] OriginUrlArray, string AppCode, string AppSecret) : IDto;