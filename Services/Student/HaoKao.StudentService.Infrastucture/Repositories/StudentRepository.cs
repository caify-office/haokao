using Girvs.BusinessBasis.Queries;
using HaoKao.StudentService.Domain.Repositories;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;

namespace HaoKao.StudentService.Infrastructure.Repositories;

public class StudentRepository(StudentDbContext dbContext) : Repository<Student>, IStudentRepository
{
    public Task<Student> GetIncludeAsync(Expression<Func<Student, bool>> predicate)
    {
        return dbContext.Students.AsNoTracking()
                        .Include(x => x.RegisterUser)
                        .Include(x => x.StudentFollows)
                        .FirstOrDefaultAsync(predicate);
    }

    public async Task<StudentQuery> GetByStudentQueryAsync(StudentQuery query)
    {
        var users = await dbContext.Students.Include(x => x.RegisterUser)
                                            .Where(query.GetQueryWhere())
                                            .Select(x => x.RegisterUser.Id)
                                            .ToListAsync();
        query.RecordCount = users.Count;
        if (query.RecordCount == 0)
        {
            query.Result = [];
        }
        else
        {
            query.Result = await dbContext.Students.AsNoTracking()
                                          .Include(x => x.RegisterUser)
                                          .Include(x => x.StudentFollows)
                                          .Where(query.GetQueryWhere())
                                          .OrderByDescending(x => x.RegisterUser.CreateTime)
                                          .Skip(query.PageStart)
                                          .Take(query.PageSize)
                                          .ToListAsync();
        }
        return query;
    }

    public async Task AutoMigrationAsync()
    {
        var provider = EngineContext.Current.Resolve<IServiceProvider>();
        var tables = await dbContext.GetTableNameList(nameof(Student));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<StudentDbContext>();
            tenantDbContext.ShardingAutoMigration();
        }
    }

    public async Task<RegisterUserPageDto> GetRegisterUsers(
        DateTime start, DateTime end,
        bool? isFollowed, bool? isBandingWeiXin,
        int pageIndex, int pageSize
    )
    {
        var sql = """
                  SELECT
                      r.`NickName`, r.`Phone`, e.`UniqueIdentifier` AS `UnionId`,
                      r.`CreateTime`, c.`FollowUserId`, c.`FollowUserName`
                  FROM `RegisterUser` r
                  LEFT JOIN `ExternalIdentity` e ON e.`RegisterUserId` = r.`Id` AND e.`Scheme` = 'WeiXin'
                  LEFT JOIN `EnterpriseWeChatContact` c ON c.`UnionId` = e.`UniqueIdentifier`
                  ORDER BY r.`CreateTime` desc
                  """;
        var query = dbContext.Database.SqlQueryRaw<RegisterUserDto>(sql).Where(x => x.CreateTime >= start && x.CreateTime < end);

        if (isFollowed == true)
        {
            query = query.Where(x => x.FollowUserId != null);
        }
        else if (isFollowed == false)
        {
            query = query.Where(x => x.FollowUserId == null);
        }

        if (isBandingWeiXin == true)
        {
            query = query.Where(x => x.UnionId != null);
        }
        else if (isBandingWeiXin == false)
        {
            query = query.Where(x => x.UnionId == null);
        }

        var result = new RegisterUserPageDto
        {
            Total = await query.CountAsync(),
            Data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
        };
        return result;
    }
}