using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Models;
using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Infrastructure.Works;

public class SyncStudentFollowWork(IServiceProvider provider, StudentDbContext dbContext) : ISyncStudentFollowWork
{
    public async Task ExecuteAsync()
    {
        var salespersons = await GetSalespersonsAsync();
        var contacts = await GetEnterpriseWeChatContactsAsync();

        var tables = await dbContext.GetTableNameList(nameof(Student));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<StudentDbContext>();
            tenantDbContext.ShardingAutoMigration();

            var tenantStudents = await tenantDbContext.Students.AsNoTracking()
                                                      .Include(x => x.StudentFollows)
                                                      .Include(x => x.RegisterUser)
                                                      .ThenInclude(x => x.ExternalIdentities)
                                                      .Where(x => x.RegisterUser.ExternalIdentities.Any(y => y.Scheme == "Weixin"))
                                                      .ToListAsync();

            var tenantFollows = new List<StudentFollow>();
            foreach (var student in tenantStudents)
            {
                // 企业微信中已经添加了学员的记录, 同一学员可以被多个销售人员添加
                var followedContacts = contacts.Where(x => x.UnionId == student.RegisterUser.UnionId).ToList();
                if (followedContacts.Count == 0) continue; // 没有销售人员添加该学员

                foreach (var contact in followedContacts)
                {
                    // 当前租户学员已经有添加记录
                    if (student.StudentFollows.Any(x => x.EnterpriseWeChatId == contact.FollowUserId)) continue;

                    // 当前租户下的销售人员
                    var salesperson = salespersons.FirstOrDefault(x => x.EnterpriseWeChatUserId == contact.FollowUserId && x.TenantId == student.TenantId);

                    tenantFollows.Add(new StudentFollow
                    {
                        StudentId = student.Id,
                        RegisterUserId = student.RegisterUserId,
                        UnionId = student.RegisterUser.UnionId,
                        SalespersonId = salesperson?.Id ?? Guid.Empty,
                        SalespersonName = salesperson?.RealName ?? "",
                        EnterpriseWeChatId = contact.FollowUserId,
                        CreateTime = DateTime.Now,
                        TenantId = student.TenantId
                    });
                }
            }

            if (tenantFollows.Count == 0) continue;
            tenantDbContext.StudentFollows.AddRange(tenantFollows);
            await tenantDbContext.SaveChangesAsync();
        }
    }

    private async Task<IReadOnlyList<Salesperson>> GetSalespersonsAsync()
    {
        var sql = "select * from `Salesperson`";
        return await dbContext.Database.SqlQueryRaw<Salesperson>(sql).ToListAsync();
    }

    private async Task<IReadOnlyList<EnterpriseWeChatContact>> GetEnterpriseWeChatContactsAsync()
    {
        var sql = "select * from `EnterpriseWeChatContact`";
        return await dbContext.Database.SqlQueryRaw<EnterpriseWeChatContact>(sql).ToListAsync();
    }
}