using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveProductPackage;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LiveProductPackageCommandHandler(
    IUnitOfWork<LiveProductPackage> uow,
    ILiveProductPackageRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLiveProductPackageCommand, bool>,
    IRequestHandler<UpdateLiveProductPackageCommand, bool>,
    IRequestHandler<DeleteLiveProductPackageCommand, bool>,
    IRequestHandler<SetLiveProductPackageShelvesCommand, bool>
{
    private readonly ILiveProductPackageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLiveProductPackageCommand request, CancellationToken cancellationToken)
    {
        var listProcuctPackgeList = _mapper.Map<List<LiveProductPackage>>(request.models);

        await _repository.AddRangeAsync(listProcuctPackgeList);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<LiveProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateLiveProductPackageCommand request, CancellationToken cancellationToken)
    {
        var ids = request.models.Select(x => x.Id);
        var existModel = await _repository.GetWhereAsync(x => ids.Contains(x.Id));
        if (!existModel.Any())
        {
            await _bus.RaiseEvent(new DomainNotification("", "未找到对应直播产品包的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }
        //调整排序
        existModel.ForEach(x =>
        {
            var requestModel = request.models.FirstOrDefault(y => y.Id == x.Id);
            if (requestModel != null)
            {
                x = _mapper.Map(requestModel, x);
            }
        });

        await _repository.UpdateRangeAsync(existModel);

        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<LiveProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLiveProductPackageCommand request, CancellationToken cancellationToken)
    {
        var liveProductPackage = await _repository.GetByIdAsync(request.Id);
        if (liveProductPackage == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播产品包的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(liveProductPackage);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<LiveProductPackage>.ByIdCacheKey.Create(liveProductPackage.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<LiveProductPackage>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(SetLiveProductPackageShelvesCommand request, CancellationToken cancellationToken)
    {
    
        await repository.UpdateIsShelvesByIds(request.Ids, request.IsShelves);
        //if (await Commit())
        {
            foreach (var id in request.Ids)
            {
                // 创建缓存Key
                var key = GirvsEntityCacheDefaults<LiveProductPackage>.ByIdCacheKey.Create(id.ToString());
                // 将新增的纪录放到缓存中
                await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
            }

           await _bus.RemoveListCacheEvent<LiveProductPackage>(cancellationToken);
        }

        return true;
    }
}