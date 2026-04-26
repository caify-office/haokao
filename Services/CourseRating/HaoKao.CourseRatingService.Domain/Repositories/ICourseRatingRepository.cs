using HaoKao.CourseRatingService.Domain.Entities;

namespace HaoKao.CourseRatingService.Domain.Repositories;

public interface ICourseRatingRepository : IRepository<CourseRating>
{
    Task<List<CourseRating>> GetCourseRatingForWeb(QueryBase<CourseRating> query);
}