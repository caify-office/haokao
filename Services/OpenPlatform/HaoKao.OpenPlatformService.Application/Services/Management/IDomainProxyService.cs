using HaoKao.OpenPlatformService.Application.ViewModels.DomainProxy;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

public interface IDomainProxyService :IAppWebApiService, IManager
{
    Task<List<BrowseDomainProxyViewModel>> GetCurrentDomainProxy();
}