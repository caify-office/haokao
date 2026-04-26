using HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface IMutedUserWeChatService : IAppWebApiService, IManager
{
    Task<bool> IsMuted();

    Task Mute(CreateMutedUserViewModel model);
}