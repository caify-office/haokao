using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;
using HaoKao.ErrorCorrectingService.Domain.Commands;

namespace HaoKao.ErrorCorrectingService.Application.AppService.App;

/// <summary>
/// 本题纠错接口服务-App
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ErrorCorrectingAppService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications
) : IErrorCorrectingAppService
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 创建本题纠错
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateErrorCorrectingViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();

        var command = new CreateErrorCorrectingCommand(
            model.QuestionId,
            userId,
            model.Description,
            model.QuestionTypes,
            model.SubjectId,
            model.SubjectName,
            model.QuestionTypeId,
            model.QuestionTypeName,
            model.QuestionText,
            model.NickName,
            model.Phone,
            model.CategoryId,
            model.CategoryName
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}