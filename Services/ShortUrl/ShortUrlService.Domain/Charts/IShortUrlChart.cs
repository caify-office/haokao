using ShortUrlService.Domain.Entities;

namespace ShortUrlService.Domain.Charts;

public interface IShortUrlChart
{
    ChartTypeEnum ChartType { get; }

    Task<ShortUrlChartsOutput> GetCharts();
}

public interface IShortUrlChartFactory : IManager
{
    IShortUrlChart GetChart(ChartTypeEnum type);
}

public class ShortUrlChartFactory(IEnumerable<IShortUrlChart> charts) : IShortUrlChartFactory
{
    public IShortUrlChart GetChart(ChartTypeEnum type)
    {
        return charts.First(x => x.ChartType == type);
    }
}

public abstract class ShortUrlChartAbstract : IShortUrlChart
{
    public abstract ChartTypeEnum ChartType { get; }

    public abstract Task<ShortUrlChartsOutput> GetCharts();

    protected IReadOnlyList<AccessLog> AccessLogs { get; set; } = [];

    protected IReadOnlyList<ShortUrl> ShortUrls { get; set; } = [];

    protected int GetGenerateCount(DateTime start, DateTime end)
    {
        return ShortUrls.Count(x => x.CreateTime >= start && x.CreateTime < end);
    }

    protected int GetAccessCount(DateTime start, DateTime end)
    {
        return AccessLogs.Count(x => x.CreateTime >= start && x.CreateTime < end);
    }

    protected Dictionary<int, int> GetAccessCountGroupByBrowser(DateTime start, DateTime end)
    {
        return AccessLogs
               .Where(x => x.CreateTime >= start && x.CreateTime < end)
               .GroupBy(x => x.BrowserType)
               .ToDictionary(x => x.Key, x => x.Count());
    }

    protected Dictionary<int, int> GetAccessCountGroupByOs(DateTime start, DateTime end)
    {
        return AccessLogs
               .Where(x => x.CreateTime >= start && x.CreateTime < end)
               .GroupBy(x => x.OsType)
               .ToDictionary(x => x.Key, x => x.Count());
    }
}