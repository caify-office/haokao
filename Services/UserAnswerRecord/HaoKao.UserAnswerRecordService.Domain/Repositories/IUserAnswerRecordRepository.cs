using HaoKao.UserAnswerRecordService.Domain.Entities;
using System.Data;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IUserAnswerRecordRepository : IRepository<UserAnswerRecord>
{
    IQueryable<UserAnswerRecord> Query { get; }

    Task<List<UserAnswerRecord>> GetLatestRecordListAsync(Expression<Func<UserAnswerRecord, bool>> wherePredicate, params string[] queryFields);

    /// <summary>
    /// 查询试卷作答参与人数
    /// </summary>
    Task<Dictionary<Guid, int>> GetPaperAnswerCountDict(List<Guid> paperIds);

    /// <summary>
    /// 读取当前用户的最新作答记录
    /// </summary>
    Task<Guid> GetUserLatestRecordId(Guid userId);

    /// <summary>
    /// 按Id查询记录和详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserAnswerRecord> GetByIdIncludeDetail(Guid id);

    /// <summary>
    /// 查询用户本月打卡记录
    /// </summary>
    Task<List<DateTime>> GetPunchInRecordList(Guid subjectId, DateOnly date);

    /// <summary>
    /// 查询用户每日打卡超过比例
    /// </summary>
    Task<decimal> GetPunchInRankingRatio(Expression<Func<UserAnswerRecord, bool>> wherePredicate);

    /// <summary>
    /// 每日一题打卡人数统计
    /// </summary>
    /// <returns></returns>
    Task<int> GetJoinPunchInPeopleNumber(Guid subjectId);

    /// <summary>
    /// 获取试卷的答题总人数
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, int>> GetPaperUserAnswerRecordCount(Guid[] paperIds);

    /// <summary>
    /// 获取当前用户的试卷得分
    /// </summary>
    /// <param name="paperIds"></param>
    /// <returns></returns>
    Task<List<UserAnswerRecord>> GetCurrentUserPaperScore(Guid[] paperIds);

    /// <summary>
    /// 查询App首页章节试题统计
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> GetChapterRecordStat(Guid subjectId);

    /// <summary>
    /// 按科目查询今日任务的练习记录Id
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Task<Guid> GetTodayTaskRecordId(Guid subject);

    /// <summary>
    /// 按科目统计练习的能力维度(存储过程实现)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> QueryPracticeAbility(Guid subjectId);

    /// <summary>
    /// 按科目统计练习速度(存储过程实现)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> QueryPracticeSpeed(Guid subjectId);

    /// <summary>
    /// 按科目统计练习状况(存储过程实现)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> QueryPracticeSituation(Guid subjectId);

    /// <summary>
    /// 按科目查询准确率排名(存储过程实现)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> QueryAccuracyRank(Guid subjectId);

    /// <summary>
    /// 按科目统计用户不同题型的准确率(存储过程实现)
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<DataTable> QueryQuestionTypeAccuracy(Guid subjectId);
}

public static class PracticeAbilityDimension
{
    /// <summary>
    /// 计算能力
    /// </summary>
    public const string CorrectNumeracy = "d67222e1-539c-405a-bba7-442e0b517f5a";

    /// <summary>
    /// 记忆能力
    /// </summary>
    public const string CorrectMemory = "d67222e1-539c-405a-bba7-442e0b517f5b";

    /// <summary>
    /// 理解能力
    /// </summary>
    public const string CorrectUnderstanding = "d67222e1-539c-405a-bba7-442e0b517f5c";

    /// <summary>
    /// 判断能力
    /// </summary>
    public const string CorrectJudgment = "d67222e1-539c-405a-bba7-442e0b517f5d";

    /// <summary>
    /// 分析能力
    /// </summary>
    public const string CorrectAnalytical = "d67222e1-539c-405a-bba7-442e0b517f5f";

    /// <summary>
    /// 练习速度
    /// </summary>
    public const string PracticeSpeed = "08dbcee9-2047-4f57-809f-2129bedadb2c";
}