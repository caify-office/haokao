using Girvs.BusinessBasis.Queries;
using Girvs.Extensions.Collections;
using Girvs.Infrastructure;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.GroupBookingService.Infrastructure.Repositories;

public class GroupSituationRepository : Repository<GroupSituation>, IGroupSituationRepository
{
    /// <summary>
    /// 拼团情况查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<List<GroupSituation>> GetSituationAndMemberAsync(QueryBase<GroupSituation> query)
    {
        query.RecordCount = await Queryable.Where(query.GetQueryWhere()).CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(query.GetQueryWhere()).Include(t => t.GroupMembers.OrderBy(t => t.CreateTime))
                    .SelectProperties(query.QueryFields)
                    .OrderByDescending(x => x.CreateTime)
                    .Skip(query.PageStart)
                    .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }

    /// <summary>
    /// 通过资料Id查询拼团情况
    /// </summary>
    /// <returns></returns>
    public async Task<List<GroupSituation>> GetSuccessGroupSituationAsync(QueryBase<GroupSituation> query)
    {
        query.RecordCount = await Queryable.Where(t => t.SuccessTime.HasValue).Where(query.GetQueryWhere()).CountAsync();
        if (query.RecordCount < 1)
        {
            query.Result = [];
        }
        else
        {
            query.Result =
                await Queryable.Where(t => t.SuccessTime.HasValue).Where(query.GetQueryWhere()).Include(t => t.GroupMembers.OrderBy(t => t.CreateTime))
                    .SelectProperties(query.QueryFields)
                    .Select(x => new GroupSituation
                    {
                        DataName = x.DataName,
                        SuccessTime = x.SuccessTime,
                        PeopleNumber = x.PeopleNumber,
                        CreateTime = x.CreateTime,
                        GroupMembers = x.GroupMembers,
                    })
                    .OrderBy(x => x.CreateTime)
                    .Skip(query.PageStart)
                    .Take(query.PageSize).ToListAsync();
        }

        return query.Result;
    }


    /// <summary>
    /// 查询拼团详细信息
    /// </summary>
    /// <param name="groupSituationId"></param>
    /// <returns></returns>
    public Task<GroupSituation> GetByIdIncludeAllAsync(Guid groupSituationId)
    {
        return Queryable.Include(x => x.GroupMembers)
                        .FirstOrDefaultAsync(x => x.Id.Equals(groupSituationId));
    }


    /// <summary>
    /// 通过用户Id获取已拼团成功数据
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<List<GroupSituation>> GetUserSuccessGroupSituationById(Guid userId)
    {
        var dbContext= EngineContext.Current.Resolve<GroupBookingDbContext>();
        var successGroupSituationIds = dbContext.GroupMembers.Where(x => x.CreatorId == userId && x.SuccessTime.HasValue).Select(x => x.GroupSituationId);
        return dbContext.GroupSituations.Where(x => successGroupSituationIds.Contains(x.Id)).ToListAsync();
    }
}
