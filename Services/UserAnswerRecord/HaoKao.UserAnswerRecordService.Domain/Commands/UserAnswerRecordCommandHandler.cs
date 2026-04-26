using Girvs.Infrastructure;
using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

public class UserAnswerRecordCommandHandler(
    IUnitOfWork<UserAnswerRecord> uow,
    IUserAnswerRecordRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<EventCreateUserAnswerRecordCommand, bool>
{
    private readonly IUserAnswerRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(EventCreateUserAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        if (request.AnswerType == SubmitAnswerType.TodayTask)
        {
            var id = await _repository.GetTodayTaskRecordId(request.SubjectId);
            if (id != Guid.Empty)
            {
                var notification = new DomainNotification(id.ToString(), "今日任务不可重复练习", StatusCodes.Status500InternalServerError);
                await _bus.RaiseEvent(notification, cancellationToken);
                return true;
            }
        }

        if (request.AnswerType == SubmitAnswerType.Daily)
        {
            if (await _repository.Query.AnyAsync(x => x.SubjectId == request.SubjectId
                                                   && x.CreatorId == request.CreatorId
                                                   && x.AnswerType == SubmitAnswerType.Daily
                                                   && x.CreateTime.Year == request.CreateTime.Year
                                                   && x.CreateTime.Month == request.CreateTime.Month
                                                   && x.CreateTime.Day == request.CreateTime.Day,
                                                 cancellationToken: cancellationToken))
            {
                var notification = new DomainNotification(request.CreateTime.ToString("yyyy-MM-dd"), "打卡失败, 重复打卡", StatusCodes.Status500InternalServerError);
                await _bus.RaiseEvent(notification, cancellationToken);
                return true;
            }
        }

        var userAnswerRecord = new UserAnswerRecord
        {
            AnswerType = request.AnswerType,
            SubjectId = request.SubjectId,
            QuestionCategoryId = request.CategoryId,
            RecordIdentifier = request.RecordIdentifier,
            RecordIdentifierName = request.RecordIdentifierName,
            ElapsedTime = request.ElapsedTime,
            PassingScore = request.PassingScore,
            TotalScore = request.TotalScore,
            QuestionCount = request.RecordQuestions.Count,
            AnswerCount = request.AnswerCount,
            CorrectCount = request.CorrectCount,
            RecordQuestions = request.RecordQuestions,
            CreateTime = request.AnswerType == SubmitAnswerType.Daily ? request.CreateTime : DateTime.Now,
            CreatorId = request.CreatorId
        };

        await _repository.AddAsync(userAnswerRecord);

        if (await Commit())
        {
            var tenantKey = $"TenantId_{EngineContext.Current.ClaimManager.IdentityClaim.TenantId}";
            var userKey = $"CreatorId_{request.CreatorId}";
            var cacheKey = new CacheKey($"{nameof(UserAnswerRecord).ToLowerInvariant()}:{tenantKey}:{userKey}:List").Create();

            var logger = EngineContext.Current.Resolve<ILogger<UserAnswerRecordCommandHandler>>();
            logger.LogInformation("清除用户答题记录缓存, 缓存key: {CacheKeyPrefix}", cacheKey.Prefixes);

            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
        }

        return true;
    }
}