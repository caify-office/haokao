using HaoKao.NotificationMessageService.Domain.Models;
using HaoKao.NotificationMessageService.Domain.Queries;

namespace HaoKao.NotificationMessageService.Domain.Repositories;

public interface INotificationMessageRepository : IRepository<NotificationMessage>
{
    Task GetByQueryResultSpAsync(NotificationMessageQuery query);

    Task<int> GetUnReadCountSpAsync(string idCard, Guid tenantAccessId);

    Task<bool> FollowWeChat(WechatMessageSetting setting, string openId);

    Task<string> GetOpenIdAsync(WechatMessageSetting wechatMessageSetting, string code);
}