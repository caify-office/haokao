using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.Repositories;

public class PermissionRepository(BasicDbContext dbContext) : Repository<BasalPermission>, IPermissionRepository
{
    private readonly BasicDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public Task<List<BasalPermission>> GetUserPermissionLimit(Guid userId)
    {
        return _dbContext.Set<BasalPermission>().Where(x => x.AppliedId == userId).ToListAsync();
    }

    public Task<List<BasalPermission>> GetRolePermissionLimit(Guid roleId)
    {
        return _dbContext.Set<BasalPermission>().Where(x => x.AppliedId == roleId).ToListAsync();
    }

    public Task<List<BasalPermission>> GetRoleListPermissionLimit(Guid[] roleIds)
    {
        return _dbContext.Set<BasalPermission>().Where(x => roleIds.Contains(x.AppliedId)).ToListAsync();
    }

    public async Task UpdatePermissions(List<BasalPermission> ps)
    {
        if (ps?.Count > 0)
        {
            var p = ps[0];
            var list = await GetWhereAsync(x => x.AppliedId == p.AppliedId
                                             && x.AppliedObjectType == p.AppliedObjectType
                                             && x.ValidateObjectType == p.ValidateObjectType);
            await DeleteRangeAsync(list);
            await AddRangeAsync(ps);
        }
    }
}