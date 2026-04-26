using HaoKao.NotificationMessageService.Domain.Commands.InSiteMessageSetting;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.CommandHandlers;

public class InSiteMessageSettingCommandHandler(
    IUnitOfWork<InSiteMessageSetting> uow,
    IMediatorHandler bus,
    IRepository<InSiteMessageSetting> repository
) : CommandHandler(uow, bus),
    IRequestHandler<SaveInSiteMessageSettingCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<InSiteMessageSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(SaveInSiteMessageSettingCommand request, CancellationToken cancellationToken)
    {
        var ms = await _repository.GetAsync(x => true);
        if (ms == null)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(SaveInSiteMessageSettingCommand), "未找到对应的站内设置信息"),
                                  cancellationToken);
            return false;
        }

        ms.Templates = request.Templates;

        if (!await Commit()) return false;

        var idKey = Guid.Empty.ToString();

        var key = GirvsEntityCacheDefaults<InSiteMessageSetting>.ByIdCacheKey.Create(idKey);
        await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        return true;
    }
}