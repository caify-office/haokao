using HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

public interface IMutedUserWebService : IAppWebApiService, IManager
{
    Task<bool> IsMuted();

    Task Mute(CreateMutedUserViewModel model);
}