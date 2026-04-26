using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Domain.Repositories;

public interface IChapterAnswerRecordRepository : IRepository<ChapterAnswerRecord>
{
    /// <summary>
    /// 按Id获取记录详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<ChapterAnswerRecord> GetById(Guid id);

    /// <summary>
    /// 获取章节记录
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    Task<AnswerRecord> GetByChapterId(Guid userId, Guid categoryId, Guid chapterId);

    /// <summary>
    /// 获取小节记录
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    Task<AnswerRecord> GetBySectionId(Guid userId, Guid categoryId, Guid sectionId);

    /// <summary>
    /// 获取知识点记录
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">分类Id</param>
    /// <param name="knowledgePointId">知识点Id</param>
    /// <returns></returns>
    Task<AnswerRecord> GetByKnowledgePointId(Guid userId, Guid categoryId, Guid knowledgePointId);

    /// <summary>
    /// 获取章节列表
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">类别Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterAnswerRecord>> GetChapterList(Guid userId, Guid categoryId, Guid subjectId);

    /// <summary>
    /// 获取小节列表
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">类别Id</param>
    /// <param name="chapterId">章节Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterAnswerRecord>> GetSectionList(Guid userId, Guid categoryId, Guid chapterId);

    /// <summary>
    /// 获取知识点列表
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="categoryId">类别Id</param>
    /// <param name="sectionId">小节Id</param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterAnswerRecord>> GetKnowledgePointList(Guid userId, Guid categoryId, Guid sectionId);

    /// <summary>
    /// 查询用户在当前科目下的章节练习统计信息
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="subjectId">科目Id</param>
    /// <returns></returns>
    Task<ChapterRecordStat> GetChapterRecordStat(Guid userId, Guid subjectId);
}