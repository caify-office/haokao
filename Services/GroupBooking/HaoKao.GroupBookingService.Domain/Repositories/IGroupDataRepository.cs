using HaoKao.GroupBookingService.Domain.Entities;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Domain.Repositories;

public interface IGroupDataRepository : IRepository<GroupData>
{
    Task<DataTable> GetGroupDataListBySubjectId(Guid[] subjectIds, int? takeCount);

    Task<DataTable> GetMyGroupDataList();
}
