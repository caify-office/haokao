using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveOnlineUserTrendRepository : IRepository<LiveOnlineUserTrend>
{
    /// <summary>
    /// 记录在线用户走势数据
    /// </summary>
    /// <param name="interval">记录间隔(分钟)</param>
    /// <returns></returns>
    Task TrackOnlineUserTrend(int interval);
}