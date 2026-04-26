namespace HaoKao.LiveBroadcastService.Application.Services.Web;

public interface ILiveAdministratorWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 是否是直播管理员
    /// </summary>
    Task<bool> IsLiveAdmin();
}