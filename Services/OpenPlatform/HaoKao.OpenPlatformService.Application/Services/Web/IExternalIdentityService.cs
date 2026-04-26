using HaoKao.OpenPlatformService.Application.IdentityServerExtensions.OAuth;
using HaoKao.OpenPlatformService.Application.ViewModels.Identity;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IExternalIdentityService : IAppWebApiService, IManager
{
    Task<IEnumerable<ExternalProvider>> GetExternalSchemeAsync();

    Task<string> GetOAuthUrlByScheme(OAuthContextViewModel model);

    Task<ExternalAuthenticationViewModel> OAuthCallBack();

    Task<(string, ExternalUser)> GetExternalUserInfo();

    Task<ExternalAuthenticationViewModel> GetExternalUserInfoIsRegister();

    Task<bool> WechatAppMiniProgramUserIsRegister(string unionId);

    Task<ExternalAuthenticationViewModel> CreateExternalTicketByCodeAsync(ExternalCodeAuthenticationOptions options);
}