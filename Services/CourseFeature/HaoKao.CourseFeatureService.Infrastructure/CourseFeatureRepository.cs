using HaoKao.CourseFeatureService.Domain;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HaoKao.CourseFeatureService.Infrastructure;

public class CourseFeatureRepository : Repository<CourseFeature>, ICourseFeatureRepository
{
    public IQueryable<CourseFeature> CourseFeatures => Queryable.AsNoTracking();

    public Task<int> ExecuteUpdateAsync(Expression<Func<CourseFeature, bool>> predicate, Expression<Func<SetPropertyCalls<CourseFeature>, SetPropertyCalls<CourseFeature>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }

}