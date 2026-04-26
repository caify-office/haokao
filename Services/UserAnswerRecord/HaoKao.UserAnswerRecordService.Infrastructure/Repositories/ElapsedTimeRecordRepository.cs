using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class ElapsedTimeRecordRepository : Repository<ElapsedTimeRecord>, IElapsedTimeRecordRepository
{
    /// <inheritdoc />
    public Task<long> GetChapterElapsedTime(Guid userId, Guid subjectId)
    {
        return Queryable.AsNoTracking()
                        .Where(t => t.CreatorId == userId
                                 && t.SubjectId == subjectId
                                 && (t.Type == SubmitAnswerType.Chapter
                                  || t.Type == SubmitAnswerType.Section
                                  || t.Type == SubmitAnswerType.KnowledgePoint))
                        .SumAsync(t => t.ElapsedSeconds);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<(DateOnly Date, long TotalSeconds)>> GetDateElaspedTime(Guid userId, Guid productId, Guid subjectId, DateOnly startDate, DateOnly endDate)
    {
        // 筛选出在指定日期范围内的、每个知识点的最新记录
        var baseQuery = Queryable.AsNoTracking()
                                 .Where(r => r.CreatorId == userId
                                          && r.ProductId == productId
                                          && r.SubjectId == subjectId
                                          && r.CreateDate >= startDate
                                          && r.CreateDate <= endDate);

        // 按日期分组，并加总学习时长
        var dailyStats = await baseQuery.GroupBy(r => r.CreateDate).Select(g => new
        {
            Date = g.Key,
            TotalSeconds = g.Sum(r => r.ElapsedSeconds)
        }).ToListAsync();

        return dailyStats.Select(s => (s.Date, s.TotalSeconds)).ToList();
    }
}