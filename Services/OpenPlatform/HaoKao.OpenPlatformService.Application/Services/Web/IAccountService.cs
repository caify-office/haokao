using HaoKao.OpenPlatformService.Application.ViewModels.Account;
using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IAccountService : IAppWebApiService, IManager
{
    Task<RegisterUserDetailViewModel> GetUserInfo();

    Task<RegisterUserDetailViewModel> GetUserInfoByToken();

    Task<bool> ChangePhone(ChangePhoneViewModel model);

    Task<bool> ModifyPasswordByCode(ModifyPasswordByCodeViewModel model);

    Task<bool> ModifyPassword(ModifyPasswordViewModel model);

    Task<bool> UpdateRegisterInformation(UpdateRegisterViewModel model);

    Task<LoginViewModel> GetLoginInfo(string returnUrl);

    Task<string> Login(LoginInput model);

    Task<LoggedOutViewModel> Logout(LogoutInput input);

    Task<ErrorViewModel> Error(string errorId);

    Task<ConsentViewModel> GetConsentContent(string returnUrl);

    Task<ConsentViewModel> Consent(ConsentInputModel model);

    Task<DeviceAuthorizationViewModel> UserCodeCapture(string userCode);

    Task<bool> IsRegister(IsRegisterInput input);

    Task<bool> CancelAccount();
}