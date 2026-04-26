using HaoKao.Common.Events.UserAnswerRecord;
using HaoKao.UserAnswerRecordService.Domain.Commands;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.EventBusHandlers;

public class CreateChapterAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateChapterAnswerRecordEventHandler> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateChapterAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateChapterAnswerRecordEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateChapterAnswerRecordEvent))]
    public override async Task Handle(CreateChapterAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateChapterAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreateChapterAnswerRecordCommand(
                    e.SubjectId,
                    e.CategoryId,
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
                    _logger.LogError($"Handling 'CreateChapterAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}

public class CreatePaperAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreatePaperAnswerRecordEventHandler> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreatePaperAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreatePaperAnswerRecordEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreatePaperAnswerRecordEvent))]
    public override async Task Handle(CreatePaperAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreatePaperAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreatePaperAnswerRecordCommand(
                    e.SubjectId,
                    e.CategoryId,
                    e.PaperId,
                    e.CreatorId,
                    DateTime.Now,
                    e.UserScore,
                    e.PassingScore,
                    e.TotalScore,
                    e.ElapsedTime,
                    e.AnswerRecord.ConvertToAnswerRecord(e.SubjectId)
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreatePaperAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}

public class CreateDateAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    IServiceProvider serviceProvider,
    ILogger<CreateDateAnswerRecordEventHandler> logger,
    INotificationHandler<DomainNotification> notifications
) : GirvsIntegrationEventHandler<CreateDateAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateDateAnswerRecordEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateDateAnswerRecordEvent))]
    public override async Task Handle(CreateDateAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        _logger.LogInformation("Handling 'CreateDateAnswerRecordEvent' eventId:{EId}", e.Id);

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreateDateAnswerRecordCommand(
                    e.SubjectId,
                    e.CreatorId,
                    DateTime.Now,
                    e.Date,
                    e.Type,
                    e.AnswerRecord.ConvertToAnswerRecord(e.SubjectId)
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    _logger.LogError($"Handling 'CreateDateAnswerRecordEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}

public static class Extension
{
    public static AnswerRecord ConvertToAnswerRecord(this CreateAnswerRecordEvent e, Guid subjectId)
    {
        return new AnswerRecord
        {
            SubjectId = subjectId,
            QuestionCount = e.QuestionCount,
            AnswerCount = e.AnswerCount,
            CorrectCount = e.CorrectCount,
            AnswerType = e.AnswerType,
            CreatorId = e.CreatorId,
            CreateTime = DateTime.Now,
            Questions = e.Questions.Select(x => new AnswerQuestion
            {
                QuestionId = x.QuestionId,
                QuestionTypeId = x.QuestionTypeId,
                JudgeResult = x.JudgeResult,
                WhetherMark = x.WhetherMark,
                ParentId = x.ParentId,
                CreatorId = e.CreatorId,
                CreateTime = DateTime.Now,
                UserAnswers = x.UserAnswers.Select(y => new UserAnswer(y.Content)).ToList()
            }).ToList()
        };
    }
}