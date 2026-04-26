using HaoKao.Common.Extensions;

namespace HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

public class ChapterNodeCommandHandler(
    IUnitOfWork<ChapterNode> uow,
    IChapterNodeRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateChapterNodeCommand, bool>,
    IRequestHandler<UpdateChapterNodeCommand, bool>,
    IRequestHandler<DeleteChapterNodeCommand, bool>
{
    private readonly IChapterNodeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateChapterNodeCommand request, CancellationToken cancellationToken)
    {
        var chapterNode = new ChapterNode
        {
            SubjectId = request.SubjectId,
            Code = request.Code,
            Name = request.Name,
            ParentId = request.ParentId,
            ParentName = request.ParentName,
            Sort = request.Sort
        };

        if (request.ParentId.HasValue)
        {
            var parentChapterNode = await _repository.GetByIdAsync(request.ParentId.Value);
            chapterNode.ParentIds = parentChapterNode != null && parentChapterNode.ParentIds != null
                ? parentChapterNode.ParentIds.Concat(new List<Guid> { request.ParentId.Value }).ToList()
                : [request.ParentId.Value,];
        }
        else
        {
            chapterNode.ParentIds = [];
        }

        await _repository.AddAsync(chapterNode);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(chapterNode, chapterNode.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateChapterNodeCommand request, CancellationToken cancellationToken)
    {
        var chapterNode = await _repository.GetByIdAsync(request.Id);
        if (chapterNode == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        chapterNode.SubjectId = request.SubjectId;
        chapterNode.Code = request.Code;
        chapterNode.Name = request.Name;
        chapterNode.ParentId = request.ParentId;
        chapterNode.ParentName = request.ParentName;
        chapterNode.Sort = request.Sort;

        if (request.ParentId.HasValue)
        {
            var parentChapterNode = await _repository.GetByIdAsync(request.ParentId.Value);
            chapterNode.ParentIds = parentChapterNode != null && parentChapterNode.ParentIds != null ? parentChapterNode.ParentIds.Concat(new List<Guid> { request.ParentId.Value }).ToList() : [request.ParentId.Value,];
        }
        else
        {
            chapterNode.ParentIds = [];
        }

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(chapterNode, chapterNode.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteChapterNodeCommand request, CancellationToken cancellationToken)
    {
        var children = await _repository.GetAsync(w => w.ParentId.Equals(request.Id));
        if (children != null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "请先删除子级节点", StatusCodes.Status400BadRequest),
                cancellationToken);
            return false;
        }

        var chapterNode = await _repository.GetByIdAsync(request.Id);
        if (chapterNode == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(chapterNode);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(chapterNode, chapterNode.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<ChapterNode>(cancellationToken);
        }

        return true;
    }
}