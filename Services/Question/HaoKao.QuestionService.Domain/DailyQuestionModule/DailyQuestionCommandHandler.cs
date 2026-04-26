namespace HaoKao.QuestionService.Domain.DailyQuestionModule;

/// <summary>
/// 创建每日一题
/// </summary>
/// <param name="SubjectId">科目Id</param>
/// <param name="QuestionId">试题Id</param>
/// <param name="CreateTime">创建时间</param>
public record CreateDailyQuestionCommand(Guid SubjectId, Guid QuestionId, DateTime CreateTime) : Command("创建每日一题");

public class DailyQuestionCommandHandler(
    IUnitOfWork<DailyQuestion> uow,
    IMediatorHandler bus,
    IDailyQuestionRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateDailyQuestionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IDailyQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateDailyQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = new DailyQuestion
        {
            SubjectId = request.SubjectId,
            QuestionId = request.QuestionId,
            CreateDate = request.CreateTime,
        };

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            // 创建缓存Key 根据每天的时间写入缓存
            var key = GirvsEntityCacheDefaults<DailyQuestion>.ByIdCacheKey.Create(entity.CreateDate.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(entity, key, key.CacheTime), cancellationToken);
        }

        return true;
    }
}