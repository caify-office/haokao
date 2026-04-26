using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Queries;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IProductChapterAnswerRecordRepository : IRepository<ProductChapterAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<ProductChapterAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取章节作答记录Ids
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="chapterIds">试卷Ids</param>
    /// <returns></returns>
    Task<Dictionary<Guid, Guid>> GetChapterRecordIds(Guid userId, Guid productId, IReadOnlyList<Guid> chapterIds);

    /// <summary>
    /// 获取科目作答统计(做题数,正确数和总数)
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId);
}

public interface IProductPaperAnswerRecordRepository : IRepository<ProductPaperAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<ProductPaperAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取试卷作答记录Ids
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="paperIds">试卷Ids</param>
    /// <returns></returns>
    Task<Dictionary<Guid, Guid>> GetPaperRecordIds(Guid userId, Guid productId, IReadOnlyList<Guid> paperIds);

    /// <summary>
    /// 获取科目作答统计(做题数,正确数和总数)
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId);
}

public interface IProductKnowledgeAnswerRecordRepository : IRepository<ProductKnowledgeAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<ProductKnowledgeAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取章节知识点作答记录
    /// </summary>
    /// <param name="query">查询参数</param>
    /// <returns></returns>
    Task<ProductKnowledgeAnswerRecordQuery> GetKnowledgePointList(ProductKnowledgeAnswerRecordQuery query);

    /// <summary>
    /// 获取知识点掌握情况
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<Dictionary<MasteryLevel, int>> GetKnowledgeMasteryStat(ProductKnowledgeAnswerRecordQuery query);

    /// <summary>
    /// 获取科目作答统计(做题数,正确数和总数)
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<(int AnswerCount, int CorrectCount, int QuestionCount)> GetSubjectAnswerStat(Guid userId, Guid productId, Guid subjectId);

    /// <summary>
    /// 获取考试频率掌握情况
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="productId">产品Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<(ExamFrequency Frequency, MasteryLevel Mastery)>> GetExamFrequencyMastery(Guid userId, Guid productId, Guid subjectId);
}