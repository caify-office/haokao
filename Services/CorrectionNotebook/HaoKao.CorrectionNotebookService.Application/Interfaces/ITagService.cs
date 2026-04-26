using HaoKao.CorrectionNotebookService.Application.ViewModels;

namespace HaoKao.CorrectionNotebookService.Application.Interfaces;

public interface ITagService : IAppWebApiService, IManager
{
    Task<IReadOnlyList<TagCategoryViewModel>> Get();

    Task Create(CreateTagViewModel model);

    Task Delete(Guid id);
}