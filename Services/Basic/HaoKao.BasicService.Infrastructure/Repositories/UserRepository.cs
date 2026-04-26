using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public Task<User> GetUserByLoginNameAndTenantIdAsync(string loginName, Guid tenantId)
    {
        //此处需地绕过所有的查询条件
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        return dbContext.Set<User>().Include(x => x.Roles)
                        .FirstOrDefaultAsync(x => x.UserAccount == loginName && x.TenantId == tenantId);
    }

    public Task<List<User>> GetUserByAndTenantIdAsync(string tenantId)
    {
        //此处需地绕过所有的查询条件
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        return dbContext.Set<User>().Where(x => x.TenantId == tenantId.ToGuid()).ToListAsync();
    }

    public Task EventUpdateAsync(User user)
    {
        //此处需地绕过所有的查询条件 因为当前是系统管理员发出的事件，需要修改考试管理员信息
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        dbContext.Set<User>().Update(user);
        return Task.CompletedTask;
    }

    public Task EventUpdateRangeAsync(IEnumerable<User> users)
    {
        //此处需地绕过所有的查询条件 因为当前是系统管理员发出的事件，需要修改考试管理员信息
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        dbContext.Set<User>().UpdateRange(users);
        return Task.CompletedTask;
    }

    public Task<User> GetUserByIdIncludeRolesAsync(Guid userId)
    {
        return Queryable.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);
    }

    public Task<User> GetUserByIdIncludeRoleAndDataRule(Guid userId)
    {
        //此处需地绕过所有的查询条件，防止无限死循环
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        return dbContext.Set<User>()
                        .Include(x => x.Roles)
                        .Include(x => x.RulesList)
                        .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public Task CreateTenantIdAdmin(User user)
    {
        //此处创建的用户是通过事件创建的租户管理员，是通过总管理员创建，需要绕过租户的判断
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        dbContext.Set<User>().AddAsync(user);
        return Task.CompletedTask;
    }

    public virtual Task<List<User>> GetAllUserAsync(string userAccount)
    {
        //此处创建的用户是通过事件创建的租户管理员，是通过总管理员创建，需要绕过租户的判断
        var dbContext = EngineContext.Current.Resolve<BasicDbContext>();
        return dbContext.Set<User>().Where(x => x.UserAccount == userAccount).ToListAsync();
    }
}