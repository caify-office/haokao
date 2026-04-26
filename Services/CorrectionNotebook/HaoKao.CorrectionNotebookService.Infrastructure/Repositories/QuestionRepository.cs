using HaoKao.CorrectionNotebookService.Domain.Dtos;
using HaoKao.CorrectionNotebookService.Domain.Entities;
using HaoKao.CorrectionNotebookService.Domain.Enums;
using HaoKao.CorrectionNotebookService.Domain.Queries;
using HaoKao.CorrectionNotebookService.Domain.Repositories;
using HaoKao.CorrectionNotebookService.Domain.ValueObjects;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Repositories;

public class QuestionRepository(CorrectionNotebookDbContext dbContext) : Repository<Question, Guid>, IQuestionRepository
{
    public Task<Question> GetWithTagsAsync(Guid id)
    {
        return dbContext.Questions.Include(x => x.Tags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<QuestionPagedListDto> GetPagedListAsync(QuestionQuery query)
    {
        var count = await dbContext.Questions.CountAsync(query.Criteria);
        if (count == 0) return new(0, []);

        var list = await dbContext.Questions.Include(x => x.Tags).ThenInclude(x => x.Tag)
                                  .Where(query.Criteria)
                                  .OrderByDescending(query.OrderBy)
                                  .Skip(query.Skip)
                                  .Take(query.Take)
                                  .ToListAsync();
        return new(count, list);
    }

    public Task<UserQuestionCountStatistics> GetQuestionCountStatisticsAsync(Guid userId)
    {
        return dbContext.Questions.Where(x => x.CreatorId == userId)
                        .GroupBy(x => x.CreatorId)
                        .Select(x => new UserQuestionCountStatistics(
                                    x.Count(),
                                    x.Count(q => q.MasteryDegree == MasteryDegree.Mastered),
                                    x.Count() - x.Count(q => q.MasteryDegree == MasteryDegree.Mastered)
                                )).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(IReadOnlyList<Question> questions)
    {
        dbContext.Questions.UpdateRange(questions);
        return Task.CompletedTask;
    }

    public Task UpdateMasteryDegreeAsync(IReadOnlyList<Guid> ids, MasteryDegree masteryDegree)
    {
        return dbContext.Questions.Where(x => ids.Contains(x.Id)).ExecuteUpdateAsync(
            s => s.SetProperty(x => x.MasteryDegree, masteryDegree)
        );
    }

    public void AddQuestionTags(IReadOnlyList<QuestionTag> tags)
    {
        dbContext.QuestionTags.AddRange(tags);
    }

    public void DeleteQuestionTags(IReadOnlyList<QuestionTag> tags)
    {
        dbContext.QuestionTags.RemoveRange(tags);
    }

    public async Task DeleteAsync(IReadOnlyList<Guid> ids)
    {
        await dbContext.Questions.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();
        await dbContext.QuestionTags.Where(x => ids.Contains(x.QuestionId)).ExecuteDeleteAsync();
    }
}