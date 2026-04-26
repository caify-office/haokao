using Girvs.BusinessBasis;

namespace HaoKao.QuestionService.Domain.Works;

public interface ICleanDuplicateQuestionWrongWork : IManager
{
    Task ExecuteAsync();
}

public interface IInitQuestionWrongSortWork : IManager
{
    Task ExecuteAsync();
}

public interface IFixQuestionWrongTypeIdWork : IManager
{
    Task ExecuteAsync();
}