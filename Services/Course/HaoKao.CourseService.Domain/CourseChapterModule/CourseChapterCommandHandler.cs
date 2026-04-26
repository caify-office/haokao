using HaoKao.Common.Extensions;
using HaoKao.CourseService.Domain.CourseMaterialsModule;
using HaoKao.CourseService.Domain.CourseVideoModule;

namespace HaoKao.CourseService.Domain.CourseChapterModule;

public class CourseChpaterCommandHandler(
    IUnitOfWork<CourseChapter> uow,
    ICourseChapterRepository repository,
    ICourseVideoRepository videoRepository,
    ICourseMaterialsRepository courseMaterialsRepository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseChapterCommand, bool>,
    IRequestHandler<UpdateCourseChapterCommand, bool>,
    IRequestHandler<DeleteCourseChapterCommand, bool>,
    IRequestHandler<CreateCourseChapterBatchCommand, bool>,
    IRequestHandler<BatchDeleteCourseChapterCommand, bool>
{
    private readonly ICourseChapterRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ICourseVideoRepository _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
    private readonly ICourseMaterialsRepository _courseMaterialsRepository = courseMaterialsRepository ?? throw new ArgumentNullException(nameof(courseMaterialsRepository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCourseChapterBatchCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddRangeAsync(request.List);

        if (await Commit())
        {
            foreach (var courseChapter in request.List)
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<CourseChapter>.ByIdCacheKey.Create(courseChapter.Id.ToString());
                // 将新增的纪录放到缓存中
                await _bus.RaiseEvent(new SetCacheEvent(courseChapter, key, key.CacheTime), cancellationToken);
                   await _bus.RemoveListCacheEvent<CourseChapter>(cancellationToken);
            }
        }

        return true;
    }

    public async Task<bool> Handle(CreateCourseChapterCommand request, CancellationToken cancellationToken)
    {
        var courseChapter = mapper.Map<CourseChapter>(request);

        await _repository.AddAsync(courseChapter);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseChapter>.ByIdCacheKey.Create(courseChapter.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseChapter, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseChapter>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(BatchDeleteCourseChapterCommand request, CancellationToken cancellationToken)
    {
        //判断课程章节下面是否存在视频或者讲义
        var courseMaterial = await _courseMaterialsRepository.MaterialsCount(request.Id);
        var videoCount = await _videoRepository.VideoCount(request.Id);
        if (int.Parse(videoCount.ToString()) > 0 || int.Parse(courseMaterial.ToString()) > 0)
        {
            await _bus.RaiseEvent(new DomainNotification("", "章节节点下面存在视频或讲义,请先删除", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }
        //清空目录
        var rows = await _repository.ClearCourseChapter(request.Id);
        if (rows > 0)
        {
            await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<CourseChapter>.ByTenantKey.Create()), cancellationToken);
        }
        return true;
    }

    public async Task<bool> Handle(UpdateCourseChapterCommand request, CancellationToken cancellationToken)
    {
        var courseChapter = await _repository.GetByIdAsync(request.Id);
        if (courseChapter == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课程章节的数据", StatusCodes.Status404NotFound), cancellationToken);
            return false;
        }

        courseChapter = mapper.Map(request, courseChapter);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseChapter>.ByIdCacheKey.Create(courseChapter.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseChapter, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseChapter>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseChapterCommand request, CancellationToken cancellationToken)
    {
        var children = await _repository.GetAsync(w => w.ParentId.Equals(request.Id));
        if (children != null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "请先删除子级节点", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }
        var courseMaterial = await _courseMaterialsRepository.GetAsync(w => w.CourseChapterId.Equals(request.Id));
        var courseVideo = await _videoRepository.GetAsync(w => w.CourseChapterId.Equals(request.Id));
        if (courseVideo != null || courseMaterial != null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "章节节点下面存在视频或讲义,请先删除", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }
        var courseChapter = await _repository.GetByIdAsync(request.Id);
        if (courseChapter == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程章节的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(courseChapter);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<CourseChapter>.ByIdCacheKey.Create(courseChapter.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseChapter>(cancellationToken);
        }

        return true;
    }
}