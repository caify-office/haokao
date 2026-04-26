namespace HaoKao.CourseService.Domain.VideoStorageModule;

public class VideoStorageCommandHandler(
    IUnitOfWork<VideoStorage> uow,
    IVideoStorageRepository repository,
    IMediatorHandler bus
) : CommandHandler(uow, bus),
    IRequestHandler<SaveVideoStorageCommand, bool>
{
    private readonly IVideoStorageRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(SaveVideoStorageCommand request, CancellationToken cancellationToken)
    {
        var tenantId = EngineContext.Current.ClaimManager.IdentityClaim.GetTenantId<Guid>();
        var videoStorage = await _repository.GetAsync(w => w.TenantId == tenantId);

        if (videoStorage == null)
        {
            videoStorage = new VideoStorage
            {
                VideoStorageHandlerId = request.VideoStorageHandlerId,
                VideoStorageHandlerName = request.VideoStorageHandlerName,
                ConfigParameter = request.ConfigParameter,
            };

            await _repository.AddAsync(videoStorage);
        }
        else
        {
            videoStorage.VideoStorageHandlerId = request.VideoStorageHandlerId;
            videoStorage.VideoStorageHandlerName = request.VideoStorageHandlerName;
            videoStorage.ConfigParameter = request.ConfigParameter;
            await _repository.UpdateAsync(videoStorage);
        }

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<VideoStorage>.ByIdCacheKey.Create(tenantId.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(videoStorage, key, key.CacheTime), cancellationToken);
        }

        return true;
    }
}