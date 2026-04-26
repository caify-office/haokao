using HaoKao.NotificationMessageService.Domain.Commands.WechatMessageSetting;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.CommandHandlers;

public class WechatMessageSettingCommandHandler(
    IUnitOfWork<WechatMessageSetting> uow,
    IMediatorHandler bus,
    IRepository<WechatMessageSetting> repository
) : CommandHandler(uow, bus), IRequestHandler<SaveWechatMessageSettingCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public async Task<bool> Handle(SaveWechatMessageSettingCommand request, CancellationToken cancellationToken)
    {
        var ms = await repository.GetAsync(x => true);
        if (ms == null)
        {
            await _bus.RaiseEvent(new DomainNotification(nameof(request.AppId), "未找到对应的短信设置信息"),
                                  cancellationToken);
            return false;
        }

        ms.AppId = request.AppId;
        ms.AppSecret = request.AppSecret;
        ms.Templates = request.Templates;

        if (!await Commit()) return false;

        var idKey = Guid.Empty.ToString();

        var key = GirvsEntityCacheDefaults<WechatMessageSetting>.ByIdCacheKey.Create(idKey);
        await _bus.RaiseEvent(new RemoveCacheEvent(key), cancellationToken);
        return true;
    }
}