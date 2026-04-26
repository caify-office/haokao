using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetRoleByIdIncludeUsersAsync(Guid roleId);
}