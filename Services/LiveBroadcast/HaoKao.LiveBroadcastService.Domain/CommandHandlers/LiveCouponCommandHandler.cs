using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveCouponCommandHandler(
    IUnitOfWork<LiveCoupon> uow,
    ILiveCouponRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveCouponCommand, bool>,
    IRequestHandler<UpdateLiveCouponCommand, bool>,
    IRequestHandler<SetLiveCouponShelvesCommand, bool>,
    IRequestHandler<DeleteLiveCouponCommand, bool>
{
    private readonly ILiveCouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveCouponCommand request, CancellationToken cancellationToken)
    {
        var list = _mapper.Map<List<LiveCoupon>>(request.models);

        await _repository.AddRangeAsync(list);

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<LiveCoupon>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLiveCouponCommand request, CancellationToken cancellationToken)
    {
        var ids = request.models.Select(x => x.Id);
        var existModel = await _repository.GetWhereAsync(x => ids.Contains(x.Id));
        if (!existModel.Any())
        {
            await _bus.RaiseEvent(
                new DomainNotification("", "未找到对应直播优惠卷的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }
        //调整排序
        foreach (var x in existModel)
        {
            var requestModel = request.models.FirstOrDefault(y => y.Id == x.Id);
            if (requestModel != null)
            {
                _mapper.Map(requestModel, x);
            }
        }

        await _repository.UpdateRangeAsync(existModel);

        if (await Commit())
        {
            await _bus.RemoveListCacheEvent<LiveCoupon>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetLiveCouponShelvesCommand request, CancellationToken cancellationToken)
    {
        await repository.UpdateIsShelvesByIds(request.Ids, request.IsShelves);

        foreach (var id in request.Ids)
        {
            await _bus.RemoveIdCacheEvent<LiveCoupon>(id.ToString(), cancellationToken);
        }

        await _bus.RemoveListCacheEvent<LiveCoupon>(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(DeleteLiveCouponCommand request, CancellationToken cancellationToken)
    {
        var liveCoupon = await _repository.GetByIdAsync(request.Id);
        if (liveCoupon == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播优惠卷的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(liveCoupon);

        if (await Commit())
        {
            await _bus.RemoveIdCacheEvent<LiveCoupon>(liveCoupon.Id.ToString(), cancellationToken);
            await _bus.RemoveListCacheEvent<LiveCoupon>(cancellationToken);
        }

        return true;
    }
}