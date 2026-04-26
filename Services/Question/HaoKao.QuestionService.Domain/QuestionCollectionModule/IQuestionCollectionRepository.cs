namespace HaoKao.QuestionService.Domain.QuestionCollectionModule;

public interface IQuestionCollectionRepository : IRepository<QuestionCollection>
{
    IQueryable<QuestionCollection> Query { get; }

    Task<List<(string TypeId, string ParentTypeId)>> GetCollectionQuestionTypes(Guid subjectId);

    Task<List<QuestionCollection>> GetByQueryAsync(QuestionCollectionQuery query);

    public Task<bool> IsCollected(Guid questionId);
}