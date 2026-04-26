using HaoKao.BasicService.Domain.Entities;

namespace HaoKao.BasicService.Infrastructure.Repositories;

public class ServicePermissionRepository : Repository<ServicePermission, Guid>, IServicePermissionRepository;