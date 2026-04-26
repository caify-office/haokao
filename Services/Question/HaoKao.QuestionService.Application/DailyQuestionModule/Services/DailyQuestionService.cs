using HaoKao.QuestionService.Application.DailyQuestionModule.Interfaces;
using HaoKao.QuestionService.Application.DailyQuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.DailyQuestionModule;
using HaoKao.QuestionService.Domain.QuestionModule;

namespace HaoKao.QuestionService.Application.DailyQuestionModule.Services;

/// <summary>
/// 管理端服务，每日一题服务
/// </summary>
public class DailyQuestionService(
    IStaticCacheManager cacheManager,
    IQuestionRepository questionRepository,
    IDailyQuestionRepository repository,
    IMediatorHandler bus,
    ILocker locker,
    INotificationHandler<DomainNotification> notifications
) : IDailyQuestionService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IQuestionRepository _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
    private readonly IDailyQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ILocker _locker = locker ?? throw new ArgumentNullException(nameof(locker));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <inheritdoc />
    public async Task<DailyQuestionViewModel> Get(Guid subjectId, DateTime dateTime)
    {
        //转为日期
        dateTime = dateTime.Date;

        var cacheKey = GirvsEntityCacheDefaults<DailyQuestion>.ByIdCacheKey.Create($"{subjectId}:{dateTime:yyyy-MM-dd}");
        var dailyQuestion = await _cacheManager.GetAsync(
            cacheKey, () => _repository.GetAsync(x => x.CreateDate == dateTime && x.SubjectId == subjectId)
        );

        Question question = null;
        var cacheConfig = EngineContext.Current.GetAppModuleConfig<CacheConfig>();
        var timeSpan = TimeSpan.FromMinutes(cacheConfig.DefaultCacheTime);
        await _locker.PerformActionWithLockAsync($"{cacheConfig.DistributedCacheConfig.InstanceName}{cacheKey.Key}", timeSpan, async () =>
            {
                if (dailyQuestion == null)
                {
                    question = await _questionRepository.ExtractDailyQuestionBySubjectId(
                        subjectId,
                        QuestionType.SingleChoice,
                        QuestionType.MultiChoice
                    );

                    if (question == null) return;

                    var command = new CreateDailyQuestionCommand(subjectId, question.Id, dateTime);

                    await _bus.SendCommand(command);

                    if (_notifications.HasNotifications())
                    {
                        var errorMessage = _notifications.GetNotificationMessage();
                        throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
                    }
                }
                else
                {
                    var questionKey = GirvsEntityCacheDefaults<Question>.ByIdCacheKey.Create(dailyQuestion.QuestionId.ToString());
                    question = await _cacheManager.GetAsync
                    (
                        questionKey, () => _questionRepository.GetAsync(x => x.Id == dailyQuestion.QuestionId)
                    );
                }
            }
        );
        return question?.MapToDto<DailyQuestionViewModel>();
    }
}