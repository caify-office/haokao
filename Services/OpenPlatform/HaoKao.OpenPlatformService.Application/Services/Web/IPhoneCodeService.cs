using HaoKao.Common.Events.NotificationMessage;
using HaoKao.OpenPlatformService.Application.ViewModels.PhoneCode;

namespace HaoKao.OpenPlatformService.Application.Services.Web;

public interface IPhoneCodeService : IAppWebApiService, IManager
{
    Task<bool> SendPhoneCode(SendPhoneCodeViewModel model);

    bool CheckPhoneCode(EventNotificationMessageType messageType, string phone, string code);
}