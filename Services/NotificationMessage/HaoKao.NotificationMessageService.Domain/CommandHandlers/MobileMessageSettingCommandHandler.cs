using HaoKao.NotificationMessageService.Domain.Commands.MobileMessageSetting;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.CommandHandlers;

public class MobileMessageSettingCommandHandler(
    IUnitOfWork<MobileMessageSetting> uow,
    IMediatorHandler bus,
    IRepository<MobileMessageSetting> repository
) : CommandHandler(uow, bus),
    IRequestHandler<SaveMobileMessageSettingCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<MobileMessageSetting> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(SaveMobileMessageSettingCommand request, CancellationToken cancellationToken)
    {
        var ms = await _repository.GetAsync(x => true);
        if (ms == null)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(request.AppId), "未找到对应的短信设置信息"),
                                  cancellationToken);
            return false;
        }

        ms.MobileMessagePlatform = request.MobileMessagePlatform;
        ms.AppId = request.AppId;
        ms.AppSecret = request.AppSecret;
        ms.DefaultSign = request.DefaultSign;
        ms.SignList = request.SignList;
        ms.Templates = request.Templates;

        if (!await Commit()) return false;

        var idKey = Guid.Empty.ToString();

        var key = GirvsEntityCacheDefaults<MobileMessageSetting>.ByIdCacheKey.Create(idKey);
        await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        return true;
    }
}