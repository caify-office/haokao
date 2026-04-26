using HaoKao.Common.Extensions;

namespace HaoKao.CourseService.Domain.CourseModule;

public class CourseCommandHandler(
    IUnitOfWork<Course> uow,
    ICourseRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseCommand, bool>,
    IRequestHandler<UpdateCourseCommand, bool>,
    IRequestHandler<DeleteCourseCommand, bool>,
    IRequestHandler<UpdateEnableStateCommand, bool>,
    IRequestHandler<UpdateCourseMaterialsPackageUrlCommand, bool>
{
    private readonly ICourseRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = mapper.Map<Course>(request);

        await _repository.AddAsync(course);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(course, course.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Course>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id);
        if (course == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        course = mapper.Map(request, course);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(course, course.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Course>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id);
        if (course == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(course);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<Course>(course.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Course>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateEnableStateCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.State, request.State));
        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<Course>(id.ToString(), cancellationToken);
        }
        await _bus.RemoveListCacheEvent<Course>(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(UpdateCourseMaterialsPackageUrlCommand request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id);
        if (course == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课程的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        course = mapper.Map(request, course);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(course, course.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<Course>(cancellationToken);
        }

        return true;
    }
}