using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public Task<Role> GetRoleByIdIncludeUsersAsync(Guid roleId)
    {
        return Queryable.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == roleId);
    }
}