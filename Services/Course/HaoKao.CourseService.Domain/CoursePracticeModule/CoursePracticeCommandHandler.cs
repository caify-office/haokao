using HaoKao.Common.Extensions;

namespace HaoKao.CourseService.Domain.CoursePracticeModule;

public class CoursePracticeCommandHandler(
    IUnitOfWork<CoursePractice> uow,
    IMediatorHandler bus,
    ICoursePracticeRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCoursePracticeCommand, bool>,
    IRequestHandler<SaveAssistantCoursePracticeCommand, bool>,
    IRequestHandler<UpdateCoursePracticeCommand, bool>,
    IRequestHandler<DeleteCoursePracticeCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly ICoursePracticeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateCoursePracticeCommand request, CancellationToken cancellationToken)
    {
        var exist = await _repository.ExistEntityAsync(w => w.CourseChapterId.Equals(request.CourseChapterId));
        if (exist)
        {
            await _bus.RaiseEvent(new DomainNotification(request.CourseChapterName.ToString(), "已存在对应课后练习的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        var coursePractice = _mapper.Map<CoursePractice>(request);

        await _repository.AddAsync(coursePractice);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CoursePractice>.ByIdCacheKey.Create(coursePractice.CourseChapterId.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(coursePractice, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CoursePractice>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SaveAssistantCoursePracticeCommand request, CancellationToken cancellationToken)
    {
        var coursePractice=await _repository.GetAsync(x=>x.CourseChapterId==request.CourseChapterId&&x.KnowledgePointId==request.KnowledgePointId&&x.CourseId==request.CourseId);
        if (coursePractice is null)
        {
            coursePractice = _mapper.Map<CoursePractice>(request);
            await _repository.AddAsync(coursePractice);
           
        }
        else
        {
            coursePractice = _mapper.Map(request, coursePractice);
            await _repository.UpdateAsync(coursePractice);
        }
       
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CoursePractice>.ByIdCacheKey.Create(coursePractice.CourseChapterId.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(coursePractice, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CoursePractice>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCoursePracticeCommand request, CancellationToken cancellationToken)
    {
        var coursePractice = await _repository.GetByIdAsync(request.Id);
        if (coursePractice == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应课后练习的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        coursePractice = _mapper.Map(request, coursePractice);
        await _repository.UpdateAsync(coursePractice);
        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<CoursePractice>.ByIdCacheKey.Create(coursePractice.CourseChapterId.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(coursePractice, key, key.CacheTime), cancellationToken);
           await _bus.RemoveListCacheEvent<CoursePractice>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCoursePracticeCommand request, CancellationToken cancellationToken)
    {
        var coursePractice = await _repository.GetByIdAsync(request.Id);
        if (coursePractice == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应课后练习的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(coursePractice);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<CoursePractice>.ByIdCacheKey.Create(coursePractice.CourseChapterId.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<CoursePractice>(cancellationToken);
        }

        return true;
    }
}