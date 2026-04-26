using Girvs.BusinessBasis;

namespace HaoKao.QuestionService.Domain.Works;

public interface IUnionQuestionWork : IManager
{
    Task ExecuteAsync();
}

public interface IInitQuestionTitleWork : IManager
{
    Task ExecuteAsync();
}

public interface IInitQuestionCountWork : IManager
{
    Task ExecuteAsync();
}