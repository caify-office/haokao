namespace HaoKao.LiveBroadcastService.Application.Services.Web;

public interface ISensitiveWordWebService : IAppWebApiService, IManager
{
    Task<BrowseSensitiveWordViewModel> Get();
}