using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class DateAnswerRecordRepository : Repository<DateAnswerRecord>, IDateAnswerRecordRepository
{
    public Task<DateAnswerRecord> GetById(Guid id)
    {
        return Queryable.AsNoTracking()
                        .Include(x => x.AnswerRecord)
                        .ThenInclude(x => x.Questions)
                        .FirstOrDefaultAsync(x => x.Id == id);
    }

    // <inheritdoc />
    public async Task<DateAnswerRecord> GetTodayTaskRecord(Guid userId, Guid subjectId)
    {
        return await Queryable.AsNoTracking()
                              .Where(t => t.CreatorId == userId
                                       && t.SubjectId == subjectId
                                       && t.Type == SubmitAnswerType.TodayTask
                                       && t.Date == DateOnly.FromDateTime(DateTime.Now))
                              .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<DateAnswerRecord> GetDailyRecord(Guid userId, Guid subjectId)
    {
        return await Queryable.AsNoTracking()
                              .Include(x => x.AnswerRecord)
                              .ThenInclude(x => x.Questions)
                              .Where(t => t.CreatorId == userId
                                       && t.SubjectId == subjectId
                                       && t.Type == SubmitAnswerType.Daily
                                       && t.Date == DateOnly.FromDateTime(DateTime.Now))
                              .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<DateOnly>> GetDailyRecordList(Guid userId, Guid subjectId, DateOnly date)
    {
        var start = new DateOnly(date.Year, date.Month, 1);
        var end = start.AddMonths(1);

        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.SubjectId == subjectId
                      && t.Type == SubmitAnswerType.Daily
                      && t.Date >= start
                      && t.Date < end
                   select t.Date;
        return await linq.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<decimal> GetDailyRankingRatio(Guid userId, Guid subjectId, DateOnly date)
    {
        var start = new DateOnly(date.Year, date.Month, 1);
        var end = start.AddMonths(1);

        var linq = from t in Queryable.AsNoTracking()
                   where t.SubjectId == subjectId
                      && t.Type == SubmitAnswerType.Daily
                      && t.Date >= start
                      && t.Date < end
                   group t by t.CreatorId into g
                   orderby g.Count()
                   select new KeyValuePair<Guid, int>(g.Key, g.Count());

        var dict = await linq.ToListAsync();

        if (dict.Count == 0) return 0.0M;

        // 用户排名
        var index = dict.FindIndex(x => x.Key.Equals(userId));

        // 当前用户未打卡
        if (index < 0) return 0.0M;

        // 处理用户打卡次数相同的情况
        // 从当前排名向前查找，直到找到第一个打卡次数不同的用户
        // 这样可以确保相同打卡次数的用户获得相同的排名
        while (index > 0 && dict[index].Value == dict[index - 1].Value) index--;

        // 计算超过人数比例
        var fixPercentage = Convert.ToDecimal((double)index / dict.Count) * 100;
        return Math.Round(fixPercentage, 1);
    }

    /// <inheritdoc />
    public Task<int> GetDailyUserCount(Guid subjectId)
    {
        return Queryable.AsNoTracking()
                        .Where(t => t.SubjectId == subjectId && t.Type == SubmitAnswerType.Daily)
                        .Select(t => t.CreatorId).Distinct().CountAsync();
    }

    /// <inheritdoc />
    public Task<bool> Exist(Guid userId, Guid subjectId, DateOnly date, SubmitAnswerType type)
    {
        return Queryable.AsNoTracking()
                        .AnyAsync(t => t.CreatorId == userId
                                    && t.SubjectId == subjectId
                                    && t.Date == date
                                    && t.Type == type);
    }
}