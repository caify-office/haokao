using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Domain.QuestionWrongModule;

public interface IQuestionWrongRepository : IRepository<QuestionWrong>
{
    IQueryable<QuestionWrong> Query { get; }

    /// <summary>
    /// 获取章节列表和错题数量
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="subjectId"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IReadOnlyList<(Guid Id, string Name, int Count)>> GetChapterList(Guid userId, Guid subjectId, bool isActive);

    /// <summary>
    /// 获取章节下的错题列表
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="chapterId"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetChapterQuestionList(Guid userId, Guid chapterId, bool isActive);

    /// <summary>
    /// 获取今日任务的试题Id集合
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="extraction"></param>
    /// <returns></returns>
    Task<List<Guid>> GetTodayTaskQuestionIds(Guid subjectId, int extraction);
}