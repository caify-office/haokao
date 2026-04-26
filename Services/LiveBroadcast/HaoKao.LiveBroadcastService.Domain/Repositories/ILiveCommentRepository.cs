using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveCommentRepository : IRepository<LiveComment>
{
    /// <summary>
    /// 综合评分
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    Task<double> GetAverageRating(Guid liveId);
}