using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Domain.Repositories;

namespace HaoKao.CourseRatingService.Infrastructure.Repositories;

public class CourseRatingRepository : Repository<CourseRating>, ICourseRatingRepository
{
    public async Task<List<CourseRating>> GetCourseRatingForWeb(QueryBase<CourseRating> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await Queryable.Where(query.GetQueryWhere())
                                          .SelectProperties(query.QueryFields)
                                          .OrderByDescending(x => x.Sticky)
                                          .ThenByDescending(x => x.CreateTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }

        return query.Result;
    }
}