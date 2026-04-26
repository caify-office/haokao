using HaoKao.Common.Extensions;
using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Domain.Repositories;

namespace HaoKao.PaperService.Domain.Commands;

public class PaperCommandHandler(
    IUnitOfWork<Paper> uow,
    IMediatorHandler bus,
    IPaperRepository repository,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreatePaperCommand, bool>,
    IRequestHandler<UpdatePaperCommand, bool>,
    IRequestHandler<UpdatePaperStructCommand, bool>,
    IRequestHandler<UpdateIsFreeCommand, bool>,
    IRequestHandler<UpdatePublishStateCommand, bool>,
    IRequestHandler<DeletePaperCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    /// <summary>
    /// 创建试卷
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(CreatePaperCommand request, CancellationToken cancellationToken)
    {
        var paper = mapper.Map<Paper>(request);

        await repository.AddAsync(paper);

        if (await Commit())
        {
            await UpdateEntityCache(paper, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新试卷
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdatePaperCommand request, CancellationToken cancellationToken)
    {
        var paper = await repository.GetByIdAsync(request.Id);
        if (paper == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应试卷的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        mapper.Map(request, paper);

        if (await Commit())
        {
            await UpdateEntityCache(paper, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新试卷结构
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdatePaperStructCommand request, CancellationToken cancellationToken)
    {
        var paper = await repository.GetByIdAsync(request.Id);
        if (paper == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应试卷的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        paper.StructJson = request.StructJson;
        paper.QuestionCount = request.QuestionCount;
        paper.TotalScore = request.TotalScore;

        if (await Commit())
        {
            await UpdateEntityCache(paper, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    /// <summary>
    /// 更新试卷是否限免
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdateIsFreeCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.IsFree, request.IsFree));
        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);
        return true;
    }

    /// <summary>
    /// 更新发布状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(UpdatePublishStateCommand request, CancellationToken cancellationToken)
    {
        await repository.ExecuteUpdateAsync(x => request.Ids.Contains(x.Id), s => s.SetProperty(x => x.State, request.PublishState));
        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);
        return true;
    }

    /// <summary>
    /// 删除试卷
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> Handle(DeletePaperCommand request, CancellationToken cancellationToken)
    {
        var paper = await repository.GetByIdAsync(request.Id);
        if (paper == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应试卷的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        await repository.DeleteAsync(paper);

        if (await Commit())
        {
            await RemoveEntityCache(paper.Id, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    #region 缓存操作

    private Task UpdateEntityCache(Paper entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<Paper>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<Paper>(cancellationToken);
    }

    #endregion
}