using HaoKao.LearnProgressService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HaoKao.LearnProgressService.Domain.Repositories;

public interface IDailyStudyDurationRepository : IRepository<DailyStudyDuration>
{
    Task<int> GetLearnDayCount(Guid productId, Guid subjectId, Guid userId);
}
