namespace HaoKao.OpenPlatformService.Application.Services.App;

public interface ITestAppService : IAppWebApiService
{
    Task<string> Get();
}