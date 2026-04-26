using HaoKao.LearnProgressService.Domain.Entities;
using HaoKao.LearnProgressService.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LearnProgressService.Infrastructure.Repositories;

public class DailyStudyDurationRepository : Repository<DailyStudyDuration>, IDailyStudyDurationRepository
{
    public Task<int> GetLearnDayCount(Guid productId,Guid subjectId, Guid userId)
    {
        return Queryable.Where(x =>
        x.ProductId==productId
        &&x.SubjectId==subjectId
        &&x.CreatorId == userId).CountAsync();
    }
}
