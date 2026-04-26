using ShortUrlService.Domain.Repositories;

namespace ShortUrlService.Domain.Charts;

public class DayShortUrlChart(
    IShortUrlRepository urlRepository,
    IAccessLogRepository logRepository
) : ShortUrlChartAbstract
{
    public override ChartTypeEnum ChartType => ChartTypeEnum.Day;

    public override async Task<ShortUrlChartsOutput> GetCharts()
    {
        var now = DateTime.Now;
        var hour = now.Hour;
        var date = now.Date;
        var endDate = date.AddDays(1);

        AccessLogs = await logRepository.GetListAsync(date, endDate);
        ShortUrls = await urlRepository.GetListAsync(date, endDate);

        var output = new ShortUrlChartsOutput(24)
        {
            Title = now.ToString("yyyy-MM-dd"),
        };

        for (var i = 0; i < 24; i++)
        {
            if (i > hour)
            {
                output.Access[i] = 0;
                output.Generate[i] = 0;
            }
            else
            {
                var start = date.AddHours(i);
                var end = date.AddHours(i + 1);
                output.Access[i] = GetAccessCount(start, end);
                output.Generate[i] = GetGenerateCount(start, end);
                output.BrowserAccess[i] = GetAccessCountGroupByBrowser(start, end);
                output.OsAccess[i] = GetAccessCountGroupByOs(start, end);
            }

            output.Labels[i] = date.AddHours(i).ToString("HH:mm");
        }
        return output;
    }
}

public class WeekShortUrlChart(
    IShortUrlRepository urlRepository,
    IAccessLogRepository logRepository
) : ShortUrlChartAbstract
{
    public override ChartTypeEnum ChartType => ChartTypeEnum.Week;

    public override async Task<ShortUrlChartsOutput> GetCharts()
    {
        var now = DateTime.Now.Date;
        var dayOfWeek = now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;
        var endDate = now.AddDays(dayOfWeek);

        AccessLogs = await logRepository.GetListAsync(now.AddDays(-dayOfWeek + 1), endDate);
        ShortUrls = await urlRepository.GetListAsync(now.AddDays(-dayOfWeek + 1), endDate);

        var output = new ShortUrlChartsOutput(7)
        {
            Title = $"{now.AddDays(-dayOfWeek + 1):yyyy-MM-dd}~{now.AddDays(-dayOfWeek + 7):yyyy-MM-dd}",
        };

        for (var i = 0; i < 7; i++)
        {
            var day = now.AddDays(0 - (dayOfWeek - i) + 1);
            if (i >= dayOfWeek)
            {
                output.Access[i] = 0;
                output.Generate[i] = 0;
            }
            else
            {
                var end = day.AddDays(1);
                output.Access[i] = GetAccessCount(day, end);
                output.Generate[i] = GetGenerateCount(day, end);
                output.BrowserAccess[i] = GetAccessCountGroupByBrowser(day, end);
                output.OsAccess[i] = GetAccessCountGroupByOs(day, end);
            }

            output.Labels[i] = $"{day:MM-dd}|{(DayOfWeek)(i == 6 ? 0 : i + 1)}";
        }

        return output;
    }
}

public class MonthShortUrlChart(
    IShortUrlRepository urlRepository,
    IAccessLogRepository logRepository
) : ShortUrlChartAbstract
{
    public override ChartTypeEnum ChartType => ChartTypeEnum.Month;

    public override async Task<ShortUrlChartsOutput> GetCharts()
    {
        var now = DateTime.Now;
        var days = DateTime.DaysInMonth(now.Year, now.Month);
        var day = now.Day;
        var date = new DateTime(now.Year, now.Month, 1);
        var entDate = date.AddDays(days);

        AccessLogs = await logRepository.GetListAsync(date, entDate);
        ShortUrls = await urlRepository.GetListAsync(date, entDate);

        var output = new ShortUrlChartsOutput(days)
        {
            Title = now.ToString("yyyy-MM"),
        };
        for (var i = 0; i < days; i++)
        {
            if (i > day)
            {
                output.Access[i] = 0;
                output.Generate[i] = 0;
            }
            else
            {
                var start = date.AddDays(i);
                var end = date.AddDays(i + 1);
                output.Access[i] = GetAccessCount(start, end);
                output.Generate[i] = GetGenerateCount(start, end);
                output.BrowserAccess[i] = GetAccessCountGroupByBrowser(start, end);
                output.OsAccess[i] = GetAccessCountGroupByOs(start, end);
            }

            output.Labels[i] = date.AddDays(i).ToString("MM-dd");
        }
        return output;
    }
}