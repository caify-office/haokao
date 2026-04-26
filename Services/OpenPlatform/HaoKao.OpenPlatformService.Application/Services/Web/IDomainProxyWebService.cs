using HaoKao.OpenPlatformService.Application.ViewModels.DomainProxy;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IDomainProxyWebService : IAppWebApiService,IManager
{
    Task<BrowseDomainProxyViewModel> GetCurrentDomainProxy(string domain);
}