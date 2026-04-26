using Microsoft.Extensions.DependencyInjection;
using HaoKao.Common.Extensions;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using DotLiquid.Tags;
using HaoKao.CourseService.Domain.CourseChapterModule;
using HaoKao.CourseService.Domain.CourseModule;
using HaoKao.CourseService.Domain.CourseVideoModule;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

namespace HaoKao.CourseService.Infrastructure.Repositories;

public class CourseVideoRepository(CourseDbContext context) : Repository<CourseVideo>, ICourseVideoRepository
{
    public IQueryable<CourseVideo> CourseVideos => Queryable.AsNoTracking();

    public Task<int> VideoCount(Guid courseId)
    {
        var query = from cv in context.CourseVideos
                    join cc in context.CourseChapters
                        on cv.CourseChapterId equals cc.Id into CourseChapter
                    from cc in CourseChapter.DefaultIfEmpty()
                    where cc.CourseId == courseId
                    select cv;
        return query.CountAsync();
    }

    public override Task<List<CourseVideo>> GetAllAsync(params string[] fields)
    {
        return context.CourseVideos.ToListAsync();
    }

    public async Task<List<UpdateVideoModel>> GetUpdateVideoList(string courseIds)
    {
        var courseTableName = EngineContext.Current.GetEntityShardingTableParameter<Course>().GetCurrentShardingTableName();
        var courseVideoTableName = EngineContext.Current.GetEntityShardingTableParameter<CourseVideo>().GetCurrentShardingTableName();
        var courseChapterTableName = EngineContext.Current.GetEntityShardingTableParameter<CourseChapter>().GetCurrentShardingTableName();
        var sql = $@"SELECT a.videoname, DATE_FORMAT(a.createtime,'%Y-%m-%d') AS createtime, c.name AS coursename,c.id as courseid
FROM {courseVideoTableName} AS  a
LEFT JOIN {courseChapterTableName} AS b ON a.CourseChapterId=b.id
LEFT JOIN `{courseTableName}` AS c ON b.courseid= c.id
where c.id in ('{courseIds.Replace(",", "','")}')
GROUP BY createtime, c.name, a.videoname, c.id ";
        return await context.Database.SqlQueryRaw<UpdateVideoModel>(sql).ToListAsync();
    }

    public async Task<List<UpdateVideoModel>> GetUpdateAssistantVideoList(string courseIds)
    {
        var result = from video in Queryable.Where(x => courseIds.Contains(x.CourseChapterId.ToString()))
                     from course in context.Courses.Where(x => x.Id == video.CourseChapterId)
                     select new UpdateVideoModel
                     {
                         videoname=video.VideoName,
                         createtime=DateOnly.FromDateTime(video.CreateTime).ToString("yyyy-MM-dd"),
                         coursename=course.Name,
                         courseid=course.Id
                     };
        return await result.ToListAsync();
    }

    public override async Task<List<CourseVideo>> GetByQueryAsync(QueryBase<CourseVideo> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();

        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere())
                               .SelectProperties(query.QueryFields)
                               .OrderBy(x => x.Sort)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public async Task AutoMigrationAllTenantId()
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        var tables = await GetTableNames();
        foreach (var table in tables)
        {
            table.SetTenantId();
            await using var tenantScope = _serviceProvider.CreateAsyncScope();
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<CourseDbContext>();
            tenantDbContext.ShardingAutoMigration();
        }
    }
    public async Task<IReadOnlyList<string>> GetTableNames()
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        await using var tenantScope = _serviceProvider.CreateAsyncScope();
        await using var dbContext = tenantScope.ServiceProvider.GetRequiredService<CourseDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(CourseVideo));
        return tables;
    }

    public async Task UpdateBatchAsync(Dictionary<Guid, CourseVideo> courseVideos)
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        await using var tenantScope1 = _serviceProvider.CreateAsyncScope();
        await using var dbContext = tenantScope1.ServiceProvider.GetRequiredService<CourseDbContext>();
        var courseVideoIds = courseVideos.Keys.ToList();
        var updateCourseVideos = dbContext.CourseVideos.Where(x => courseVideoIds.Contains(x.Id));
        foreach (var courseVideo in updateCourseVideos)
        {

            var target = courseVideos[courseVideo.Id];
            if (target != null)
            {
                courseVideo.DisplayName = courseVideo.VideoName;
                courseVideo.CateId = target.CateId;
                courseVideo.CateName = target.CateName;
                courseVideo.Tags = target.Tags;
            }
        }
        dbContext.SaveChanges();
    }

    public override Task UpdateRangeAsync(List<CourseVideo> ts, params string[] fields)
    {
        return context.SaveChangesAsync();
    }

    public async Task<List<CourseVideo>> GetWhere(Expression<Func<CourseVideo, bool>> expression)
    {
        var _serviceProvider = EngineContext.Current.Resolve<IServiceProvider>();
        await using var tenantScope1 = _serviceProvider.CreateAsyncScope();
        await using var dbContext = tenantScope1.ServiceProvider.GetRequiredService<CourseDbContext>();
        return await dbContext.CourseVideos.Where(expression).ToListAsync();
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<CourseVideo, bool>> predicate, Expression<Func<SetPropertyCalls<CourseVideo>, SetPropertyCalls<CourseVideo>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }
}