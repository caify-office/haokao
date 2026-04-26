using DotNetCore.CAP;
using Girvs.Cache.Configuration;
using Girvs.EventBus;
using Girvs.EventBus.Extensions;
using HaoKao.Common.Events.StudentPermission;
using HaoKao.ProductService.Domain.Commands.StudentPermission;
using HaoKao.ProductService.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace HaoKao.ProductService.Application.EventBusHandlers;

public class CreateStudentPermissionEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    ILogger<CreateStudentPermissionEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider)
    : GirvsIntegrationEventHandler<CreateStudentPermissionEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateStudentPermissionEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateStudentPermissionEvent))]
    public override async Task Handle(CreateStudentPermissionEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation($"Handling 'CreateStudentPermissionEvent' eventId:{e.Id}");

        // 这里调用命令，具体处理逻辑在命令类中
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        //加锁
        await _locker.PerformActionWithLockAsync(
            e.Id.ToString(), timeSpan,
            async () =>
            {
                var command = new CreateStudentPermissionEventCommand
                (
                    e.StudentName,
                    e.StudentId,
                    e.OrderNumber,
                    e.PurchaseProductContents,
                    (SourceMode)e.SourceMode
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    _logger.LogError($"Handling 'CreateStudentPermissionEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            });
    }
}