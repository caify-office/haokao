using HaoKao.Common.Extensions;

namespace HaoKao.CourseService.Domain.CourseVideoNoteModule;

public class CourseVideoNoteCommandHandler(
    IUnitOfWork<CourseVideoNote> uow,
    ICourseVideoNoteRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseVideoNoteCommand, bool>,
    IRequestHandler<UpdateCourseVideoNoteCommand, bool>,
    IRequestHandler<DeleteCourseVideoNoteCommand, bool>,
    IRequestHandler<DeleteBatchCourseVideoNoteCommand, bool>
{
    private readonly ICourseVideoNoteRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCourseVideoNoteCommand request, CancellationToken cancellationToken)
    {
        var courseVideoNote = new CourseVideoNote
        {
            VideoId = request.VideoId,
            TimeNode = request.TimeNode,
            CourseVideoNoteType = request.CourseVideoNoteType,
            NoteContent = request.NoteContent,
        };

        await _repository.AddAsync(courseVideoNote);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideoNote>.ByIdCacheKey.Create(courseVideoNote.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideoNote, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideoNote>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCourseVideoNoteCommand request, CancellationToken cancellationToken)
    {
        var courseVideoNote = await _repository.GetByIdAsync(request.Id);
        if (courseVideoNote == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课程视频笔记的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        courseVideoNote.VideoId = request.VideoId;
        courseVideoNote.TimeNode = request.TimeNode;
        courseVideoNote.CourseVideoNoteType = request.CourseVideoNoteType;
        courseVideoNote.NoteContent = request.NoteContent;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CourseVideoNote>.ByIdCacheKey.Create(courseVideoNote.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(courseVideoNote, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideoNote>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseVideoNoteCommand request, CancellationToken cancellationToken)
    {
        var courseVideoNote = await _repository.GetByIdAsync(request.Id);
        if (courseVideoNote == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程视频笔记的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(courseVideoNote);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<CourseVideoNote>.ByIdCacheKey.Create(courseVideoNote.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<CourseVideoNote>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteBatchCourseVideoNoteCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteRangeAsync(x => x.VideoId == request.VideoId);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<CourseVideoNote>(cancellationToken);
        }

        return true;
    }
}