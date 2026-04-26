using HaoKao.Common.Enums;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.QuestionWrongModule;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HaoKao.QuestionService.Infrastructure.Repositories;

public class QuestionRepository(QuestionDbContext dbContext) : Repository<Question>, IQuestionRepository
{
    public IQueryable<Question> Query => Queryable.AsNoTracking();

    #region Management

    public Task<int> ExecuteUpdateAsync(Expression<Func<Question, bool>> predicate, Expression<Func<SetPropertyCalls<Question>, SetPropertyCalls<Question>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }

    public Task<Dictionary<Guid, Question>> GetQuestionDictByIds(IEnumerable<Guid> ids)
    {
        return Query.Where(x => ids.Contains(x.Id))
                    .Select(x => new Question
                    {
                        Id = x.Id,
                        SubjectId = x.SubjectId,
                        SubjectName = x.SubjectName,
                        ChapterId = x.ChapterId,
                        ChapterName = x.ChapterName,
                        SectionId = x.SectionId,
                        SectionName = x.SectionName,
                        KnowledgePointId = x.KnowledgePointId,
                        KnowledgePointName = x.KnowledgePointName,
                        QuestionCategoryId = x.QuestionCategoryId,
                        QuestionCategoryName = x.QuestionCategoryName,
                        QuestionTypeId = x.QuestionTypeId,
                        QuestionTypeName = x.QuestionTypeName,
                        QuestionOptions = x.QuestionOptions,
                        ParentId = x.ParentId,
                    })
                    .ToDictionaryAsync(x => x.Id, x => x);
    }

    public Task<string> GetQuestionOptionsByIdAsync(Guid id)
    {
        return Query.Where(x => x.Id.Equals(id)).Select(x => x.QuestionOptions).FirstOrDefaultAsync();
    }

    public Task<List<Question>> GetPaperSubQuestionListByParentIdsAsync(IEnumerable<Guid> parentIds)
    {
        return Query.Where(x => parentIds.Contains(x.ParentId.Value))
                    .Select(x => new Question
                    {
                        Id = x.Id,
                        QuestionText = x.QuestionText,
                        QuestionTypeId = x.QuestionTypeId,
                        ParentId = x.ParentId,
                    }).ToListAsync();
    }

    #endregion

    #region App & WebSite

    /// <inheritdoc />
    public Task<int> GetSubjectQuestionCount(Guid subjectId, IReadOnlyList<Guid> categories)
    {
        var query = Query.Where(x => x.EnableState == EnableState.Enable && x.SubjectId == subjectId);
        if (categories?.Count > 0)
        {
            query = query.Where(x => categories.Contains(x.QuestionCategoryId));
        }
        return query.SumAsync(x => x.QuestionCount);
    }

    public Task<int> GetChaperCategorieQuestionCount(Guid chaperId, Guid questionCategoryId)
    {
        var count = Query.Where(x => x.EnableState == EnableState.Enable
                                  && x.ChapterId == chaperId
                                  && x.QuestionCategoryId == questionCategoryId)
                         .CountAsync();
        return count;
    }

    /// <summary>
    /// 根据科目代码抽取每日一题
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <param name="questionTypeIds">题型列表</param>
    /// <returns></returns>
    public Task<Question> ExtractDailyQuestionBySubjectId(Guid subjectId, params Guid[] questionTypeIds)
    {
        var query = dbContext.DailyQuestions.AsNoTracking()
                             .Where(x => x.SubjectId == subjectId)
                             .Select(x => x.QuestionId);
        return dbContext.Questions.AsNoTracking()
                        .Where(x => x.SubjectId == subjectId
                                 && x.EnableState == EnableState.Enable
                                 && questionTypeIds.Contains(x.QuestionTypeId)
                                 && !query.Any(i => i == x.Id))
                        .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 根据Ids获取试题列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<Question>> GetQuestionListByIds(IReadOnlyList<Guid> ids)
    {
        var linq = from t in Query
                   where ids.Contains(t.Id)
                      && t.EnableState == EnableState.Enable
                   select t;
        return GetListUnionChildren(linq);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetChapterList(Guid subjectId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.SubjectId == subjectId
                      && t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   group t by new { t.ChapterId, t.ChapterName } into g
                   select new
                   {
                       Id = g.Key.ChapterId,
                       Name = g.Key.ChapterName,
                       Count = g.Sum(x => x.QuestionCount)
                   };
        var list = await linq.ToListAsync();
        return list.Where(x => x.Id != Guid.Empty).Select(x => (x.Id, x.Name, x.Count)).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetSectionList(Guid chapterId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.ChapterId == chapterId
                      && t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   group t by new { t.SectionId, t.SectionName } into g
                   select new
                   {
                       Id = g.Key.SectionId,
                       Name = g.Key.SectionName,
                       Count = g.Sum(x => x.QuestionCount)
                   };
        var list = await linq.ToListAsync();
        return list.Where(x => x.Id != Guid.Empty).Select(x => (x.Id, x.Name, x.Count)).ToList();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetKnowledgePointList(Guid sectionId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.SectionId == sectionId
                      && t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   group t by new { t.KnowledgePointId, t.KnowledgePointName } into g
                   select new
                   {
                       Id = g.Key.KnowledgePointId,
                       Name = g.Key.KnowledgePointName,
                       Count = g.Sum(x => x.QuestionCount)
                   };
        var list = await linq.ToListAsync();
        return list.Where(x => x.Id != Guid.Empty).Select(x => (x.Id, x.Name, x.Count)).ToList();
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<Question>> GetChapterQuestions(Guid chapterId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && t.ChapterId == chapterId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   select t;
        return GetListUnionChildren(linq);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<Question>> GetSectionQuestions(Guid sectionId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && t.SectionId == sectionId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   select t;
        return GetListUnionChildren(linq);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<Question>> GetKnowledgePointQuestions(Guid knowledgePointId, Guid categoryId, FreeState? freeState)
    {
        var linq = from t in Query
                   where t.EnableState == EnableState.Enable
                      && t.QuestionCategoryId == categoryId
                      && t.KnowledgePointId == knowledgePointId
                      && (!freeState.HasValue || t.FreeState == freeState.Value)
                   select t;
        return GetListUnionChildren(linq);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Question>> GetListUnionChildren(IQueryable<Question> linq)
    {
        // 查询子题目
        var children = from t in Query
                       where linq.Any(q => q.ParentId == t.Id || q.Id == t.ParentId)
                       select t;
        // 合并查询结果
        return await linq.Union(children).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Guid>> GetSpecialPromotionQuestionIds(Guid subjectId, string ability, int count, bool trial)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        // 试用: 从用户已选科目中抽题，抽题范围不受权限限制
        if (trial)
        {
            if (count > 20) throw new GirvsException("专项提升试用一次不能超过20道试题");

            return await Query.Where(x => x.SubjectId == subjectId)
                              .Where(x => x.AbilityIds.Contains(ability))
                              .Where(x => x.QuestionTypeId != QuestionType.CaseAnalysis)
                              .OrderBy(_ => EF.Functions.Random())
                              .Select(x => x.Id)
                              .Take(count)
                              .ToListAsync();
        }

        // 正式使用: 从用户已选中的、有权限的科目和模块中抽题。
        // 抽题算法: 默认错题60%，新题40%。数量不足相互弥补；仍无法弥补时则忽略。
        // 例如，用户设置了抽题上限为10，她的错题有3道，未做过的新题5道，按规则只能抽取8道题

        var wrongCount = Convert.ToInt32(count * 0.6);
        var newCount = Convert.ToInt32(count * 0.4);

        // 错题的Ids
        var wrongIds = await EngineContext.Current.Resolve<IQuestionWrongRepository>().Query.Include(x => x.Question)
                                          .Where(x => x.IsActive && x.CreatorId == userId)
                                          .Where(x => x.Question.SubjectId == subjectId)
                                          .Where(x => x.Question.AbilityIds.Contains(ability))
                                          .Select(x => x.QuestionId).ToListAsync();

        wrongIds = wrongIds.OrderBy(_ => Guid.NewGuid()).ToList();
        var questions = await GetPermittedQuestions(subjectId, ability);
        var questionIds = questions.Keys.ToList();
        var records = await GetUserAnsweredQuestionIds(subjectId);

        // 新题的Ids
        var newIds = questionIds.Where(x => !records.Contains(x)).OrderBy(_ => Guid.NewGuid()).ToList();

        // 数量不足且无法互补则忽略
        if (wrongIds.Count + newIds.Count < count)
        {
            return wrongIds.Take(wrongCount).Concat(newIds.Take(newCount)).ToList();
        }

        // 数量充足且可以互补
        var ids = new List<Guid>(count);
        ids.AddRange(
            wrongIds.Count < wrongCount
                ? wrongIds.Concat(newIds.TakeLast(wrongCount - wrongIds.Count))
                : wrongIds.Take(wrongCount)
        );
        ids.AddRange(
            newIds.Count < newCount
                ? newIds.Concat(wrongIds.TakeLast(newCount - newIds.Count))
                : newIds.Take(newCount)
        );
        return ids;
    }

    /// <summary>
    /// 按科目获取近30天内用户已答试题的Id集合
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    private async Task<List<Guid>> GetUserAnsweredQuestionIds(Guid subjectId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();

        var database = dbContext.Database;
        await database.OpenConnectionAsync();

        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = GetQueryString();

        var list = new List<Guid>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(reader.GetGuid(0));
        }
        await database.CloseConnectionAsync();

        return list;

        string GetQueryString()
        {
            // 答题记录按年分表
            var suffix = $"{tenantId.Replace("-", "")}_{DateTime.Today.Year}";
            var tableSchema = connection.Database.Replace("Question", "UserAnswerRecord");

            command.AddParameter("SubjectId", subjectId);
            command.AddParameter("UserId", userId);
            command.AddParameter("Start", DateTime.Today.AddDays(-30));
            command.AddParameter("End", DateTime.Today.AddDays(1));

            return $"""
                    SELECT DISTINCT t1.QuestionId
                    FROM {tableSchema}.UserAnswerRecord_{suffix} t0
                    JOIN {tableSchema}.UserAnswerQuestion_{suffix} t1 ON t0.Id = t1.UserAnswerRecordId
                    WHERE t1.JudgeResult != {(int)ScoringRuleType.Missing}
                      AND t0.SubjectId = @SubjectId
                      AND t0.CreatorId = @UserId
                      AND t0.CreateTime >= @Start
                      AND t0.CreateTime < @End
                    """;
        }
    }

    /// <summary>
    /// 根据科目查询用户拥有的题库权限的试题Id和能力维度集合
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="ability"></param>
    /// <returns></returns>
    private async Task<Dictionary<Guid, string[]>> GetPermittedQuestions(Guid subjectId, string ability)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId();
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();

        var database = dbContext.Database;
        await database.OpenConnectionAsync();

        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = GetQueryString();
        command.AddParameter("SubjectId", subjectId);
        command.AddParameter("UserId", userId);
        command.AddParameter("TenantId", tenantId);
        command.AddParameter("QuestionType", QuestionType.CaseAnalysis);
        command.AddParameter("Ability", $"%{ability}%");

        var dict = new Dictionary<Guid, string[]>();
        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var id = reader.GetGuid(0);
            var str = reader.IsDBNull(1) ? "" : reader.GetString(1);
            var value = string.IsNullOrEmpty(str) ? [] : str.Split(',');
            dict.TryAdd(id, value);
        }
        await database.CloseConnectionAsync();

        return dict;

        string GetQueryString()
        {
            var suffix = tenantId.Replace("-", "");
            var questionSchema = TableSchema("Question");
            var productSchema = TableSchema("Product");

            return $"""
                    SELECT Id, AbilityIds
                    FROM {questionSchema}.Question_{suffix}
                    WHERE EnableState = 1
                      AND SubjectId = @SubjectId
                      AND QuestionTypeId != @QuestionType
                      AND AbilityIds LIKE @Ability
                      AND QuestionCategoryId IN(
                        SELECT pp.PermissionId
                        FROM {productSchema}.StudentPermission sp
                        JOIN {productSchema}.Product p ON sp.ProductId = p.Id
                        JOIN {productSchema}.ProductPermission pp ON pp.ProductId = p.Id
                        WHERE sp.ProductType = 0
                          AND sp.Enable = 1
                          AND sp.ExpiryTime >= now()
                          AND sp.TenantId = @TenantId
                          AND sp.StudentId = @UserId
                          AND pp.SubjectId = @SubjectId
                    )
                    """;
        }

        string TableSchema(string schema)
        {
            return connection.Database.Replace("Question", schema);
        }
    }

    #endregion
}