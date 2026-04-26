using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels;
using QueryChapterQuestionViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterQuestionViewModel;
using QueryChapterViewModel = HaoKao.QuestionService.Application.QuestionWrongModule.ViewModels.QueryChapterViewModel;

namespace HaoKao.QuestionService.Application.QuestionWrongModule.Interfaces;

public interface IQuestionWrongWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据科目和分类获取章节列表和题目数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ChapterViewModel>> GetChapterList(QueryChapterViewModel input);

    /// <summary>
    /// 获取章节试题Id, 并根据题型进行分组
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetChapterQuestionIdGroup(QueryChapterQuestionViewModel input);

    /// <summary>
    /// 根据科目判断用户是否存在未消灭的错题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<bool> Any(Guid subjectId);

    /// <summary>
    /// 今日任务抽题
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, List<QueryQuestionListAppViewModel>>> GetTodayTask(QueryTodayTaskViewModel input);

    /// <summary>
    /// 根据科目查询错题组卷列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<QueryMakingPaperViewModel> GetMakingPaper(QueryMakingPaperViewModel input);
}