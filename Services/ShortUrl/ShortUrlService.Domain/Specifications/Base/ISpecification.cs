using System.Linq.Expressions;

namespace ShortUrlService.Domain.Specifications.Base;

// https://learn.microsoft.com/zh-cn/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core#implement-the-query-specification-pattern
public interface ISpecification<T> where T : class
{
    Expression<Func<T, bool>>? Criteria { get; }

    List<Expression<Func<T, object>>> Includes { get; }

    List<string> IncludeStrings { get; }

    Queue<(Expression<Func<T, object>>, bool)> OrderByQueue { get; }

    int Skip { get; }

    int Take { get; }

    bool EnalePaging { get; }
}

public abstract class BaseSpecification<T>(Expression<Func<T, bool>>? criteria = null) : ISpecification<T> where T : class
{
    public Expression<Func<T, bool>>? Criteria { get; private set; } = criteria;

    protected virtual void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public List<Expression<Func<T, object>>> Includes { get; } = [];

    protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    public List<string> IncludeStrings { get; } = [];

    // string-based includes allow for including children of children
    // e.g. Basket.Items.Product
    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    public Queue<(Expression<Func<T, object>>, bool)> OrderByQueue { get; private set; } = new();

    protected virtual void AddOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderByQueue.Enqueue((orderBy, true));
    }

    protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByQueue.Enqueue((orderByDescending, false));
    }

    public int Skip { get; private set; }

    public int Take { get; private set; }

    public bool EnalePaging { get; private set; }

    protected virtual void AddPaging(int skip, int take)
    {
        Skip = skip > 0 ? skip : throw new ArgumentOutOfRangeException(nameof(skip));
        Take = take > 0 ? take : throw new ArgumentOutOfRangeException(nameof(take));
        EnalePaging = true;
    }
}
