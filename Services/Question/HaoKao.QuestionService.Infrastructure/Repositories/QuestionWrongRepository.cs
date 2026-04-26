using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;

namespace HaoKao.QuestionService.Infrastructure.Repositories;

public class QuestionWrongRepository(
    QuestionDbContext dbContext,
    IQuestionRepository repository
) : Repository<QuestionWrong>, IQuestionWrongRepository
{
    public IQueryable<QuestionWrong> Query => Queryable.AsNoTracking();

    /// <inheritdoc />
    public async Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetChapterList(Guid userId, Guid subjectId, bool isActive)
    {
        var linq = from t in dbContext.Questions
                   join q in dbContext.QuestionWrongs on t.Id equals q.QuestionId
                   where t.SubjectId == subjectId
                      && q.CreatorId == userId
                      && t.EnableState == EnableState.Enable
                      && q.IsActive == isActive
                   group t by new { t.ChapterId, t.ChapterName } into g
                   select new
                   {
                       Id = g.Key.ChapterId,
                       Name = g.Key.ChapterName,
                       Count = g.Sum(x => x.QuestionCount)
                   };
        var list = await linq.ToListAsync();
        return list.Select(x => (x.Id, x.Name, x.Count)).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Question>> GetChapterQuestionList(Guid userId, Guid chapterId, bool isActive)
    {
        var linq = from t in dbContext.Questions
                   join q in dbContext.QuestionWrongs on t.Id equals q.QuestionId
                   where t.ChapterId == chapterId
                      && q.CreatorId == userId
                      && t.EnableState == EnableState.Enable
                      && q.IsActive == isActive
                   select t;
        return await repository.GetListUnionChildren(linq);
    }

    /// <inheritdoc />
    public async Task<List<Guid>> GetTodayTaskQuestionIds(Guid subjectId, int extraction)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        // 判断是否存在错题
        if (!await ExistEntityAsync(x => x.CreatorId == userId &&
                                         x.Question.SubjectId == subjectId &&
                                         x.IsActive))
        {
            return [];
        }

        // 获取所有错题Id和题型Id
        var query = from t in Query.Include(x => x.Question)
                    where t.IsActive
                       && t.CreatorId == userId
                       && t.Question.SubjectId == subjectId
                       && t.Question.EnableState == EnableState.Enable
                    select new { t.Question.Id, t.Question.QuestionTypeId };
        var list = await query.ToListAsync();

        // 如果错题数量不足, 直接返回
        if (list.Count <= extraction) return list.Select(x => x.Id).ToList();

        // 计算不同题型的错题比例和应该抽取的题目数量
        var dict = list.GroupBy(x => x.QuestionTypeId).ToDictionary(
            x => x.Key,
            x => (int)Math.Round(1.0 * x.Count() / list.Count * extraction)
        );

        // 按比例随机抽取题目
        var ids = new List<Guid>(dict.Values.Count);
        foreach (var key in dict.Keys)
        {
            var temp = list.Where(x => x.QuestionTypeId == key)
                           .OrderBy(_ => Guid.NewGuid())
                           .Take(dict[key])
                           .Select(x => x.Id);
            ids.AddRange(temp);
        }

        // 如果数量不够, 随机抽取剩余的题目
        if (ids.Count < extraction)
        {
            var temp = list.Where(x => !ids.Contains(x.Id))
                           .OrderBy(_ => Guid.NewGuid())
                           .Take(extraction - ids.Count)
                           .Select(x => x.Id);
            ids.AddRange(temp);
        }

        return ids.Take(extraction).ToList();
    }

    public override async Task<List<QuestionWrong>> GetByQueryAsync(QueryBase<QuestionWrong> query)
    {
        query.RecordCount = await Query.Where(query.GetQueryWhere()).CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            // 查询错题是根据QuestionId去重
            query.Result = await Query.Include(x => x.Question)
                                      .Where(query.GetQueryWhere())
                                      .OrderBy(x => x.Sort)
                                      .ThenByDescending(x => x.CreateTime)
                                      .Skip(query.PageStart)
                                      .Take(query.PageSize)
                                      .ToListAsync();
        }

        return query.Result;
    }
}