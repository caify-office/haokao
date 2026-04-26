using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Infrastructure.Repositories;

public class GroupMemberRepository : Repository<GroupMember>, IGroupMemberRepository
{
    
    /// <summary>
    /// 当前用户当前资料是否已完成拼团
    /// </summary>
    /// <param name="groupDataId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<bool> IsGroupSuccess(Guid groupDataId, Guid userId)
    {
        var result = Queryable.Any(x => x.CreatorId == userId && x.GroupDataId == groupDataId && x.SuccessTime.HasValue);
        return Task.FromResult(result);
    }

    /// <summary>
    /// 当前用户当前资料是否正在拼团中
    /// </summary>
    /// <param name="groupDataId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<bool> IsInGroup(Guid groupDataId, Guid userId)
    {
        var timeNow = DateTime.Now;
        var result = Queryable.Any(x => x.CreatorId == userId && x.GroupDataId == groupDataId && !x.SuccessTime.HasValue && x.ExpirationTime > timeNow);
        return Task.FromResult(result);
    }
}
