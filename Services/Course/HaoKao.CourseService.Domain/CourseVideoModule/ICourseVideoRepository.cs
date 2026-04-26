using Microsoft.EntityFrameworkCore.Query;

namespace HaoKao.CourseService.Domain.CourseVideoModule;

public interface ICourseVideoRepository : IRepository<CourseVideo>
{
    IQueryable<CourseVideo> CourseVideos { get; }

    Task<int> VideoCount(Guid courseId);

    Task<List<UpdateVideoModel>> GetUpdateVideoList(string courseIds);

    Task<List<UpdateVideoModel>> GetUpdateAssistantVideoList(string courseIds);

    Task AutoMigrationAllTenantId();

    Task<IReadOnlyList<string>> GetTableNames();

    Task UpdateBatchAsync(Dictionary<Guid, CourseVideo> courseVideos);

    Task<List<CourseVideo>> GetWhere(Expression<Func<CourseVideo, bool>> expression);

    Task<int> ExecuteUpdateAsync(Expression<Func<CourseVideo, bool>> predicate, Expression<Func<SetPropertyCalls<CourseVideo>, SetPropertyCalls<CourseVideo>>> setPropertyCalls);
}