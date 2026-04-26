using Girvs.Extensions;
using HaoKao.GroupBookingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace HaoKao.GroupBookingService.Domain.Queries.EntityQuery;

public class GroupDataQuery : QueryBase<GroupData>
{
    [QueryCacheKey]
    public string DataName { get; set; }

    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    public override Expression<Func<GroupData, bool>> GetQueryWhere()
    {
        Expression<Func<GroupData, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(DataName))
        {
            expression = expression.And(x => x.DataName.Contains(DataName));
        }
        if (SubjectId.HasValue)
        {
            expression = expression.And(x => EF.Functions.JsonContains(x.SuitableSubjects, $"\"{SubjectId}\""));
        }

        return expression;
    }
}
