using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.CommandHandlers;

public class DrawPrizeCommandHandler(
    IUnitOfWork<DrawPrize> uow,
    IDrawPrizeRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateDrawPrizeCommand, bool>,
    IRequestHandler<UpdateDrawPrizeCommand, bool>,
    IRequestHandler<SetDrawPrizeRuleCommand, bool>,
    IRequestHandler<SetDrawPrizeEnableCommand, bool>,
    IRequestHandler<DeleteDrawPrizeCommand, bool>
{
    private readonly IDrawPrizeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateDrawPrizeCommand request, CancellationToken cancellationToken)
    {
        var drawPrize = _mapper.Map<DrawPrize>(request);

        await _repository.AddAsync(drawPrize);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(drawPrize, drawPrize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateDrawPrizeCommand request, CancellationToken cancellationToken)
    {
        var drawPrize = await _repository.GetByIdAsync(request.Id);
        if (drawPrize == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应抽奖的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        drawPrize = _mapper.Map(request, drawPrize);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(drawPrize, drawPrize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetDrawPrizeRuleCommand request, CancellationToken cancellationToken)
    {
        var drawPrize = await _repository.GetByIdAsync(request.Id);
        if (drawPrize == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应抽奖的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        drawPrize = _mapper.Map(request, drawPrize);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(drawPrize, drawPrize.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetDrawPrizeEnableCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateEnableByIds(request.Ids, request.Enable);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<DrawPrize>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(DeleteDrawPrizeCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIds(request.Ids);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<DrawPrize>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<DrawPrize>(cancellationToken);

        return true;
    }
}