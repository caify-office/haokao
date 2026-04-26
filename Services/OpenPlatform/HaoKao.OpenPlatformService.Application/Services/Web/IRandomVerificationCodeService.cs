using HaoKao.OpenPlatformService.Application.ViewModels;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IRandomVerificationCodeService : IAppWebApiService, IManager
{
    Task<string> GetRandomVerificationCode(string randomMark);
    Task<VerificationCodeViewModel> GetRandomVerification(string randomMark);
    Task<bool> VerificationCode(string randomMark,string code);
}