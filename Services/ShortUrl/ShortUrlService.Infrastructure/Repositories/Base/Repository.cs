using ShortUrlService.Domain.Repositories.Base;
using ShortUrlService.Domain.Specifications.Base;
using ShortUrlService.Infrastructure.Extensions;

namespace ShortUrlService.Infrastructure.Repositories.Base;

public abstract class Repository<T, TKey>(DbContext context) : IRepository<T, TKey> where T : BaseEntity<TKey>
{
    private readonly DbSet<T> _set = context.Set<T>();

    public virtual async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _set.ToListAsync();
    }

    public virtual async Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> predicate)
    {
        return await _set.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _set.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T?> GetByIdAsync(TKey id)
    {
        return await _set.FindAsync(id);
    }

    public virtual async ValueTask AddAsync(T entity)
    {
        await _set.AddAsync(entity);
    }

    public virtual async ValueTask AddRangeAsync(IEnumerable<T> entities)
    {
        await _set.AddRangeAsync(entities);
    }

    public virtual async ValueTask DeleteAsync(T entity)
    {
        await Task.Run(() => _set.Remove(entity));
    }

    public virtual async ValueTask UpdateAsync(T entity)
    {
        await Task.Run(() => _set.Update(entity));
    }

    public virtual async Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification)
    {
        return await _set.SatisfiedBy(specification).ToListAsync();
    }

    public async Task<(int TotalCount, IReadOnlyList<T> Data)> GetPagedListAsync(int pageIndex, int pageSize)
    {
        var count = await _set.CountAsync();
        return count == 0
            ? (count, [])
            : (count, await _set.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync());
    }

    public virtual async Task<T?> GetAsync(ISpecification<T> specification)
    {
        return await _set.SatisfiedBy(specification).FirstOrDefaultAsync();
    }

    public virtual async Task<int> CountAsync(ISpecification<T> specification)
    {
        return await _set.SatisfiedBy(specification).CountAsync();
    }

    public virtual async Task<bool> AnyAsync(ISpecification<T> specification)
    {
        return await _set.SatisfiedBy(specification).AnyAsync();
    }
}