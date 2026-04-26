using AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Domain.CommandHandlers;

public class SensitiveWordCommandHandler(
    IUnitOfWork<SensitiveWord> uow,
    ISensitiveWordRepository repository,
    IMediatorHandler bus,
    IMapper mapper
) : CommandHandler(uow, bus),
    IRequestHandler<CreateSensitiveWordCommand, bool>,
    IRequestHandler<UpdateSensitiveWordCommand, bool>
{
    private readonly ISensitiveWordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<bool> Handle(CreateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = _mapper.Map<SensitiveWord>(request);
        await _repository.AddAsync(sensitiveWord);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<SensitiveWord>.ByIdCacheKey.Create(sensitiveWord.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(sensitiveWord, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<SensitiveWord>.ByTenantKey.Create()),
                                  cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await _repository.GetByIdAsync(request.Id);
        if (sensitiveWord == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "未找到对应敏感词的数据", StatusCodes.Status404NotFound),
                                  cancellationToken);
            return false;
        }

        sensitiveWord = _mapper.Map(request, sensitiveWord);

        if (await Commit())
        {
            // 创建缓存Key
            var key = GirvsEntityCacheDefaults<SensitiveWord>.ByIdCacheKey.Create(sensitiveWord.Id.ToString());
            // 将新增的纪录放到缓存中
            await _bus.RaiseEvent(new SetCacheEvent(sensitiveWord, key, key.CacheTime), cancellationToken);
           await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<SensitiveWord>.ByTenantKey.Create()),
                                  cancellationToken);
        }

        return true;
    }
}