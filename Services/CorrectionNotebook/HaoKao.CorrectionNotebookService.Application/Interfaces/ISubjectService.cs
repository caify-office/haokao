using HaoKao.CorrectionNotebookService.Application.ViewModels;

namespace HaoKao.CorrectionNotebookService.Application.Interfaces;

public interface ISubjectService : IAppWebApiService, IManager
{
    Task<IReadOnlyList<SubjectListItemViewModel>> Get(Guid examLevelId);

    Task Create(CreateSubjectViewModel model);

    Task Resort(ResortSubjectViewModel model);

    Task Delete(Guid id);
}