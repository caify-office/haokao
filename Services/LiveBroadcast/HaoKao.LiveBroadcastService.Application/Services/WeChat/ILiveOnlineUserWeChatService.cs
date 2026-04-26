using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ILiveOnlineUserWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取直播间在线用户
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    Task<List<OnlineUserViewModel>> GetOnlineUserList(Guid liveId);
}