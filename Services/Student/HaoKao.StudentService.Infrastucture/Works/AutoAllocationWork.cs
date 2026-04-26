using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Works;

namespace HaoKao.StudentService.Infrastructure.Works;

public class AutoAllocationWork(IServiceProvider provider, StudentDbContext dbContext) : IAutoAllocationWork
{
    public async Task ExecuteAsync()
    {
        var tables = await dbContext.GetTableNameList(nameof(Student));
        foreach (var table in tables)
        {
            await using var tenantScope = provider.CreateTenantAsyncScope(table);
            // using var tenantScope = provider.CreateTenantAsyncScope("Student_08db5bf2afae4d40889618e7e86b6b37");
            await using var tenantDbContext = tenantScope.ServiceProvider.GetRequiredService<StudentDbContext>();
            tenantDbContext.ShardingAutoMigration();

            // 当前租户下绑定了微信的学员
            var students = await tenantDbContext.Students.AsNoTracking()
                                                .Include(x => x.StudentFollows)
                                                .Where(x => x.StudentFollows.Count == 0) // 尚未被销售添加过的
                                                .Include(x => x.RegisterUser)
                                                .ThenInclude(x => x.ExternalIdentities)
                                                .Where(x => x.RegisterUser.ExternalIdentities.Any(y => y.Scheme == "Weixin"))
                                                .ToListAsync();

            // 读取当前租户下的所有已分配的学员
            var tenantAllocatedStudents = await tenantDbContext.StudentAllocations.AsNoTracking()
                                                               .Include(x => x.Student)
                                                               .ThenInclude(x => x.StudentFollows)
                                                               .Include(x => x.Student)
                                                               .ThenInclude(x => x.RegisterUser)
                                                               .ThenInclude(x => x.ExternalIdentities)
                                                               .Where(x => x.Student.RegisterUser.ExternalIdentities.Any(y => y.Scheme == "Weixin"))
                                                               .ToListAsync();

            // 当前租户下未被分配的学员
            var notAllocatedStudents = students.Where(x => !tenantAllocatedStudents.Select(y => y.StudentId).Contains(x.Id))
            .Select(x => new StudentAllocation
            {
                StudentId = x.Id,
                RegisterUserId = x.RegisterUserId,
                UnionId = x.RegisterUser.UnionId,
                TenantId = x.TenantId
            }).ToList();

            // 分配日起三个自然日后重新分配
            var reallocationStudents = tenantAllocatedStudents.Where(x => x.Student.StudentFollows.Count == 0 && DateTime.Now > x.AllocationTime.AddDays(3)).ToList();

            notAllocatedStudents.AddRange(reallocationStudents.Select(x => new StudentAllocation
            {
                StudentId = x.StudentId,
                RegisterUserId = x.RegisterUserId,
                UnionId = x.Student.RegisterUser.UnionId,
                TenantId = x.TenantId
            }));

            if (notAllocatedStudents.Count == 0) continue;

            // 读取当前租户下的学员分配配置
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
            var allocationConfig = await tenantDbContext.StudentAllocationConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.TenantId == tenantId);
            if (allocationConfig == null) continue; // 当前租户下没有配置分配规则

            var allocator = new DefaultStudentAllocator(
                [.. allocationConfig.Data.OrderBy(_ => Guid.NewGuid())],
                notAllocatedStudents,
                tenantAllocatedStudents
            );
            allocator.Allocate();

            tenantDbContext.StudentAllocations.RemoveRange(reallocationStudents);
            await tenantDbContext.SaveChangesAsync();
            await tenantDbContext.StudentAllocations.AddRangeAsync(notAllocatedStudents);
            await tenantDbContext.SaveChangesAsync();
        }
    }
}

public interface IStudentAllocator
{
    HashSet<PercentageAllocation> Options { get; }

    List<StudentAllocation> NotAllocateStudents { get; }

    List<StudentAllocation> AllocatedStudents { get; }

    void Allocate();
}

public class DefaultStudentAllocator(
    HashSet<PercentageAllocation> options,
    List<StudentAllocation> notAllocateStudents,
    List<StudentAllocation> allocatedStudents
) : IStudentAllocator
{
    public HashSet<PercentageAllocation> Options => options;

    public List<StudentAllocation> NotAllocateStudents => notAllocateStudents;

    public List<StudentAllocation> AllocatedStudents => allocatedStudents;

    public void Allocate()
    {
        // 按学员的数量比例分配
        var skip = 0;
        var totalCount = NotAllocateStudents.Count;

        foreach (var option in Options)
        {
            var take = (int)Math.Ceiling(totalCount * option.Percentage);
            foreach (var allocation in NotAllocateStudents.Skip(skip).Take(take))
            {
                allocation.SalespersonId = option.SalespersonId;
                allocation.SalespersonName = option.SalespersonName;
                allocation.EnterpriseWeChatId = option.EnterpriseWeChatId;
                allocation.AllocationTime = DateTime.Now;
            }
            skip += take;
        }
    }
}

public class BalanceAllocator(
    HashSet<PercentageAllocation> options,
    List<StudentAllocation> notAllocateStudents,
    List<StudentAllocation> allocatedStudents
) : IStudentAllocator
{
    public HashSet<PercentageAllocation> Options => options;

    public List<StudentAllocation> NotAllocateStudents => notAllocateStudents;

    public List<StudentAllocation> AllocatedStudents => allocatedStudents;

    public void Allocate()
    {
        // 按学员的数量比例分配
        var skip = 0;
        var totalCount = NotAllocateStudents.Count;

        // 按销售分配到的数量的比例分配
        foreach (var option in Options)
        {
            // 销售已经分配的学员数量
            var currentAllocatedCount = AllocatedStudents.Count(x => x.SalespersonId == option.SalespersonId);
            // 销售应该分配的学员数量
            var pieceCount = (int)Math.Ceiling((AllocatedStudents.Count + NotAllocateStudents.Count) * option.Percentage);

            var take = pieceCount - currentAllocatedCount;
            if (take <= 0) continue;

            foreach (var allocation in NotAllocateStudents.Skip(skip).Take(take))
            {
                allocation.SalespersonId = option.SalespersonId;
                allocation.SalespersonName = option.SalespersonName;
                allocation.EnterpriseWeChatId = option.EnterpriseWeChatId;
                allocation.AllocationTime = DateTime.Now;
            }
            skip += take;
        }
    }
}