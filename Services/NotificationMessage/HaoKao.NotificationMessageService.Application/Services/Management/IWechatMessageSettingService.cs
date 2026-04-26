namespace HaoKao.NotificationMessageService.Application.Services.Management;

public interface IWechatMessageSettingService : IAppWebApiService, IManager
{
    Task<EditWechatMessageSettingViewModel> GetCurrentWechatMessageSetting();

    Task UpdateCurrentWechatMessageSetting(EditWechatMessageSettingViewModel model);
}