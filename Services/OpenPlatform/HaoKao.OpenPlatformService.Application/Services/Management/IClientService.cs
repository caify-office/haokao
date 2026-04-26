using HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

public interface IClientService : IAppWebApiService, IManager
{
    Task<ClientQueryListViewModel> Get(ClientQueryListViewModel query);
    Task<BrowseClientViewModel> Get(Guid id);
    Task<BrowseClientViewModel> GetByClientId(string clientId);
    Task Create(CreateClientViewModel model);
    Task Update(UpdateClientViewModel model);
    Task Delete(Guid id);
}