namespace HaoKao.NotificationMessageService.Application.Services.Management;

public interface ITenantSignSettingService : IAppWebApiService
{
    Task<string> GetCurrentSignSetting();

    Task UpdateCurrentSignSetting(string sign);

    Task<List<string>> GetSystemSignList();
}