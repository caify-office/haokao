using HaoKao.QuestionService.Domain.QuestionCollectionModule;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Infrastructure.Repositories;

public class QuestionCollectionRepository(IQuestionRepository questionRepository) : Repository<QuestionCollection>, IQuestionCollectionRepository
{
    public IQueryable<QuestionCollection> Query => Queryable.AsNoTracking();

    /// <summary>
    /// 获取收藏试题列表的题型
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    public async Task<List<(string TypeId, string ParentTypeId)>> GetCollectionQuestionTypes(Guid subjectId)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();

        var queries = from qc in Query
                      join q1 in questionRepository.Query on qc.QuestionId equals q1.Id
                      join q2 in questionRepository.Query on q1.ParentId equals q2.Id into parent
                      from q2 in parent.DefaultIfEmpty()
                      where qc.CreatorId == userId && q1.SubjectId == subjectId
                      select new Tuple<string, string>(q1.QuestionTypeId.ToString(), q2.QuestionTypeId.ToString());

        var list = await queries.ToListAsync();
        return list.Select(x => (x.Item1, x.Item2)).ToList();
    }

    public async Task<List<QuestionCollection>> GetByQueryAsync(QuestionCollectionQuery query)
    {
        var linq = GetLinq();

        query.RecordCount = await linq.CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await linq.OrderByDescending(x => x.CreateTime).Skip(query.PageStart).Take(query.PageSize).ToListAsync();
        }

        return query.Result;

        IQueryable<QuestionCollection> GetLinq()
        {
            if (query.QuestionTypeId.HasValue)
            {
                if (query.QuestionTypeId == QuestionType.CaseAnalysis)
                {
                    return from qc in Query.AsNoTracking().Where(query.GetQueryWhere()).Include(x => x.Question)
                           join qn in questionRepository.Query on qc.Question.ParentId equals qn.Id into parent
                           from qn in parent.DefaultIfEmpty()
                           where qn.QuestionTypeId == query.QuestionTypeId.Value
                           select qc;
                }

                return Query.AsNoTracking().Include(x => x.Question).Where(query.GetQueryWhere()).Where(x => x.Question.QuestionTypeId == query.QuestionTypeId.Value);
            }

            return Query.AsNoTracking().Include(x => x.Question).Where(query.GetQueryWhere());
        }
    }

    public Task<bool> IsCollected(Guid questionId)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return Query.AnyAsync(x => x.QuestionId == questionId && x.CreatorId == userId);
    }
}