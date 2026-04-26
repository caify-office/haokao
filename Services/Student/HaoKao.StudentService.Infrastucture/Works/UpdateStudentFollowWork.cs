using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Models;
using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Infrastructure.Works;

public class UpdateStudentFollowWork(IServiceProvider provider, StudentDbContext dbContext) : IUpdateStudentFollowWork
{
    public async Task ExecuteAsync()
    {
        var salespersons = await GetSalespersonsAsync();

        var tables = await dbContext.GetTableNameList(nameof(Student));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<StudentDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var tenantFollows = await tenantDbContext.StudentFollows.AsNoTracking().ToListAsync();

            foreach (var follow in tenantFollows)
            {
                var salesperson = salespersons.FirstOrDefault(x => x.EnterpriseWeChatUserId == follow.EnterpriseWeChatId);
                if (salesperson != null)
                {
                    follow.SalespersonId = salesperson.Id;
                    follow.SalespersonName = salesperson.RealName;
                }
            }

            if (tenantFollows.Count == 0) continue;
            tenantDbContext.StudentFollows.UpdateRange(tenantFollows);
            await tenantDbContext.SaveChangesAsync();
        }
    }

    private async Task<IReadOnlyList<Salesperson>> GetSalespersonsAsync()
    {
        var sql = "select * from `Salesperson`";
        return await dbContext.Database.SqlQueryRaw<Salesperson>(sql).ToListAsync();
    }
}