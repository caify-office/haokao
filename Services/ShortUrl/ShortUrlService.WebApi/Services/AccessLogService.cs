using ShortUrlService.Domain.Charts;
using ShortUrlService.Domain.Entities;
using ShortUrlService.Domain.Repositories;
using ShortUrlService.WebApi.Models;

namespace ShortUrlService.WebApi.Services;

public interface IAccessLogService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取短链接的访问日志列表
    /// </summary>
    /// <param name="shortUrlId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PagingResult<AccessLog>> GetAsync(long shortUrlId, PagingRequest request);
}

[DynamicWebApi]
[Route("apiShortUrl/AccessLogService")]
public class AccessLogService(IAccessLogRepository repository) : IAccessLogService
{
    /// <summary>
    /// 获取短链接的访问日志列表
    /// </summary>
    /// <param name="shortUrlId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("{shortUrlId:long}")]
    public async Task<PagingResult<AccessLog>> GetAsync(long shortUrlId, [FromQuery] PagingRequest request)
    {
        var (total, data) = await repository.GetPagedListAsync(shortUrlId, request.PageIndex, request.PageSize);
        return new PagingResult<AccessLog>(total, data);
    }

    /// <summary>
    /// 获取短链接的访问统计图标
    /// </summary>
    /// <param name="chartFactory"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet("Charts")]
    public Task<ShortUrlChartsOutput> GetCharts([FromServices] IShortUrlChartFactory chartFactory, ChartTypeEnum type)
    {
        var chart = chartFactory.GetChart(type);
        return chart.GetCharts();
    }
}