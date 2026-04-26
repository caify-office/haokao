namespace HaoKao.OpenPlatformService.Application.Services.Management;

public interface IScopeService : IAppWebApiService
{
    Task<ICollection<string>> Get();
}