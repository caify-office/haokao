using AutoMapper;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using HaoKao.Common.Extensions;

namespace HaoKao.ChapterNodeService.Domain.KnowledgePointModule;

public class KnowledgePointCommandHandler(
    IUnitOfWork<KnowledgePoint> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IKnowledgePointRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateKnowledgePointCommand, bool>,
    IRequestHandler<UpdateKnowledgePointCommand, bool>,
    IRequestHandler<DeleteKnowledgePointCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<KnowledgePoint>(request);

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应知识点的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        entity = mapper.Map(request, entity);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应知识点的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<KnowledgePoint>(entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }
}