using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Domain.CommandHandlers;

public class DrawPrizeRecordCommandHandler(
    IUnitOfWork<DrawPrizeRecord> uow,
    IDrawPrizeRecordRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateDrawPrizeRecordCommand, bool>,
    IRequestHandler<UpdateDrawPrizeRecordCommand, bool>,
    IRequestHandler<DeleteDrawPrizeRecordCommand, bool>
{
    private readonly IDrawPrizeRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateDrawPrizeRecordCommand request, CancellationToken cancellationToken)
    {
        var drawPrizeRecord = _mapper.Map<DrawPrizeRecord>(request);

        await _repository.AddAsync(drawPrizeRecord);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(drawPrizeRecord, drawPrizeRecord.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrizeRecord>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateDrawPrizeRecordCommand request, CancellationToken cancellationToken)
    {
        var drawPrizeRecord = await _repository.GetByIdAsync(request.Id);
        if (drawPrizeRecord == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应抽奖记录的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        drawPrizeRecord = _mapper.Map(request, drawPrizeRecord);

        if (await Commit())
        {
            await _bus.UpdateIdCacheEvent(drawPrizeRecord, drawPrizeRecord.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrizeRecord>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteDrawPrizeRecordCommand request, CancellationToken cancellationToken)
    {
        var drawPrizeRecord = await _repository.GetByIdAsync(request.Id);
        if (drawPrizeRecord == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应抽奖记录的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(drawPrizeRecord);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<DrawPrizeRecord>(drawPrizeRecord.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<DrawPrizeRecord>(cancellationToken);
        }

        return true;
    }
}