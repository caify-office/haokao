using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.AgreementService.Domain.Commands;

public class CourseAgreementCommandHandler(
    IUnitOfWork<CourseAgreement> uow,
    ICourseAgreementRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseAgreementCommand, bool>,
    IRequestHandler<UpdateCourseAgreementCommand, bool>,
    IRequestHandler<DeleteCourseAgreementCommand, bool>
{
    private readonly ICourseAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateCourseAgreementCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<CourseAgreement>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateCourseAgreementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应课程协议的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseAgreementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应课程协议的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        if (await Commit())
        {
            await RemoveEntityCache(entity.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(CourseAgreement entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<CourseAgreement>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<CourseAgreement>(cancellationToken);
    }

    #endregion
}