using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Infrastructure.Repositories;

public class UserAnswerRecordRepository : Repository<UserAnswerRecord>, IUserAnswerRecordRepository
{
    public IQueryable<UserAnswerRecord> Query => Queryable.AsNoTracking();

    public Task<List<UserAnswerRecord>> GetLatestRecordListAsync(
        Expression<Func<UserAnswerRecord, bool>> wherePredicate,
        params string[] queryFields
    )
    {
        var query = from t in Query.Where(wherePredicate)
                    where t.CreateTime == (
                        from s in Query
                        where s.RecordIdentifier == t.RecordIdentifier &&
                              s.AnswerType == t.AnswerType &&
                              s.SubjectId == t.SubjectId &&
                              s.QuestionCategoryId == t.QuestionCategoryId &&
                              s.CreatorId == t.CreatorId
                        select s.CreateTime
                    ).Max()
                    orderby t.CreateTime descending
                    select t;

        return query.SelectProperties(queryFields).ToListAsync();
    }

    /// <summary>
    /// 读取当前用户的最新作答记录
    /// </summary>
    public Task<Guid> GetUserLatestRecordId(Guid userId)
    {
        return Query.Where(t => t.CreatorId == userId)
                    .OrderByDescending(p => p.CreateTime)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 按Id查询记录和详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<UserAnswerRecord> GetByIdIncludeDetail(Guid id)
    {
        return Query.Where(x => x.Id == id)
                    .Include(x => x.RecordQuestions)
                    .ThenInclude(y => y.QuestionOptions)
                    .FirstOrDefaultAsync();
    }

    /// <summary>
    /// 查询试卷作答参与人数
    /// </summary>
    public Task<Dictionary<Guid, int>> GetPaperAnswerCountDict(List<Guid> paperIds)
    {
        return Query.Where(t => paperIds.Contains(t.RecordIdentifier))
                    .GroupBy(t => t.RecordIdentifier)
                    .Select(x => new { PaperId = x.Key, Count = x.Count() })
                    .ToDictionaryAsync(t => t.PaperId, t => t.Count);
    }

    /// <summary>
    /// 查询用户本月打卡记录
    /// </summary>
    public async Task<List<DateTime>> GetPunchInRecordList(Guid subjectId, DateOnly date)
    {
        var tableName = EngineContext.Current
                                     .GetEntityShardingTableParameter<UserAnswerRecord>()
                                     .GetCurrentShardingTableName()
                                     .Replace($"_{DateTime.Now.Year}", $"_{date.Year}");

        // 判断一下表是否存在
        var tableNameList = await GetTableNameList();
        var table = tableNameList.FirstOrDefault(x => x.Equals(tableName, StringComparison.CurrentCultureIgnoreCase));
        if (string.IsNullOrEmpty(table)) return [];

        var sql = $"""
                   SELECT DISTINCT CreateTime FROM {table}
                   WHERE SubjectId = '{subjectId}'
                     AND AnswerType = {(int)SubmitAnswerType.Daily}
                     AND CreateTime >= @StartTime
                     AND CreateTime < @EndTime
                   """;

        if (EngineContext.Current.IsAuthenticated)
        {
            sql += $"\n  AND CreatorId = '{EngineContext.Current.ClaimManager.IdentityClaim.UserId}'";
        }

        var hashSet = new HashSet<DateTime>();
        var context = EngineContext.Current.Resolve<UserAnswerRecordDbContext>();
        await context.Database.OpenConnectionAsync();
        var connection = context.Database.GetDbConnection();
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.AddParameter("StartTime", date.ToString("yyyy-MM-dd"));
        command.AddParameter("EndTime", date.AddMonths(1).ToString("yyyy-MM-dd"));

        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            hashSet.Add(reader.GetDateTime("CreateTime").Date);
        }
        await context.Database.CloseConnectionAsync();
        return hashSet.ToList();
    }

    /// <summary>
    /// 查询用户每日打卡超过比例
    /// </summary>
    public async Task<decimal> GetPunchInRankingRatio(Expression<Func<UserAnswerRecord, bool>> wherePredicate)
    {
        var result = await Query.Where(wherePredicate)
                                .GroupBy(t => t.CreatorId)
                                .Select(g => new { g.Key, Value = g.Count() })
                                .OrderBy(t => t.Value)
                                .Select(g => new KeyValuePair<Guid, int>(g.Key, g.Value))
                                .ToListAsync();

        if (result.Count != 0)
        {
            //用户排名
            var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
            var index = result.FindIndex(x => x.Key.Equals(userId));

            if (index < 0)
            {
                //当前用户未打卡
                return 0.0M;
            }

            //处理打卡次数相同的情况
            if (index > 0)
            {
                var userPunchCount = result[index].Value;
                var lastPunchCount = result[index - 1].Value;
                while (userPunchCount == lastPunchCount && index > 0)
                {
                    index--;
                    if (index == 0) break;
                    lastPunchCount = result[index - 1].Value;
                }
            }


            //计算超过人数比例
            var fixPercentage = Convert.ToDecimal((double)index / result.Count) * 100;
            return Math.Round(fixPercentage, 1);
        }

        return 0.0M;
    }

    /// <summary>
    /// 每日一题打卡人数统计
    /// </summary>
    /// <returns></returns>
    public Task<int> GetJoinPunchInPeopleNumber(Guid subjectId)
    {
        return Query.Where(t => t.SubjectId == subjectId && t.AnswerType == SubmitAnswerType.Daily)
                    .Select(t => t.CreatorId).Distinct().CountAsync();
    }

    /// <summary>
    /// 获取试卷的答题总人数
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    public Task<Dictionary<Guid, int>> GetPaperUserAnswerRecordCount(Guid[] paperIds)
    {
        return Query
               .Where(x => x.AnswerType == SubmitAnswerType.Paper && paperIds.Contains(x.RecordIdentifier))
               .GroupBy(x => x.RecordIdentifier)
               .Select(g => new { g.Key, Count = g.Count() })
               .ToDictionaryAsync(g => g.Key, g => g.Count);
    }

    /// <summary>
    /// 获取当前用户的试卷得分
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    public Task<List<UserAnswerRecord>> GetCurrentUserPaperScore(Guid[] paperIds)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var maxDateQuery = from t in Query.Where(x => x.CreatorId == userId)
                           group t by t.RecordIdentifier into g
                           select new
                           {
                               RecordIdentifier = g.Key,
                               CreateTime = g.Max(x => x.CreateTime)
                           };

        var query = from t in Query
                    join s in maxDateQuery
                        on new { t.RecordIdentifier, t.CreateTime }
                        equals new { s.RecordIdentifier, s.CreateTime }
                    where paperIds.Contains(t.RecordIdentifier) &&
                          t.CreatorId == userId
                    select t;
        return query.ToListAsync();
    }

    /// <summary>
    /// 查询App首页章节试题统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    public async Task<DataTable> GetChapterRecordStat(Guid subjectId)
    {
        var tableName = EngineContext.Current.GetEntityShardingTableParameter<UserAnswerRecord>().GetCurrentShardingTableName();
        var userId = EngineContext.Current.ClaimManager.GetUserId();
        var sql = @$"
SELECT count(0)             AS ChapterAnswerCount,
       sum(t.AnswerCount)   AS AnswerCount,
       sum(t.CorrectCount)  AS CorrectCount,
       sum(t.QuestionCount) AS QuestionCount,
       sum(t.ElapsedTime)   AS ElapsedTime
FROM (SELECT RecordIdentifier,
             AnswerCount,
             CorrectCount,
             QuestionCount,
             ElapsedTime,
             ROW_NUMBER() OVER ( PARTITION BY RecordIdentifier, QuestionCategoryId ORDER BY CreateTime DESC ) AS rn
      FROM {tableName}
      WHERE AnswerType = 0
        AND CreatorId = '{userId}'
        AND SubjectId = '{subjectId}') AS t
WHERE rn = 1";
        var dbContext = EngineContext.Current.Resolve<UserAnswerRecordDbContext>();
        var connection = dbContext.Database.GetDbConnection();
        await dbContext.Database.OpenConnectionAsync();
        var adapter = new MySqlDataAdapter(sql, connection as MySqlConnection);
        var dt = new DataTable();
        adapter.Fill(dt);
        await dbContext.Database.CloseConnectionAsync();
        return dt;
    }

    /// <summary>
    /// 按科目查询今日任务的练习记录Id
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task<Guid> GetTodayTaskRecordId(Guid subject)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        return Queryable.Where(x => x.SubjectId == subject
                                 && x.CreatorId == userId
                                 && x.CreateTime >= today
                                 && x.CreateTime < tomorrow
                                 && x.AnswerType == SubmitAnswerType.TodayTask
        ).Select(x => x.Id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 查询数据库中的表
    /// </summary>
    /// <returns></returns>
    private static async Task<List<string>> GetTableNameList()
    {
        var context = EngineContext.Current.Resolve<UserAnswerRecordDbContext>();
        await context.Database.OpenConnectionAsync();
        var connection = context.Database.GetDbConnection();
        var command = connection.CreateCommand();
        const string tableName = nameof(UserAnswerRecord) + "_";
        command.CommandText = $@"
SELECT t.TABLE_NAME
FROM information_schema.`TABLES` t
WHERE t.TABLE_SCHEMA = @TableSchema
  AND LEFT(t.TABLE_NAME, {tableName.Length}) = @TableName";

        command.AddParameter("TableSchema", connection.Database);
        command.AddParameter("TableName", tableName);

        var tables = new List<string>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }
        await context.Database.CloseConnectionAsync();
        return tables;
    }

    #region 数据统计存储过程

    public Task<DataTable> QueryPracticeAbility(Guid subjectId)
    {
        return QueryStoredProcedure(subjectId, "Sp_PracticeAbility");
    }

    public Task<DataTable> QueryPracticeSpeed(Guid subjectId)
    {
        return QueryStoredProcedure(subjectId, "Sp_PracticeSpeed");
    }

    public Task<DataTable> QueryPracticeSituation(Guid subjectId)
    {
        return QueryStoredProcedure(subjectId, "Sp_PracticeSituation");
    }

    public Task<DataTable> QueryAccuracyRank(Guid subjectId)
    {
        return QueryStoredProcedure(subjectId, "Sp_AccuracyRank");
    }

    public Task<DataTable> QueryQuestionTypeAccuracy(Guid subjectId)
    {
        return QueryStoredProcedure(subjectId, "Sp_QuestionTypeAccuracy");
    }

    private static async Task<DataTable> QueryStoredProcedure(Guid subjectId, string spName)
    {
        // 模拟任务耗时
        // await Task.Delay(TimeSpan.FromSeconds(5));

        var context = EngineContext.Current.Resolve<UserAnswerRecordDbContext>();
        var database = context.Database;
        await database.OpenConnectionAsync();
        var connection = database.GetDbConnection();
        var command = connection.CreateCommand();

        var userId = EngineContext.Current.ClaimManager.GetUserId();
        // const string userId = "08db5b34-be54-4d73-8835-6347cc05842f";
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
        // const string tenantId = "08db5bf2-afae-4d40-8896-18e7e86b6b37";

        command.AddParameter("subjectId", subjectId);
        command.AddParameter("tenantId", tenantId);
        command.AddParameter("userId", userId);

        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = spName;

        var dataTable = new DataTable();
        var adapter = new MySqlDataAdapter((MySqlCommand)command);
        adapter.Fill(dataTable);
        await database.CloseConnectionAsync();
        return dataTable;
    }

    #endregion
}