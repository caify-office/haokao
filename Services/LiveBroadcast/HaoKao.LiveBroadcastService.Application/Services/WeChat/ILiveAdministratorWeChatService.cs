namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ILiveAdministratorWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 是否是直播管理员
    /// </summary>
    Task<bool> IsLiveAdmin();
}