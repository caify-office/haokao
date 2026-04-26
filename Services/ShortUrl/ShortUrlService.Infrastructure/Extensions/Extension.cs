using ShortUrlService.Domain.Specifications.Base;

namespace ShortUrlService.Infrastructure.Extensions;

public static partial class ListExtension
{
    private static readonly Random random = new();

    public static List<T> Shuffle<T>(this List<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }

        return list;
    }
}

public static partial class QueryableExtension
{
    public static IQueryable<T> SatisfiedBy<T>(this IQueryable<T> source, ISpecification<T> specification) where T : class
    {
        var query = source;

        // modify the IQueryable using the specification's criteria expression
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // Includes all expression-based includes
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Include any string-based include statements
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        if (specification.OrderByQueue.Count > 0)
        {
            var (sort, isAscending) = specification.OrderByQueue.Dequeue();
            IOrderedQueryable<T> orderedQuery = isAscending ? query.OrderBy(sort) : query.OrderByDescending(sort);

            while (specification.OrderByQueue.Count > 0)
            {
                (sort, isAscending) = specification.OrderByQueue.Dequeue();
                orderedQuery = isAscending ? orderedQuery.ThenBy(sort) : orderedQuery.ThenByDescending(sort);
            }

            query = orderedQuery;
        }

        if (specification.EnalePaging)
        {
            query = query.Skip((specification.Skip - 1) * specification.Take).Take(specification.Take);
        }

        return query;
    }
}