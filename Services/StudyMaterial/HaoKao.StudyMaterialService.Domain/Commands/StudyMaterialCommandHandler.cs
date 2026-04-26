using HaoKao.Common.Extensions;
using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Domain.Repositories;

namespace HaoKao.StudyMaterialService.Domain.Commands;

public class StudyMaterialCommandHandler(
    IUnitOfWork<StudyMaterial> uow,
    IMediatorHandler bus,
    IStudyMaterialRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateStudyMaterialCommand, bool>,
    IRequestHandler<UpdateStudyMaterialCommand, bool>,
    IRequestHandler<DeleteStudyMaterialCommand, bool>,
    IRequestHandler<EnableStudyMaterialCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateStudyMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<StudyMaterial>(request);

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateStudyMaterialCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应学习资料的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = mapper.Map(request, entity);
        entity.Materials = request.Materials;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteStudyMaterialCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteRangeAsync(x => request.Ids.Contains(x.Id));

        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(EnableStudyMaterialCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.Enable, request.Enable));
        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(StudyMaterial entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<StudyMaterial>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveListCacheEvent<StudyMaterial>(cancellationToken);
    }

    #endregion
}