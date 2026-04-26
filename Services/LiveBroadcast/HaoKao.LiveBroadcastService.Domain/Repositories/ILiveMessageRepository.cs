using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.Repositories;

public interface ILiveMessageRepository : IRepository<LiveMessage>
{
    /// <summary>
    /// 新增消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task AddMessage(LiveMessage message);
}