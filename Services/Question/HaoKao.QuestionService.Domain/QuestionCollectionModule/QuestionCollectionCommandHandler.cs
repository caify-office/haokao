using Girvs.Infrastructure;
using HaoKao.QuestionService.Domain.CacheExtensions;

namespace HaoKao.QuestionService.Domain.QuestionCollectionModule;

public class QuestionCollectionCommandHandler(
    IUnitOfWork<QuestionCollection> uow,
    IMediatorHandler bus,
    IRepository<QuestionCollection> repository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateQuestionCollectionCommand, bool>,
    IRequestHandler<DeleteQuestionCollectionCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<QuestionCollection> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(CreateQuestionCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var exist = await _repository.ExistEntityAsync(x => x.QuestionId == request.QuestionId && x.CreatorId == userId);

        if (exist)
        {
            await _bus.RaiseEvent(new DomainNotification(request.QuestionId.ToString(), "该试题已收藏", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }

        var entity = new QuestionCollection(request.QuestionId);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            //删除当前用户的列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(QuestionCollectionCacheManager.ListPrefix.Create()), cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<bool> Handle(DeleteQuestionCollectionCommand request, CancellationToken cancellationToken)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        var entity = await _repository.GetAsync(x => x.QuestionId == request.QuestionId && x.CreatorId == userId);

        if (entity == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.QuestionId.ToString(), "该试题已取消收藏", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(entity);

        if (await Commit())
        {
            //删除当前用户的列表缓存
            await _bus.RaiseEvent(new RemoveCacheListEvent(QuestionCollectionCacheManager.ListPrefix.Create()), cancellationToken);
            return true;
        }

        return false;
    }
}