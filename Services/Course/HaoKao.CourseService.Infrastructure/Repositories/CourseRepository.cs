using HaoKao.Common.Extensions;
using HaoKao.CourseService.Domain.CourseModule;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace HaoKao.CourseService.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public IQueryable<Course> Courses => Queryable.AsNoTracking();

    public override async Task<List<Course>> GetByQueryAsync(QueryBase<Course> query)
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
                               .OrderByDescending(x => x.CreateTime)
                               .Skip(query.PageStart)
                               .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<Course, bool>> predicate, Expression<Func<SetPropertyCalls<Course>, SetPropertyCalls<Course>>> setPropertyCalls)
    {
        return Queryable.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
    }

    public async Task MergeChapterAndVideoDo()
    {
        var provider = EngineContext.Current.Resolve<IServiceProvider>();

        //先获取合成所有的进度表并拿到结果
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<CourseDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(Course));

        foreach (var table in tables)
        {
            table.SetTenantId();
            await using var tenantScope = provider.CreateAsyncScope();
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<CourseDbContext>();
            tenantDbContext.ShardingAutoMigration();
        }

        var database = dbContext.Database;
        await database.ExecuteSqlRawAsync("Call Sp_MergeChapter() ");
        await database.ExecuteSqlRawAsync("Call Sp_MergeVideo() ");
    }
}