namespace HaoKao.NotificationMessageService.Application.Services.Management;

public interface IMobileMessageSettingService : IAppWebApiService, IManager
{
    Task<EditMobileMessageSettingViewModel> GetCurrentMobileMessageSetting();

    Task UpdateCurrentMobileMessageSetting(EditMobileMessageSettingViewModel model);
}