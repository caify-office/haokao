using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.CourseService.Domain.CourseModule;

public interface ICourseRepository : IRepository<Course>
{
    IQueryable<Course> Courses { get; }

    /// <summary>
    /// 合并课程章节和视频
    /// </summary>
    /// <returns></returns>
    Task MergeChapterAndVideoDo();

    Task<int> ExecuteUpdateAsync(Expression<Func<Course, bool>> predicate, Expression<Func<SetPropertyCalls<Course>, SetPropertyCalls<Course>>> setPropertyCalls);
}