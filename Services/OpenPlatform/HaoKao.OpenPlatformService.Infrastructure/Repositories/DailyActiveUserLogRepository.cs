using Girvs.Extensions;
using System.Linq;

namespace HaoKao.OpenPlatformService.Infrastructure.Repositories;

public class DailyActiveUserLogRepository : Repository<DailyActiveUserLog>, IDailyActiveUserLogRepository
{
    public IQueryable<DailyActiveUserLog> Query => Queryable;

    /// <summary>
    /// 每日活跃用户走势
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="prev"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, int>> QueryDailyActiveUserTrend(DateTime? start,
                                                                         DateTime? end,
                                                                         DateTime? prev,
                                                                         DateTime? next)
    {
        var predicate = Predicate(start, end, prev, next);
        var list = await Queryable.Where(predicate)
                                  .GroupBy(x => x.CreateDate)
                                  .Select(x => new { x.Key, Count = x.Count() })
                                  .ToListAsync();

        var dict = GetDates(start, end, prev, next).ToDictionary(x => x.ToString("yyyy-MM-dd"), x => 0);

        foreach (var item in list)
        {
            if (dict.ContainsKey(item.Key))
            {
                dict[item.Key] = item.Count;
            }
        }

        return dict;
    }

    static Expression<Func<DailyActiveUserLog, bool>> Predicate(DateTime? start,
                                                                DateTime? end,
                                                                DateTime? prev,
                                                                DateTime? next)
    {
        Expression<Func<DailyActiveUserLog, bool>> predicate = x => true;

        if (start.HasValue)
        {
            predicate = predicate.And(x => x.CreateTime >= start.Value);
        }
        if (end.HasValue)
        {
            var endTime = end.Value.AddDays(1);
            predicate = predicate.And(x => x.CreateTime <= endTime);
        }
        if (prev.HasValue)
        {
            var dates = GetWeekDates(prev.Value.AddDays(-1), false).Select(x => x.ToString("yyyy-MM-dd"));
            predicate = predicate.And(x => dates.Contains(x.CreateDate));
        }
        if (next.HasValue)
        {
            var dates = GetWeekDates(next.Value.AddDays(1), true).Select(x => x.ToString("yyyy-MM-dd"));
            predicate = predicate.And(x => dates.Contains(x.CreateDate));
        }

        return predicate;
    }

    static DateTime[] GetDates(DateTime? start,
                               DateTime? end,
                               DateTime? prev,
                               DateTime? next)
    {
        if (prev.HasValue)
        {
            return GetWeekDates(prev.Value.AddDays(-1), false);
        }
        if (next.HasValue)
        {
            return GetWeekDates(next.Value.AddDays(1), true);
        }
        if (start.HasValue && end.HasValue)
        {
            return GetSerialDates(start.Value, end.Value);
        }
        return GetWeekDates(DateTime.Now, false);
    }

    static DateTime[] GetSerialDates(DateTime start, DateTime end)
    {
        var dates = new List<DateTime>();
        for (var date = end; date >= start; date = date.AddDays(-1))
        {
            dates.Add(date);
        }
        return dates.OrderBy(x => x).ToArray();
    }

    static DateTime[] GetWeekDates(DateTime inputDate, bool isNextWeek)
    {
        var dates = new DateTime[7];
        for (var i = 0; i < 7; i++)
        {
            if (isNextWeek)
            {
                dates[i] = inputDate.AddDays(i);
            }
            else
            {
                dates[i] = inputDate.AddDays(-i);
            }
        }
        return dates.OrderBy(x => x).ToArray();
    }
}