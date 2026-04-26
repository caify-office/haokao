using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;


public interface IDateAnswerRecordRepository : IRepository<DateAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<DateAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取今日任务记录
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<DateAnswerRecord> GetTodayTaskRecord(Guid userId, Guid subjectId);

    /// <summary>
    /// 获取每日打卡记录
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<DateAnswerRecord> GetDailyRecord(Guid userId, Guid subjectId);

    /// <summary>
    /// 获取每日打卡记录(月)
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    Task<IReadOnlyList<DateOnly>> GetDailyRecordList(Guid userId, Guid subjectId, DateOnly date);

    /// <summary>
    /// 获取每日打卡排名占比(月)
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <returns></returns>
    Task<decimal> GetDailyRankingRatio(Guid userId, Guid subjectId, DateOnly date);

    /// <summary>
    /// 每日一题打卡人数统计
    /// </summary>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<int> GetDailyUserCount(Guid subjectId);

    /// <summary>
    /// 判断用户是否已打卡
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <param name="date">日期</param>
    /// <param name="type">类别</param>
    /// <returns></returns>
    Task<bool> Exist(Guid userId, Guid subjectId, DateOnly date, SubmitAnswerType type);
}