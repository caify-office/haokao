using DotNetCore.CAP;
using Girvs.Cache.Configuration;
using Girvs.EventBus;
using Girvs.EventBus.Extensions;
using HaoKao.Common.Events.Student;
using HaoKao.StudentService.Domain.Commands;
using System.Threading;

namespace HaoKao.StudentService.Application.EventBusHandlers;

public class CreateStudentEventHandler(
    ILocker locker,
    ILogger<CreateStudentEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<CreateStudentEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly ILogger<CreateStudentEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    [CapSubscribe(nameof(CreateStudentEvent))]
    public override async Task Handle(CreateStudentEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation($"Handling 'UpdateStudentPaidEvent' eventId:{e.Id}");

        // 这里调用命令，具体处理逻辑在命令类中
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _semaphore.WaitAsync(cancellationToken);

        //加锁
        await _locker.PerformActionWithLockAsync(
            e.Id.ToString(), timeSpan,
            async () =>
            {
                EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.TenantId, e.TenantId.ToString() } });
                await using var scope = EngineContext.Current.Resolve<IServiceProvider>().CreateAsyncScope();
                var bus = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                var command = new CreateStudentCommand(e.RegisterUserId);
                await bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    _logger.LogError($"Handling 'UpdateStudentPaidEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            });

        _semaphore.Release();
    }
}