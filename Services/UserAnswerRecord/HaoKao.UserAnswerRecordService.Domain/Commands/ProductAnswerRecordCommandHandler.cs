using HaoKao.UserAnswerRecordService.Domain.Entities;
using HaoKao.UserAnswerRecordService.Domain.Repositories;

namespace HaoKao.UserAnswerRecordService.Domain.Commands;

public class ProductAnswerRecordCommandHandler(
    IUnitOfWork<UserAnswerRecord> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IProductChapterAnswerRecordRepository chapterRepository,
    IProductPaperAnswerRecordRepository paperRepository,
    IProductKnowledgeAnswerRecordRepository knowledgeRepository
) : CommandHandler(uow, bus),
    IRequestHandler<EventCreateProductChapterAnswerRecordCommand, bool>,
    IRequestHandler<EventCreateProductPaperAnswerRecordCommand, bool>,
    IRequestHandler<EventCreateProductKnowledgeAnswerRecordCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(EventCreateProductChapterAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        // 避免重复添加
        if (await chapterRepository.ExistEntityAsync(x => x.CreatorId == request.CreatorId
                                                       && x.ProductId == request.ProductId
                                                       && x.ChapterId == request.ChapterId
                                                       && x.CreateTime == request.CreateTime))
        {
            return true;
        }

        var entity = mapper.Map<ProductChapterAnswerRecord>(request);
        entity.CreateDate = DateOnly.FromDateTime(entity.CreateTime);
        await chapterRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<ProductChapterAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<ProductChapterAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EventCreateProductPaperAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        // 避免重复添加
        if (await paperRepository.ExistEntityAsync(x => x.CreatorId == request.CreatorId
                                                     && x.ProductId == request.ProductId
                                                     && x.PaperId == request.PaperId
                                                     && x.CreateTime == request.CreateTime))
        {
            return true;
        }

        var entity = mapper.Map<ProductPaperAnswerRecord>(request);
        entity.CreateDate = DateOnly.FromDateTime(entity.CreateTime);
        await paperRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<ProductPaperAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<ProductPaperAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(EventCreateProductKnowledgeAnswerRecordCommand request, CancellationToken cancellationToken)
    {
        // 避免重复添加
        if (await knowledgeRepository.ExistEntityAsync(x => x.CreatorId == request.CreatorId
                                                         && x.ProductId == request.ProductId
                                                         && x.SubjectId == request.SubjectId
                                                         && x.ChapterId == request.ChapterId
                                                         && x.SectionId == request.SectionId
                                                         && x.KnowledgePointId == request.KnowledgePointId
                                                         && x.CreateTime == request.CreateTime))
        {
            return true;
        }

        var entity = mapper.Map<ProductKnowledgeAnswerRecord>(request);
        entity.CorrectRate = entity.AnswerRecord.CorrectCount * 100 / entity.AnswerRecord.QuestionCount;
        // 掌握情况：取最近一次课后练的正确率未掌握（没有做课后练或正确率为0-50%含50%）；待加强（正确率50-80%不含80%），已学握（80%及以上）
        entity.MasteryLevel = entity.CorrectRate switch
        {
            <= 50 => MasteryLevel.NotMastered,
            < 80 => MasteryLevel.NeedsImprovement,
            > 80 => MasteryLevel.Mastered,
            _ => throw new ArgumentOutOfRangeException(nameof(entity.MasteryLevel))
        };
        entity.CreateDate = DateOnly.FromDateTime(entity.CreateTime);
        await knowledgeRepository.AddAsync(entity);
        if (await Commit())
        {
            var cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.ListCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheListEvent(cacheKey), cancellationToken);
            cacheKey = GirvsEntityCacheDefaults<ProductKnowledgeAnswerRecord>.ByIdCacheKey.Create();
            await _bus.RaiseEvent(new RemoveCacheEvent(cacheKey), cancellationToken);
        }
        return true;
    }
}