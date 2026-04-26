using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByLoginNameAndTenantIdAsync(string loginName, Guid tenantId);

    Task<List<User>> GetUserByAndTenantIdAsync(string tenantId);

    Task EventUpdateAsync(User user);

    Task EventUpdateRangeAsync(IEnumerable<User> users);

    Task<User> GetUserByIdIncludeRolesAsync(Guid userId);

    Task<User> GetUserByIdIncludeRoleAndDataRule(Guid userId);

    Task CreateTenantIdAdmin(User user);

    Task<List<User>> GetAllUserAsync(string userAccount);
}