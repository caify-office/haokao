using HaoKao.Common.Events.UserAnswerRecord;
using HaoKao.UserAnswerRecordService.Domain.Commands;
using HaoKao.UserAnswerRecordService.Domain.Entities;

namespace HaoKao.UserAnswerRecordService.Application.EventBusHandlers;

public class CreateUserAnswerRecordEventHandler(
    ILocker locker,
    IMediatorHandler bus,
    ILogger<CreateUserAnswerRecordEventHandler> logger,
    INotificationHandler<DomainNotification> notifications,
    IServiceProvider serviceProvider
) : GirvsIntegrationEventHandler<CreateUserAnswerRecordEvent>(serviceProvider)
{
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILogger<CreateUserAnswerRecordEventHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DomainNotificationHandler _notifications = notifications as DomainNotificationHandler ?? throw new ArgumentNullException(nameof(notifications));

    [CapSubscribe(nameof(CreateUserAnswerRecordEvent))]
    public override async Task Handle(CreateUserAnswerRecordEvent e, [FromCap] CapHeader header, CancellationToken cancellationToken)
    {
        // 需要重新设置身份认证头
        EngineContext.Current.ClaimManager.CapEventBusReSetClaim(header);

        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation($"Handling 'CreateUserEvent' eventId:{e.Id}");

        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromSeconds(cacheConfig.DefaultCacheTime);

        await _locker.PerformActionWithLockAsync
        (
            e.Id.ToString(), timeSpan,
            async () =>
            {
                // 这里调用命令，具体处理逻辑在命令类中
                var command = new EventCreateUserAnswerRecordCommand
                (
                    e.SubjectId,
                    e.CategoryId,
                    e.RecordIdentifier,
                    e.RecordIdentifierName,
                    e.ElapsedTime,
                    e.PassingScore,
                    e.TotalScore,
                    e.AnswerCount,
                    e.CorrectCount,
                    e.AnswerType,
                    e.CreateTime,
                    e.CreatorId,
                    e.RecordQuestions.Select(x => new UserAnswerQuestion
                    {
                        QuestionId = x.QuestionId,
                        ParentId = x.ParentId,
                        QuestionTypeId = x.QuestionTypeId,
                        WhetherMark = x.WhetherMark,
                        UserScore = x.UserScore,
                        JudgeResult = x.JudgeResult,
                        QuestionOptions = x.QuestionOptions.Select(o => new UserQuestionOption
                        {
                            OptionId = o.OptionId,
                            OptionContent = o.OptionContent,
                        }).ToList(),
                    }).ToList()
                );
                await _bus.SendCommand(command, cancellationToken);

                if (_notifications.HasNotifications())
                {
                    var errorMessage = _notifications.GetNotificationMessage();
                    // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                    _logger.LogError($"Handling 'CreateUserEvent' event Error Code:{400},Message:{errorMessage}", e);
                }
            }
        );
    }
}