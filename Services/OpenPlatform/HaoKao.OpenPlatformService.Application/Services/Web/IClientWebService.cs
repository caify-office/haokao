using HaoKao.OpenPlatformService.Application.ViewModels.ClientViewModel;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IClientWebService : IAppWebApiService, IManager
{
    Task<BrowseClientViewModel> Get(string clientId);
}