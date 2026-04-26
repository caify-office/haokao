using HaoKao.GroupBookingService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Domain.Repositories;

public interface IGroupMemberRepository : IRepository<GroupMember>
{
    Task<bool> IsGroupSuccess(Guid groupContentId, Guid userId);

    Task<bool> IsInGroup(Guid groupContentId, Guid userId);
}
