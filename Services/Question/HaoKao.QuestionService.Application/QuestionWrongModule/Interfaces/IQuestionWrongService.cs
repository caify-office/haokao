using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionModule;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;

public interface IQuestionWrongService : IManager, IAppWebApiService
{
    /// <summary>
    /// 根据科目获取章节列表和错题数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetChapterList(QueryChapterViewModel input);

    /// <summary>
    /// 获取章节下的所有试题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Question>> GetChapterQuestionList(QueryChapterQuestionViewModel input);

    /// <summary>
    /// 根据科目判断用户是否存在未消灭的错题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<bool> Any(Guid subjectId);

    /// <summary>
    /// 根据科目获取今日任务的试题Ids
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<Guid>> GetTodayTaskQuestionIds(QueryTodayTaskViewModel input);

    /// <summary>
    /// 获取错题数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Dictionary<string, int>> GetQuestionWrongCount(Guid subjectId, Guid userId);
}