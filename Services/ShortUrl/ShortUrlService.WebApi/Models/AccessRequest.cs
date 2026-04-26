namespace ShortUrlService.WebApi.Models;

public record AccessRequest(string ShortKey) : IDto;