using ShortUrlService.Domain.Repositories.Base;

namespace ShortUrlService.Infrastructure.Repositories.Base;

public class UnitOfWork(ShortUrlDbContext context) : IUnitOfWork
{
    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}