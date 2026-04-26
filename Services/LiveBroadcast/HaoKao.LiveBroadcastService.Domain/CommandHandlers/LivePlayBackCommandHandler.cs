using AutoMapper;
using HaoKao.Common.Extensions;
using HaoKao.LiveBroadcastService.Domain.Commands.LivePlayBack;
using HaoKao.LiveBroadcastService.Domain.Entities;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class LivePlayBackCommandHandler(
    IUnitOfWork<LivePlayBack> uow,
    ILivePlayBackRepository repository,
    IMediatorHandler bus,
    IMapper mapper,
    ILiveVideoRepository liveVideoRepository
) : CommandHandler(uow, bus),
    IRequestHandler<CreateLivePlayBackCommand, bool>,
    IRequestHandler<UpdateLivePlayBackCommand, bool>,
    IRequestHandler<DeleteLivePlayBackCommand, bool>
{
    private readonly ILiveVideoRepository _liveVideoRepository = liveVideoRepository ?? throw new ArgumentNullException(nameof(liveVideoRepository));
    private readonly ILivePlayBackRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateLivePlayBackCommand request, CancellationToken cancellationToken)
    {
        var liveVideo = await _liveVideoRepository.GetByIdAsync(request.models.FirstOrDefault().LiveVideoId);
        if (liveVideo is null)
        {
           await  _bus.RaiseEvent(new DomainNotification("", "当前直播不存在", StatusCodes.Status400BadRequest), cancellationToken);
            return false;
        }
        var livePlayBackList = _mapper.Map<List<LivePlayBack>>(request.models);
       
        await _repository.AddRangeAsync(livePlayBackList);
        liveVideo.IsUploadPlayBack = true;
        if (await Commit())
        {
           await _bus.RemoveListCacheEvent<LivePlayBack>(cancellationToken);

            await RemoveLiveVideoCache(liveVideo.Id, cancellationToken);
        }

        return true;
    }

    private async Task RemoveLiveVideoCache(Guid liveVideoId, CancellationToken cancellationToken)
    {
        // 创建缓存Key
        var key = GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(liveVideoId.ToString());
        // 将新增的纪录放到缓存中
        await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        // 删除查询相关的缓存
        await _bus.RemoveListCacheEvent<LiveVideo>(cancellationToken);
    }

    public async Task<bool> Handle(UpdateLivePlayBackCommand request, CancellationToken cancellationToken)
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
           await _bus.RemoveListCacheEvent<LivePlayBack>(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteLivePlayBackCommand request, CancellationToken cancellationToken)
    {
   
        var livePlayBack = await _repository.GetIncludeLiveVideo(request.Id);
        if (livePlayBack == null)
        {
            await _bus.RaiseEvent(
                new DomainNotification(request.Id.ToString(), "未找到对应直播回放的数据", StatusCodes.Status404NotFound),
                cancellationToken);
            return false;
        }

        await _repository.DeleteAsync(livePlayBack);
        livePlayBack.LiveVideo.IsUploadPlayBack = await _repository.ExistEntityAsync(x => x.LiveVideoId == livePlayBack.LiveVideoId&x.Id!=request.Id);

        if (await Commit())
        {
            var key = GirvsEntityCacheDefaults<LivePlayBack>.ByIdCacheKey.Create(livePlayBack.Id.ToString());
            await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
           await _bus.RemoveListCacheEvent<LivePlayBack>(cancellationToken);

            await RemoveLiveVideoCache(livePlayBack.LiveVideoId, cancellationToken);
        }

        return true;
    }
}