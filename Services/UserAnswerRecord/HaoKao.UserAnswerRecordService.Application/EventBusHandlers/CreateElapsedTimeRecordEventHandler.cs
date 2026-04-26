using HaoKao.Common.Events.UserAnswerRecord;
using HaoKao.UserAnswerRecordService.Domain.Commands;

namespace HaoKao.UserAnswerRecordService.Application.EventBusHandlers;

public class CreateElapsedTimeRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateElapsedTimeRecordEventHandler> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateElapsedTimeRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateElapsedTimeRecordEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateElapsedTimeRecordEvent))]
    public override async Task Handle(CreateElapsedTimeRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateElapsedTimeRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreateElapsedTimeRecordCommand(
                    e.SubjectId,
                    e.TargetId,
                    e.ProductId,
                    e.Type,
                    e.QuestionCount,
                    e.AnswerCount,
                    e.CorrectCount,
                    e.ElapsedSeconds,
                    e.CreatorId,
                    DateTime.Now
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateElapsedTimeRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}