using HaoKao.CorrectionNotebookService.Application.ViewModels;

namespace HaoKao.CorrectionNotebookService.Application.Interfaces;

public interface IExamLevelService : IAppWebApiService, IManager
{
    Task<IReadOnlyList<ExamLevelListItemViewModel>> Get();

    Task Create(CreateExamLevelViewModel model);

    Task EditName(EditExamLevelNameViewModel model);

    Task Delete(Guid id);
}