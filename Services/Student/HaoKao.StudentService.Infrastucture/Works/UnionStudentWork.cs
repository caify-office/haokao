using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Infrastructure.Works;

public class UnionStudentWork(IServiceProvider provider) : IUnionStudentWork
{
    public async Task ExecuteAsync()
    {
        await using var scope = provider.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
        var tables = await dbContext.GetTableNameList(nameof(Student));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<StudentDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var sql = @$"
UPDATE `UnionStudent` u
INNER JOIN `{table}` s ON u.`RegisterUserId` = s.`RegisterUserId` AND u.`TenantId` = s.`TenantId`
SET u.`IsPaidStudent` = s.`IsPaidStudent`";
            await tenantDbContext.Database.ExecuteSqlRawAsync(sql);

            tenantDbContext.UnionStudents.AddRange(
                tenantDbContext.Students.AsNoTracking()
                .Where(x => !tenantDbContext.UnionStudents.Any(y => y.RegisterUserId == x.RegisterUserId && y.TenantId==x.TenantId))
                .Select(x => new UnionStudent
                {
                    Id = x.Id,
                    RegisterUserId = x.RegisterUserId,
                    IsPaidStudent = x.IsPaidStudent,
                    CreateTime = x.CreateTime,
                    TenantId = x.TenantId,
                })
            );
            await tenantDbContext.SaveChangesAsync();
        }
    }
}