namespace HaoKao.NotificationMessageService.Application.Services.Management;

public interface INotificationMessageService : IAppWebApiService
{
    Task<NotificationMessageQueryViewModel> Get(NotificationMessageQueryViewModel queryViewModel);

    Task SendAssignNotificationMessage(AssignNotificationMessageViewModel model);
}