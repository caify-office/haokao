using Girvs.Extensions;
using HaoKao.GroupBookingService.Domain.Entities;
using HaoKao.GroupBookingService.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HaoKao.GroupBookingService.Domain.Queries.EntityQuery;

public class GroupSituationQuery : QueryBase<GroupSituation>
{
    [QueryCacheKey]
    public string DataName { get; set; }

    /// <summary>
    /// 拼团资料Id
    /// </summary>
    [QueryCacheKey]
    public Guid? GroupDataId { get; set; }

    [QueryCacheKey]
    public Guid? SubjectId { get; set; }

    [QueryCacheKey]
    public string Name { get; set; }

    [QueryCacheKey]
    public GroupSituationStatus? Status { get; set; }

    public override Expression<Func<GroupSituation, bool>> GetQueryWhere()
    {
        Expression<Func<GroupSituation, bool>> expression = x => true;

        if (!string.IsNullOrEmpty(DataName))
        {
            expression = expression.And(x => x.DataName.Contains(DataName));
        }
        if (GroupDataId.HasValue)
        {
            expression = expression.And(x => x.GroupDataId == GroupDataId.Value);
        }

        if (SubjectId.HasValue)
        {
            expression = expression.And(x => EF.Functions.JsonContains(x.SuitableSubjects, $"\"{SubjectId}\""));
        }

        if (!string.IsNullOrEmpty(Name))
        {
            expression = expression.And(x => x.GroupMembers.Any(t => t.Name.Contains(Name)));
        }

        if (Status.HasValue)
        {
            if (Status.Value == GroupSituationStatus.Success) //已成功
            {
                expression = expression.And(x => x.SuccessTime.HasValue);
            }
            else if (Status.Value == GroupSituationStatus.Faild)//已失败
            {
                expression = expression.And(x => DateTime.Now > x.CreateTime.AddHours(x.LimitTime) && !x.SuccessTime.HasValue);
            }
            else if (Status.Value == GroupSituationStatus.InGroup)//拼团中
            {
                expression = expression.And(x => DateTime.Now < x.CreateTime.AddHours(x.LimitTime) && !x.SuccessTime.HasValue);
            }
        }
        return expression;
    }
}