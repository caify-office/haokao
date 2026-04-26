using HaoKao.Common.Extensions;
using HaoKao.KnowledgePointService.Domain.Commands.KnowledgePoint;
using HaoKao.KnowledgePointService.Domain.Entities;
using HaoKao.KnowledgePointService.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.KnowledgePointService.Domain.CommandHandlers;

public class KnowledgePointCommandHandler(
    IUnitOfWork<KnowledgePoint> uow,
    IMediatorHandler bus,
    IKnowledgePointRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateKnowledgePointCommand, bool>,
    IRequestHandler<UpdateKnowledgePointCommand, bool>,
    IRequestHandler<DeleteKnowledgePointCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var knowledgePoint = new KnowledgePoint
        {
            Remark = request.Remark,
            Name = request.Name,
            ChapterNodeId = request.ChapterNodeId,
            ChapterNodeName = request.ChapterNodeName,
            SubjectName = request.SubjectName
        };

        await repository.AddAsync(knowledgePoint);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(knowledgePoint, knowledgePoint.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应知识点的数据", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }
        entity.Remark = request.Remark;
        entity.Name = request.Name;
        entity.ChapterNodeId = request.ChapterNodeId;
        entity.ChapterNodeName = request.ChapterNodeName;

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应知识点的数据", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        await repository.DeleteAsync(entity);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<KnowledgePoint>(entity.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<KnowledgePoint>(cancellationToken);
        }

        return true;
    }
}