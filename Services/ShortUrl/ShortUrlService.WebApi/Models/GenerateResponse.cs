namespace ShortUrlService.WebApi.Models;

public record GenerateResponse(string ShortKey, Uri ShortUrl, string QrCodeBase64) : IDto;