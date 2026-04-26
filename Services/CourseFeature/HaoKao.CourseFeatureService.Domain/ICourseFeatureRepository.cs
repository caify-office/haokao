using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.CourseFeatureService.Domain;

public interface ICourseFeatureRepository : IRepository<CourseFeature>
{
    IQueryable<CourseFeature> CourseFeatures { get; }

    Task<int> ExecuteUpdateAsync(Expression<Func<CourseFeature, bool>> predicate, Expression<Func<SetPropertyCalls<CourseFeature>, SetPropertyCalls<CourseFeature>>> setPropertyCalls);
}