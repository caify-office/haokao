using HaoKao.CorrectionNotebookService.Application.ViewModels;
using HaoKao.CorrectionNotebookService.Domain.ValueObjects;

namespace HaoKao.CorrectionNotebookService.Application.Interfaces;

public interface IQuestionService : IAppWebApiService, IManager
{
    Task<QuestionListViewModel> GetList(QueryQuestionViewModel model);

    Task<QuestionViewModel> Get(Guid id);

    Task Create(CreateQuestionViewModel model);

    Task<GetAnswerAndAnalysisViewModel> GetAnswerAndAnalysis(Guid id);

    IAsyncEnumerable<string> GetAnswerAndAnalysisStream(Guid id);

    Task MasteryDegree(EditQuestionMasteryDegreeViewModel model);

    Task QuestionTag(EditQuestionTagViewModel model);

    Task Delete(Guid subjectId, IReadOnlyList<Guid> ids);

    Task<UserQuestionCountStatistics> GetUserQuestionCountStatistics();
}