using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IElapsedTimeRecordRepository : IRepository<ElapsedTimeRecord>
{
    /// <summary>
    /// 获取用户章节做题时间
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<long> GetChapterElapsedTime(Guid userId, Guid subjectId);

    /// <summary>
    /// 按日期统计做题时长
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <returns></returns>
    Task<IReadOnlyList<(DateOnly Date, long TotalSeconds)>> GetDateElaspedTime(Guid userId, Guid productId, Guid subjectId, DateOnly startDate, DateOnly endDate);
}