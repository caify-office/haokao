using HaoKao.OpenPlatformService.Application.ViewModels.Account;
using HaoKao.OpenPlatformService.Domain.Entities;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface ILoginHelpService : IManager
{
    Task<RegisterUser> LoginRegisterUser(LoginInput loginInput);
}