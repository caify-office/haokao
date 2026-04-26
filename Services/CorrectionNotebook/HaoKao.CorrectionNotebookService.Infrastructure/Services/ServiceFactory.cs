using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public class ServiceFactory<T>(IEnumerable<T> services) : IServiceFactory<T> where T : IDomainService
{
    public T Create(Guid id)
    {
        return services.First(x => x.Id == id);
    }
}