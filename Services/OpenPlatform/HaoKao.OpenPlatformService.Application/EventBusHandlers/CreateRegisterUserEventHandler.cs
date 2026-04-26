using DotNetCore.CAP;
using Girvs.Cache.Configuration;
using Girvs.EventBus;
using Girvs.EventBus.Extensions;
using HaoKao.Common.Events.OpenPlatform;
using HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HaoKao.OpenPlatformService.Application.EventBusHandlers;

public class CreateRegisterUserEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    ILogger<CreateRegisterUserEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<CreateRegisterUserEvent>(serviceProvider)
{
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateRegisterUserEvent))]
    public override async Task Handle(CreateRegisterUserEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        logger.LogInformation($"Handling 'CreateRegisterUserEvent' eventId:{e.Id}");

        // 这里调用命令，具体处理逻辑在命令类中
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        //加锁
        await locker.PerformActionWithLockAsync(
            e.Id.ToString(), timeSpan,
            async () =>
            {
                var command = new CreateRegisterUserCommand(e.Phone, null, null, e.CreatorId, e.NickName);

                await bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    logger.LogError($"Handling 'CreateStudentPermissionEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            });
    }
}