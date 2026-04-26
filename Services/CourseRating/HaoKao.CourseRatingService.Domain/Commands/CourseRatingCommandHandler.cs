using HaoKao.Common.Extensions;
using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Domain.Enums;
using HaoKao.CourseRatingService.Domain.Repositories;

namespace HaoKao.CourseRatingService.Domain.Commands;

public class CourseRatingCommandHandler(
    IUnitOfWork<CourseRating> uow,
    IMediatorHandler bus,
    ICourseRatingRepository repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateCourseRatingCommand, bool>,
    IRequestHandler<DeleteCourseRatingCommand, bool>,
    IRequestHandler<AuditCourseRatingCommand, bool>,
    IRequestHandler<StickyCourseRatingCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(CreateCourseRatingCommand request, CancellationToken cancellationToken)
    {
        var entity = new CourseRating
        {
            CourseId = request.CourseId,
            CourseName = request.CourseName,
            Comment = request.Comment,
            Rating = request.Rating,
            AuditState = AuditState.InAudit,
            Sticky = false,
            NickName = request.NickName,
            Avatar = request.Avatar
        };

        await repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteCourseRatingCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return await NotFound(request.Id, cancellationToken);
        }

        await repository.DeleteAsync(entity);

        if (await Commit())
        {
            await RemoveEntityCache(entity.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(AuditCourseRatingCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return await NotFound(request.Id, cancellationToken);
        }

        entity.AuditState = request.AuditState;
        entity.UpdateTime = DateTime.Now;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(StickyCourseRatingCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return await NotFound(request.Id, cancellationToken);
        }

        entity.Sticky = request.Sticky;
        entity.UpdateTime = DateTime.Now;

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    private async Task<bool> NotFound(Guid id, CancellationToken cancellationToken)
    {
        var notification = new DomainNotification(id.ToString(), "未找到对应课程评价的数据", StatusCodes.Status404NotFound);
        await _bus.RaiseEvent(notification, cancellationToken);
        return false;
    }

    #region 缓存操作

    private Task UpdateEntityCache(CourseRating entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<CourseRating>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<CourseRating>(cancellationToken);
    }

    #endregion
}