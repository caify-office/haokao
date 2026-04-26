using HaoKao.GroupBookingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Domain.Repositories;

public interface IGroupSituationRepository : IRepository<GroupSituation>
{
    Task<List<GroupSituation>> GetSituationAndMemberAsync(QueryBase<GroupSituation> query);

    Task<List<GroupSituation>> GetSuccessGroupSituationAsync(QueryBase<GroupSituation> query);

    

    Task<GroupSituation> GetByIdIncludeAllAsync(Guid groupSituationId);



    Task<List<GroupSituation>> GetUserSuccessGroupSituationById(Guid userId);
}