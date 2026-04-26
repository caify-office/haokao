namespace ShortUrlService.WebApi.Models;

public record PagingRequest(int PageIndex = 1, int PageSize = 10);

public record PagingResult<T>(int TotalCount, IReadOnlyList<T> Data);