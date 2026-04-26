namespace ShortUrlService.Domain.Repositories.Base;

public interface IUnitOfWork : IManager
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}