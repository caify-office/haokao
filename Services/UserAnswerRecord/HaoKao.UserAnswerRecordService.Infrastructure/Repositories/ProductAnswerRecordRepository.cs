using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Queries;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class ProductChapterAnswerRecordRepository : Repository<ProductChapterAnswerRecord>, IProductChapterAnswerRecordRepository
{
    /// <inheritdoc />
    public Task<ProductChapterAnswerRecord> GetById(Guid id)
    {
        return Queryable.AsNoTracking().Include(x => x.AnswerRecord).ThenInclude(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public Task<Dictionary<Guid, Guid>> GetChapterRecordIds(Guid userId, Guid productId, IReadOnlyList<Guid> chapterIds)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.ProductId == productId
                      && chapterIds.Contains(t.ChapterId)
                   select t;

        return GetLatestQuery(linq).ToDictionaryAsync(r => r.ChapterId, r => r.Id);
    }

    /// <inheritdoc />
    public async Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId)
    {
        var baseQuery = Queryable.AsNoTracking()
                                 .Where(r => r.CreatorId == userId
                                          && r.ProductId == productId
                                          && r.SubjectId == subjectId);

        var latestQuery = GetLatestQuery(baseQuery);

        var stats = await latestQuery.Select(g => new
        {
            g.AnswerRecord.AnswerCount,
            g.AnswerRecord.CorrectCount,
            g.AnswerRecord.QuestionCount
        }).ToListAsync();

        return (stats.Sum(s => s.AnswerCount), stats.Sum(s => s.CorrectCount), stats.Sum(s => s.QuestionCount));
    }

    private IQueryable<ProductChapterAnswerRecord> GetLatestQuery(IQueryable<ProductChapterAnswerRecord> linq)
    {
        var latest = from t in linq
                     group t by new
                     {
                         t.CreatorId,
                         t.ProductId,
                         t.ChapterId,
                     } into g
                     select new
                     {
                         g.Key.CreatorId,
                         g.Key.ProductId,
                         g.Key.ChapterId,
                         CreateTime = g.Max(x => x.CreateTime)
                     };

        return from t in Queryable.AsNoTracking().Include(x => x.AnswerRecord)
               join l in latest on new
               {
                   t.CreatorId,
                   t.ProductId,
                   t.ChapterId,
                   t.CreateTime
               } equals new
               {
                   l.CreatorId,
                   l.ProductId,
                   l.ChapterId,
                   l.CreateTime
               }
               select t;
    }
}

public class ProductPaperAnswerRecordRepository : Repository<ProductPaperAnswerRecord>, IProductPaperAnswerRecordRepository
{
    /// <inheritdoc />
    public Task<ProductPaperAnswerRecord> GetById(Guid id)
    {
        return Queryable.AsNoTracking().Include(x => x.AnswerRecord).ThenInclude(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public Task<Dictionary<Guid, Guid>> GetPaperRecordIds(Guid userId, Guid productId, IReadOnlyList<Guid> paperIds)
    {
        var linq = from t in Queryable.AsNoTracking()
                   where t.CreatorId == userId
                      && t.ProductId == productId
                      && paperIds.Contains(t.PaperId)
                   select t;

        return GetLatestQuery(linq).ToDictionaryAsync(r => r.PaperId, r => r.Id);
    }

    /// <inheritdoc />
    public async Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId)
    {
        var baseQuery = Queryable.AsNoTracking()
                                 .Where(r => r.CreatorId == userId
                                          && r.ProductId == productId
                                          && r.SubjectId == subjectId);

        var latestQuery = GetLatestQuery(baseQuery);

        var stats = await latestQuery.Select(g => new
        {
            g.AnswerRecord.AnswerCount,
            g.AnswerRecord.CorrectCount,
            g.AnswerRecord.QuestionCount
        }).ToListAsync();

        return (stats.Sum(s => s.AnswerCount), stats.Sum(s => s.CorrectCount), stats.Sum(s => s.QuestionCount));
    }

    private IQueryable<ProductPaperAnswerRecord> GetLatestQuery(IQueryable<ProductPaperAnswerRecord> linq)
    {
        var latest = from t in linq
                     group t by new
                     {
                         t.CreatorId,
                         t.ProductId,
                         t.PaperId,
                     } into g
                     select new
                     {
                         g.Key.CreatorId,
                         g.Key.ProductId,
                         g.Key.PaperId,
                         CreateTime = g.Max(x => x.CreateTime)
                     };

        return from t in Queryable.AsNoTracking().Include(x => x.AnswerRecord)
               join l in latest on new
               {
                   t.CreatorId,
                   t.ProductId,
                   t.PaperId,
                   t.CreateTime
               } equals new
               {
                   l.CreatorId,
                   l.ProductId,
                   l.PaperId,
                   l.CreateTime
               }
               select t;
    }
}

public class ProductKnowledgeAnswerRecordRepository : Repository<ProductKnowledgeAnswerRecord>, IProductKnowledgeAnswerRecordRepository
{
    /// <inheritdoc />
    public Task<ProductKnowledgeAnswerRecord> GetById(Guid id)
    {
        return Queryable.AsNoTracking().Include(x => x.AnswerRecord).ThenInclude(x => x.Questions).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<ProductKnowledgeAnswerRecordQuery> GetKnowledgePointList(ProductKnowledgeAnswerRecordQuery query)
    {
        var linqQuery = Queryable.AsNoTracking().Include(x => x.AnswerRecord).Where(query.GetQueryWhere());

        var latestQuery = GetLatestQuery(linqQuery);

        query.RecordCount = await latestQuery.CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await latestQuery.Skip(query.PageStart).Take(query.PageSize).ToListAsync();
        }
        return query;
    }

    /// <inheritdoc />
    public Task<Dictionary<MasteryLevel, int>> GetKnowledgeMasteryStat(ProductKnowledgeAnswerRecordQuery query)
    {
        // 筛选指定用户、产品和科目的记录
        var baseQuery = Queryable.AsNoTracking().Where(query.GetQueryWhere());

        // 为每个知识点找到最新的作答记录
        var latestQuery = GetLatestQuery(baseQuery);

        // 按掌握程度分组并计数
        return latestQuery
               .GroupBy(r => r.MasteryLevel)
               .Select(g => new { MasteryLevel = g.Key, Count = g.Count() })
               .ToDictionaryAsync(x => x.MasteryLevel, x => x.Count);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<(ExamFrequency Frequency, MasteryLevel Mastery)>> GetExamFrequencyMastery(Guid userId, Guid productId, Guid subjectId)
    {
        var baseQuery = Queryable.AsNoTracking()
                                 .Where(r => r.CreatorId == userId
                                          && r.ProductId == productId
                                          && r.SubjectId == subjectId);

        var latestQuery = GetLatestQuery(baseQuery);

        var stats = await latestQuery.Select(r => new { r.ExamFrequency, r.MasteryLevel }).ToListAsync();

        return stats.Select(s => (s.ExamFrequency, s.MasteryLevel)).ToList();
    }

    /// <inheritdoc />
    public async Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId)
    {
        var baseQuery = Queryable.AsNoTracking()
                                 .Where(r => r.CreatorId == userId
                                          && r.ProductId == productId
                                          && r.SubjectId == subjectId);

        var latestQuery = GetLatestQuery(baseQuery);

        var stats = await latestQuery.Select(g => new
        {
            g.AnswerRecord.AnswerCount,
            g.AnswerRecord.CorrectCount,
            g.AnswerRecord.QuestionCount
        }).ToListAsync();

        return (stats.Sum(s => s.AnswerCount), stats.Sum(s => s.CorrectCount), stats.Sum(s => s.QuestionCount));
    }

    private IQueryable<ProductKnowledgeAnswerRecord> GetLatestQuery(IQueryable<ProductKnowledgeAnswerRecord> linq)
    {
        var latest = from t in linq
                     group t by new
                     {
                         t.CreatorId,
                         t.ProductId,
                         t.KnowledgePointId,
                     } into g
                     select new
                     {
                         g.Key.CreatorId,
                         g.Key.ProductId,
                         g.Key.KnowledgePointId,
                         CreateTime = g.Max(x => x.CreateTime)
                     };

        return from t in Queryable.AsNoTracking().Include(x => x.AnswerRecord)
               join l in latest on new
               {
                   t.CreatorId,
                   t.ProductId,
                   t.KnowledgePointId,
                   t.CreateTime
               } equals new
               {
                   l.CreatorId,
                   l.ProductId,
                   l.KnowledgePointId,
                   l.CreateTime
               }
               select t;
    }
}