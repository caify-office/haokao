using HaoKao.LiveBroadcastService.Domain.Enums;

namespace HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

[AutoMapFrom(typeof(LiveMessageQuery))]
[AutoMapTo(typeof(LiveMessageQuery))]
public class QueryLiveMessageViewModel : QueryDtoBase<BrowseLiveMessageViewModel>
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public LiveMessageType? MessageType { get; set; }

    /// <summary>
    /// 直播间Id
    /// </summary>
    public Guid? LiveId { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }
}