using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ILiveMessageWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryLiveMessageViewModel> Get(QueryLiveMessageViewModel queryViewModel);

    /// <summary>
    /// 获取直播间置顶的消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<PinTopMessageOutput> GetPinedMessage(PinTopMessageRequest request);
}