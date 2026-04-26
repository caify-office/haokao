using Girvs.BusinessBasis.Entities;
using ShortUrlService.Domain.Specifications.Base;
using System.Linq.Expressions;

namespace ShortUrlService.Domain.Repositories.Base;

public interface IRepository<T, in TKey> where T : BaseEntity<TKey>
{
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<IReadOnlyList<T>> GetListAsync(Expression<Func<T, bool>> predicate);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    Task<T?> GetByIdAsync(TKey id);

    ValueTask AddAsync(T entity);

    ValueTask AddRangeAsync(IEnumerable<T> entities);

    ValueTask UpdateAsync(T entity);

    ValueTask DeleteAsync(T entity);

    Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification);

    Task<(int TotalCount, IReadOnlyList<T> Data)> GetPagedListAsync(int pageIndex, int pageSize);

    Task<T?> GetAsync(ISpecification<T> specification);

    Task<int> CountAsync(ISpecification<T> specification);

    Task<bool> AnyAsync(ISpecification<T> specification);
}