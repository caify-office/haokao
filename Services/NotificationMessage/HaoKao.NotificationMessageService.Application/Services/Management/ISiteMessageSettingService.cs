namespace HaoKao.NotificationMessageService.Application.Services.Management;

public interface ISiteMessageSettingService : IAppWebApiService
{
    Task<List<MessageTemplate>> GetCurrentMobileMessageSetting();

    Task UpdateCurrentMobileMessageSetting(List<MessageTemplate> model);
}