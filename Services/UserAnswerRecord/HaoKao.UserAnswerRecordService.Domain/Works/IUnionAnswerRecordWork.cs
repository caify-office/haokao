using Girvs.BusinessBasis;

namespace HaoKao.UserAnswerRecordService.Domain.Works;

public interface IUnionAnswerRecordWork : IManager
{
    Task ExecuteAsync();
}

public interface IMigrateRecordDataWork : IManager
{
    Task ExecuteAsync();
}