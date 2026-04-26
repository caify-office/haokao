using HaoKao.QuestionService.Application.DailyQuestionModule.ViewModels;

namespace HaoKao.QuestionService.Application.DailyQuestionModule.Interfaces;

public interface IDailyQuestionWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据科目Id获取每日一题
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    Task<DailyQuestionViewModel> Get(Guid subjectId, DateTime dateTime);
}