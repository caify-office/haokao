using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IPaperAnswerRecordRepository : IRepository<PaperAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<PaperAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <param name="categoryId">类别Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<PaperAnswerRecord>> GetPaperList(Guid userId, Guid subjectId, Guid categoryId);

    /// <summary>
    /// 获取试卷列表
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="paperIds">试卷Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<PaperAnswerRecord>> GetPaperList(Guid userId, IReadOnlyList<Guid> paperIds);

    /// <summary>
    /// 查询每份试卷有多少用户作答过
    /// </summary>
    /// <param name="paperIds">试卷Id</param>
    /// <returns></returns>
    Task<Dictionary<Guid, int>> GetPaperUserCount(IReadOnlyList<Guid> paperIds);
}