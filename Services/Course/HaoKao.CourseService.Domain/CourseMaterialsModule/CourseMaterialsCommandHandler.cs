using HaoKao.Common.Extensions;

namespace HaoKao.CourseService.Domain.CourseMaterialsModule;

public class CourseMaterialsCommandHandler(
    IUnitOfWork<CourseMaterials> uow,
    ICourseMaterialsRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseMaterialsCommand, bool>,
    IRequestHandler<SaveCourseMaterialsCommand, bool>,
    IRequestHandler<SetCourseMaterialsSortCommand, bool>,
    IRequestHandler<DeleteCourseMaterialsCommand, bool>
{
    private readonly ICourseMaterialsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateCourseMaterialsCommand request, CancellationToken cancellationToken)
    {
        var courseMaterials = _mapper.Map<CourseMaterials>(request);
        await _repository.AddAsync(courseMaterials);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseMaterials>.ByIdCacheKey.Create(courseMaterials.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseMaterials, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseMaterials>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SaveCourseMaterialsCommand request, CancellationToken cancellationToken)
    {
        var courseMaterials = await _repository.GetAsync(x => x.CourseChapterId == request.CourseChapterId && x.KnowledgePointId == request.KnowledgePointId);
        if (courseMaterials is null)
        {
            courseMaterials = _mapper.Map<CourseMaterials>(request);
            await _repository.AddAsync(courseMaterials);
        }
        else
        {
            courseMaterials = _mapper.Map(request, courseMaterials);
            await _repository.UpdateAsync(courseMaterials);
        }

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseMaterials>.ByIdCacheKey.Create(courseMaterials.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseMaterials, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseMaterials>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetCourseMaterialsSortCommand request, CancellationToken cancellationToken)
    {
        var courseMaterials = await _repository.GetByIdAsync(request.Id);
        if (courseMaterials == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程讲义的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        courseMaterials.Sort = request.Sort;
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseMaterials>.ByIdCacheKey.Create(courseMaterials.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseMaterials, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseMaterials>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseMaterialsCommand request, CancellationToken cancellationToken)
    {
        var courseMaterials = await _repository.GetByIdAsync(request.Id);
        if (courseMaterials == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程讲义的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(courseMaterials);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<CourseMaterials>.ByIdCacheKey.Create(courseMaterials.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseMaterials>(cancellationToken);
        }

        return true;
    }
}