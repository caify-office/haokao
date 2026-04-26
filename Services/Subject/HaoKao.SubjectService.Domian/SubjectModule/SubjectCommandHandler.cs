using System;
using System.Threading;
using System.Threading.Tasks;

namespace HaoKao.SubjectService.Domain.SubjectModule;

public class SubjectCommandHandler(
    IUnitOfWork<Subject> uow,
    ISubjectRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSubjectCommand, bool>,
    IRequestHandler<UpdateSubjectCommand, bool>,
    IRequestHandler<DeleteSubjectCommand, bool>
{
    private readonly ISubjectRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = new Subject
        {
            Name = request.Name,
            IsCommon = request.IsCommon,
            Sort = request.Sort,
            IsShow = request.IsShow
        };

        await _repository.AddAsync(subject);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Subject>.ByIdCacheKey.Create(subject.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(subject, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Subject>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repository.GetByIdAsync(request.Id);
        if (subject == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应科目的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        subject.Name = request.Name;
        subject.IsCommon = request.IsCommon;
        subject.Sort = request.Sort;
        subject.IsShow = request.IsShow;

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<Subject>.ByIdCacheKey.Create(subject.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(subject, key, key.CacheTime), cancellationToken);
            //删除列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Subject>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _repository.GetByIdAsync(request.Id);
        if (subject == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应科目的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(subject);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<Subject>.ByIdCacheKey.Create(subject.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<Subject>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }
}