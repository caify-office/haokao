using Girvs.Extensions;
using HaoKao.GroupBookingService.Domain.Entities;
using System;

namespace HaoKao.GroupBookingService.Domain.Queries.EntityQuery;

public class GroupSituationMemberQuery : QueryBase<GroupSituation>
{
    [QueryCacheKey]
    public Guid GroupDataId { get; set; }

    public override Expression<Func<GroupSituation, bool>> GetQueryWhere()
    {
        Expression<Func<GroupSituation, bool>> expression = x => true;

        expression = expression.And(x => x.GroupDataId == GroupDataId);

        return expression;
    }
}