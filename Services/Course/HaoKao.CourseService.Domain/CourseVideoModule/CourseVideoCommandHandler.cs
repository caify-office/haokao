using HaoKao.Common.Extensions;

namespace HaoKao.CourseService.Domain.CourseVideoModule;

public class CourseVideoCommandHandler(
    IUnitOfWork<CourseVideo> uow,
    IMediatorHandler bus,
    ICourseVideoRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<SaveCourseVideoCommand, bool>,
    IRequestHandler<UpdateCourseVideoCommand, bool>,
    IRequestHandler<DeleteCourseVideoCommand, bool>,
    IRequestHandler<UpdateIsTryCommand, bool>,
    IRequestHandler<UpdateKnowledgePointCommand, bool>,
    IRequestHandler<CreateCourseVideoBatchCommand, bool>,
    IRequestHandler<BatchUpdateNameCommand, bool>,
    IRequestHandler<UpdateSortCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(IMapper));

    public async Task<bool> Handle(UpdateIsTryCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.IsTry, request.State));
        foreach (var key in request.Ids.Select(id => GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(id.ToString())))
        {
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        }
        //删除列表缓存
        await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(BatchUpdateNameCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.QzName, request.Name));
        foreach (var key in request.Ids.Select(id => GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(id.ToString())))
        {
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        }
        //删除列表缓存
        await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(CreateCourseVideoBatchCommand request, CancellationToken cancellationToken)
    {
        await repository.AddRangeAsync(request.Models);

        if (await Commit())
        {
            foreach (var courseChapter in request.Models)
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseChapter.Id.ToString());
                // 将新增的纪录放到缓存中
                await _bus.RaiseEvent(new SetCacheEvent(courseChapter, key, key.CacheTime), cancellationToken);
                   await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
            }
        }

        return true;
    }

    public async Task<bool> Handle(SaveCourseVideoCommand request, CancellationToken cancellationToken)
    {
        //单个添加只给智辅学习使用，同课程同知识点下如果已存在，则替换
        var courseVideo=await repository.GetAsync(x=>x.CourseChapterId==request.CourseChapterId&&x.KnowledgePointId==request.KnowledgePointId);

        if (courseVideo is null)
        {
            courseVideo = _mapper.Map<CourseVideo>(request);
            await repository.AddAsync(courseVideo);
        }
        else
        {
            courseVideo = _mapper.Map(request, courseVideo);
            await repository.UpdateAsync(courseVideo);
        }


        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateKnowledgePointCommand request, CancellationToken cancellationToken)
    {
        var courseVideo = await repository.GetByIdAsync(request.Id);
        if (courseVideo == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课程视频的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        courseVideo.KnowledgePointIds = request.KnowledgePointIds;
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSortCommand request, CancellationToken cancellationToken)
    {
        var courseVideo = await repository.GetByIdAsync(request.Id);
        if (courseVideo == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课程视频的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        courseVideo.Sort = request.Sort;
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCourseVideoCommand request, CancellationToken cancellationToken)
    {
        var courseVideo = await repository.GetByIdAsync(request.Id);
        if (courseVideo == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课程视频的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        courseVideo = _mapper.Map(request, courseVideo);
        await repository.UpdateAsync(courseVideo);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseVideo.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideo, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseVideoCommand request, CancellationToken cancellationToken)
    {
        var courseVideo = await repository.GetByIdAsync(request.Id);
        if (courseVideo == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程视频的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await repository.DeleteAsync(courseVideo);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<CourseVideo>.ByIdCacheKey.Create(courseVideo.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideo>(cancellationToken);
        }

        return true;
    }
}