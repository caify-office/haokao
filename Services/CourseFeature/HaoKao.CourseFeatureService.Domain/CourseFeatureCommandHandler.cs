using HaoKao.Common.Extensions;

namespace HaoKao.CourseFeatureService.Domain;

public class CourseFeatureCommandHandler(
    IUnitOfWork<CourseFeature> uow,
    IMediatorHandler bus,
    ICourseFeatureRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseFeatureCommand, bool>,
    IRequestHandler<UpdateCourseFeatureCommand, bool>,
    IRequestHandler<DeleteCourseFeatureCommand, bool>,
    IRequestHandler<EnableCourseFeatureCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCourseFeatureCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<CourseFeature>(request);

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCourseFeatureCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应课程特色服务的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = mapper.Map(request, entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseFeatureCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteRangeAsync(x => request.Ids.Contains(x.Id));

        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(EnableCourseFeatureCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.Enable, request.Enable));
        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(CourseFeature entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<CourseFeature>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<CourseFeature>(cancellationToken);
    }

    #endregion
}