using HaoKao.NotificationMessageService.Domain.Commands.TenantSignSetting;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.CommandHandlers;

public class TenantSignSettingCommandHandler(
    IUnitOfWork<TenantSignSetting> uow,
    IMediatorHandler bus,
    IRepository<TenantSignSetting> repository,
    IStaticCacheManager staticCacheManager
) : CommandHandler(uow, bus),
    IRequestHandler<SaveTenantSignSettingCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<TenantSignSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public async Task<bool> Handle(SaveTenantSignSettingCommand request, CancellationToken cancellationToken)
    {
        var set = await _repository.GetAsync(x => true);
        if (set == null)
        {
            set = new TenantSignSetting
            {
                Sign = request.Sign
            };
            await _repository.AddAsync(set);
        }
        else
        {
            set.Sign = request.Sign;
        }

        if (!await Commit()) return false;

        var key = GirvsEntityCacheDefaults<TenantSignSetting>.ByIdCacheKey.Create(set.TenantId.ToString());
        await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        return true;
    }
}