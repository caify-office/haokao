using HaoKao.CorrectionNotebookService.Domain.Dtos;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Enums;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.ValueObjects;

namespace HaoKao.CorrectionNotebookService.Domain.Repositories;

public interface IQuestionRepository : IRepository<Question, Guid>
{
    Task<Question> GetWithTagsAsync(Guid id);

    Task<QuestionPagedListDto> GetPagedListAsync(QuestionQuery query);

    Task<UserQuestionCountStatistics> GetQuestionCountStatisticsAsync(Guid userId);

    Task UpdateAsync(IReadOnlyList<Question> questions);

    Task UpdateMasteryDegreeAsync(IReadOnlyList<Guid> ids, MasteryDegree masteryDegree);

    void AddQuestionTags(IReadOnlyList<QuestionTag> tags);

    void DeleteQuestionTags(IReadOnlyList<QuestionTag> tags);

    Task DeleteAsync(IReadOnlyList<Guid> ids);
}