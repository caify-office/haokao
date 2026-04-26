namespace HaoKao.OpenPlatformService.Domain.Repositories;

public interface IDailyActiveUserLogRepository : IRepository<DailyActiveUserLog>
{
    public IQueryable<DailyActiveUserLog> Query { get; }

    /// <summary>
    /// 每日活跃用户走势
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="prev"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    Task<Dictionary<string, int>> QueryDailyActiveUserTrend(DateTime? start, DateTime? end, DateTime? prev, DateTime? next);
}