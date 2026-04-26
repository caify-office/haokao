namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ISensitiveWordWeChatService : IAppWebApiService, IManager
{
    Task<BrowseSensitiveWordViewModel> Get();
}