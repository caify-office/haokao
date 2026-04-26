using ShortUrlService.Domain.Commands;

namespace ShortUrlService.WebApi.Models;

[AutoMapTo(typeof(CreateRegisterAppCommand))]
public record CreateRegisterAppRequest(
    string AppName,
    string AppCode,
    string? Description,
    IReadOnlyList<string> AppDomains
) : IDto;