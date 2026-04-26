using HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;
using HaoKao.FeedBackService.Domain.Commands.FeedBackReply;

namespace HaoKao.FeedBackService.Application.Services.Management;

/// <summary>
/// 反馈建议回复服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "反馈建议",
    "08dbfbb4-44c1-49a9-8b71-7e3e0d98e0eb",
    "64",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class FeedBackReplyService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications
) : IFeedBackReplyService
{
    #region 初始参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    #endregion

    #region 服务方法

    /// <summary>
    /// 创建答疑回复
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateFeedBackReplyViewModel model)
    {
        var command = new CreateFeedBackReplyCommand(
            model.ReplyContent,
            model.ReplyUserName,
            model.FeedBackId,
            model.FileUrl
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}