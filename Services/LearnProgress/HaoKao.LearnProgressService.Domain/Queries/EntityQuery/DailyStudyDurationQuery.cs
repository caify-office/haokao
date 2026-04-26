using HaoKao.LearnProgressService.Domain.Entities;
using System;

namespace HaoKao.LearnProgressService.Domain.Queries.EntityQuery;

public class DailyStudyDurationQuery : QueryBase<DailyStudyDuration>
{   
    public override Expression<Func<DailyStudyDuration, bool>> GetQueryWhere()
    {
        Expression<Func<DailyStudyDuration, bool>> expression = x => true;
        return expression;
    }
}
