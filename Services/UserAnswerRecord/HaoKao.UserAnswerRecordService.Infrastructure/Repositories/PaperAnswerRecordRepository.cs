using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class PaperAnswerRecordRepository : Repository<PaperAnswerRecord>, IPaperAnswerRecordRepository
{
    /// <inheritdoc />
    public async Task<PaperAnswerRecord> GetById(Guid id)
    {
        return await Queryable.AsNoTracking()
                              .Include(x => x.AnswerRecord)
                              .ThenInclude(x => x.Questions)
                              .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaperAnswerRecord>> GetPaperList(Guid userId, Guid subjectId, Guid categoryId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.SubjectId == subjectId
                      && t.CategoryId == categoryId
                   select t;

        return await GetLatestQuery(linq).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaperAnswerRecord>> GetPaperList(Guid userId, IReadOnlyList<Guid> paperIds)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && paperIds.Contains(t.PaperId)
                   select t;

        return await GetLatestQuery(linq).ToListAsync();
    }

    /// <inheritdoc />
    public Task<Dictionary<Guid, int>> GetPaperUserCount(IReadOnlyList<Guid> paperIds)
    {
        return Queryable.AsNoTracking()
                        .Where(t => paperIds.Contains(t.PaperId))
                        .GroupBy(t => t.PaperId)
                        .ToDictionaryAsync(t => t.Key, t => t.Select(x => x.CreatorId).Distinct().Count());
    }

    private IQueryable<PaperAnswerRecord> GetLatestQuery(IQueryable<PaperAnswerRecord> linq)
    {
        var latest = from t in linq
                     group t by new
                     {
                         t.CreatorId,
                         t.SubjectId,
                         t.CategoryId,
                         t.PaperId,
                     } into g
                     select new
                     {
                         g.Key.CreatorId,
                         g.Key.SubjectId,
                         g.Key.CategoryId,
                         g.Key.PaperId,
                         CreateTime = g.Max(x => x.CreateTime)
                     };

        return from t in Queryable.AsNoTracking().Include(x => x.AnswerRecord)
               join l in latest on new
               {
                   t.CreatorId,
                   t.SubjectId,
                   t.CategoryId,
                   t.PaperId,
                   t.CreateTime
               } equals new
               {
                   l.CreatorId,
                   l.SubjectId,
                   l.CategoryId,
                   l.PaperId,
                   l.CreateTime
               }
               select t;
    }
}