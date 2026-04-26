using HaoKao.Common.Enums;
using HaoKao.Common.Events.UserAnswerRecord;
using HaoKao.UserAnswerRecordService.Domain.Commands;

namespace HaoKao.UserAnswerRecordService.Application.EventBusHandlers;

public class CreateProductChapterAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateProductChapterAnswerRecordEvent> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateProductChapterAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateProductChapterAnswerRecordEvent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateProductChapterAnswerRecordEvent))]
    public override async Task Handle(CreateProductChapterAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateProductChapterAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreateProductChapterAnswerRecordCommand(
                    e.ProductId,
                    e.SubjectId,
                    e.ChapterId,
                    e.SectionId,
                    e.KnowledgePointId,
                    e.CreatorId,
                    DateTime.Now,
                    e.AnswerRecord.ConvertToAnswerRecord(e.SubjectId)
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateProductChapterAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}

public class CreateProductPaperAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateProductPaperAnswerRecordEvent> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateProductPaperAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateProductPaperAnswerRecordEvent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateProductPaperAnswerRecordEvent))]
    public override async Task Handle(CreateProductPaperAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateProductPaperAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                var command = new EventCreateProductPaperAnswerRecordCommand(
                    e.ProductId,
                    e.SubjectId,
                    e.PaperId,
                    e.UserScore,
                    e.PassingScore,
                    e.TotalScore,
                    e.CreatorId,
                    DateTime.Now,
                    e.AnswerRecord.ConvertToAnswerRecord(e.SubjectId)
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateProductPaperAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}

public class CreateProductKnowledgeAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateProductKnowledgeAnswerRecordEvent> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateProductKnowledgeAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateProductKnowledgeAnswerRecordEvent> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateProductKnowledgeAnswerRecordEvent))]
    public override async Task Handle(CreateProductKnowledgeAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateProductKnowledgeAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                var command = new EventCreateProductKnowledgeAnswerRecordCommand(
                    e.ProductId,
                    e.SubjectId,
                    e.ChapterId,
                    e.SectionId,
                    e.KnowledgePointId,
                    e.CreatorId,
                    DateTime.Now,
                    (ExamFrequency)e.ExamFrequency,
                    e.AnswerRecord.ConvertToAnswerRecord(e.SubjectId)
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateProductKnowledgeAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}