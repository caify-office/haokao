using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class ChapterAnswerRecordRepository : Repository<ChapterAnswerRecord>, IChapterAnswerRecordRepository
{
    private IQueryable<ChapterAnswerRecord> IncludeRecord => Queryable.AsNoTracking().Include(x => x.AnswerRecord);

    private IQueryable<ChapterAnswerRecord> IncludeQuestion => Queryable.AsNoTracking().Include(x => x.AnswerRecord).ThenInclude(x => x.Questions);

    /// <inheritdoc />
    public Task<ChapterAnswerRecord> GetById(Guid id)
    {
        return IncludeQuestion.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<AnswerRecord> GetByChapterId(Guid userId, Guid categoryId, Guid chapterId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.CategoryId == categoryId
                      && t.ChapterId == chapterId
                   select t;
        var list = await GetLatestRecordsQuery(linq, IncludeQuestion).ToListAsync();
        return CreateAnswerRecord(list);
    }

    /// <inheritdoc />
    public async Task<AnswerRecord> GetBySectionId(Guid userId, Guid categoryId, Guid sectionId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.CategoryId == categoryId
                      && t.SectionId == sectionId
                   select t;
        var list = await GetLatestRecordsQuery(linq, IncludeQuestion).ToListAsync();
        return CreateAnswerRecord(list);
    }

    /// <inheritdoc />
    public async Task<AnswerRecord> GetByKnowledgePointId(Guid userId, Guid categoryId, Guid knowledgePointId)
    {
        var record = await Queryable.AsNoTracking()
                                    .Include(x => x.AnswerRecord)
                                    .ThenInclude(x => x.Questions)
                                    .Where(t => t.CreatorId == userId && t.CategoryId == categoryId && t.KnowledgePointId == knowledgePointId)
                                    .OrderByDescending(t => t.CreateTime)
                                    .FirstOrDefaultAsync();

        return record?.AnswerRecord ?? new AnswerRecord();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ChapterAnswerRecord>> GetChapterList(Guid userId, Guid categoryId, Guid subjectId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.SubjectId == subjectId
                      && t.CategoryId == categoryId
                   select t;
        return await GetLatestRecordsQuery(linq, IncludeRecord).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ChapterAnswerRecord>> GetSectionList(Guid userId, Guid categoryId, Guid chapterId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.ChapterId == chapterId
                      && t.CategoryId == categoryId
                   select t;
        return await GetLatestRecordsQuery(linq, IncludeRecord).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ChapterAnswerRecord>> GetKnowledgePointList(Guid userId, Guid categoryId, Guid sectionId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.SectionId == sectionId
                      && t.CategoryId == categoryId
                   select t;
        return await GetLatestRecordsQuery(linq, IncludeRecord).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ChapterRecordStat> GetChapterRecordStat(Guid userId, Guid subjectId)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.SubjectId == subjectId
                   select t;
        var list = await GetLatestRecordsQuery(linq, IncludeRecord).ToListAsync();

        var result = new ChapterRecordStat(
            list.GroupBy(x => x.CategoryId).Sum(g => g.Select(x => x.ChapterId).Distinct().Count()),
            list.Sum(x => x.AnswerRecord.AnswerCount),
            list.Sum(x => x.AnswerRecord.CorrectCount),
            list.Sum(x => x.AnswerRecord.QuestionCount)
        )
        {
            ElapsedTime = await EngineContext.Current.Resolve<IElapsedTimeRecordRepository>()
                                             .GetChapterElapsedTime(userId, subjectId),
        };

        return result;
    }

    #region Private

    private static IQueryable<ChapterAnswerRecord> GetLatestRecordsQuery(
        IQueryable<ChapterAnswerRecord> linq,
        IQueryable<ChapterAnswerRecord> include
    )
    {
        var latest = from t in linq
                     group t by new
                     {
                         t.CreatorId,
                         t.SubjectId,
                         t.CategoryId,
                         t.ChapterId,
                         t.SectionId,
                         t.KnowledgePointId
                     } into g
                     select new
                     {
                         g.Key.CreatorId,
                         g.Key.SubjectId,
                         g.Key.CategoryId,
                         g.Key.ChapterId,
                         g.Key.SectionId,
                         g.Key.KnowledgePointId,
                         CreateTime = g.Max(x => x.CreateTime)
                     };

        return from t in include
               join l in latest on new
               {
                   t.CreatorId,
                   t.SubjectId,
                   t.CategoryId,
                   t.ChapterId,
                   t.SectionId,
                   t.KnowledgePointId,
                   t.CreateTime
               } equals new
               {
                   l.CreatorId,
                   l.SubjectId,
                   l.CategoryId,
                   l.ChapterId,
                   l.SectionId,
                   l.KnowledgePointId,
                   l.CreateTime
               }
               select t;
    }

    private static AnswerRecord CreateAnswerRecord(IReadOnlyList<ChapterAnswerRecord> list)
    {
        if (list == null || list.Count == 0)
        {
            return new();
        }

        return new AnswerRecord
        {
            QuestionCount = list.Sum(x => x.AnswerRecord.QuestionCount),
            AnswerCount = list.Sum(x => x.AnswerRecord.AnswerCount),
            CorrectCount = list.Sum(x => x.AnswerRecord.CorrectCount),
            CreateTime = list.Max(x => x.AnswerRecord.CreateTime),
            Questions = list.SelectMany(x => x.AnswerRecord.Questions).ToList(),
        };
    }

    #endregion
}