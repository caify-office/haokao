using HaoKao.ProductService.Domain.Entities;

namespace HaoKao.ProductService.Domain.Repositories;

public interface ISupervisorStudentRepository : IRepository<SupervisorStudent>
{
    bool ExistSupervisorClass(Guid registerUserId, Guid productId);

    bool ExistSupervisorClass(Guid registerUserId, Guid productId, Guid supervisorClassId);
    Task<List<SupervisorStudent>> GetStatisticsData(List<SupervisorStudent> supervisorStudent, Guid productId);
}
