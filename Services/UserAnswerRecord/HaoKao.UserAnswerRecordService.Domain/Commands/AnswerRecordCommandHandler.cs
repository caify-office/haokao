using HaoKao.Common.Enums;
using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

public class AnswerRecordCommandHandler(
    IUnitOfWork<UserAnswerRecord> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IChapterAnswerRecordRepository chapterRepository,
    IPaperAnswerRecordRepository paperRepository,
    IDateAnswerRecordRepository dateRepository,
    IElapsedTimeRecordRepository elapsedTimeRepository
) : CommandHandler(uow, bus),
    IRequestHandler<EventCreateChapterAnswerRecordCommand, bool>,
    IRequestHandler<EventCreatePaperAnswerRecordCommand, bool>,
    IRequestHandler<EventCreateDateAnswerRecordCommand, bool>,
    IRequestHandler<EventCreateElapsedTimeRecordCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(EventCreateChapterAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        // 避免重复添加
        if (await chapterRepository.ExistEntityAsync(x => x.CreatorId == request.CreatorId
                                                       && x.SubjectId == request.SubjectId
                                                       && x.ChapterId == request.ChapterId
                                                       && x.SectionId == request.SectionId
                                                       && x.KnowledgePointId == request.KnowledgePointId
                                                       && x.CreateTime == request.CreateTime))
        {
            return true;
        }

        var entity = mapper.Map<ChapterAnswerRecord>(request);
        await chapterRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<ChapterAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EventCreatePaperAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        // 避免重复添加
        if (await paperRepository.ExistEntityAsync(x => x.CreatorId == request.CreatorId
                                                     && x.SubjectId == request.SubjectId
                                                     && x.PaperId == request.PaperId
                                                     && x.CreateTime == request.CreateTime))
        {
            return true;
        }

        var entity = mapper.Map<PaperAnswerRecord>(request);
        await paperRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<PaperAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<PaperAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EventCreateDateAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        if (await dateRepository.Exist(request.CreatorId, request.SubjectId, request.Date, request.Type))
        {
            var message = request.Type == SubmitAnswerType.TodayTask ? "今日任务不可重复练习" : "打卡失败, 重复打卡";
            var notification = new DomainNotification(request.Date.ToString(), message, StatusCodes.Status500InternalServerError);
            await _bus.RaiseEvent(notification, cancellationToken);
            return true;
        }

        var entity = mapper.Map<DateAnswerRecord>(request);
        await dateRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<DateAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EventCreateElapsedTimeRecordCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ElapsedTimeRecord>(request);
        entity.CreateDate = DateOnly.FromDateTime(entity.CreateTime);
        await elapsedTimeRepository.AddAsync(entity);
        return await Commit();
    }
}