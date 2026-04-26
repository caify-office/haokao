using HaoKao.NotificationMessageService.Domain.Commands.NotificationMessage;
using HaoKao.NotificationMessageService.Domain.Enumerations;
using HaoKao.NotificationMessageService.Domain.Models;

namespace HaoKao.NotificationMessageService.Domain.CommandHandlers;

public class NotificationMessageCommandHandler(
    IUnitOfWork<NotificationMessage> uow,
    IMediatorHandler bus,
    IRepository<NotificationMessage> repository
) : CommandHandler(uow, bus),
    IRequestHandler<SendNotificationMessageCommand, bool>,
    IRequestHandler<ReadAllSiteNotificationMessageCommand, bool>,
    IRequestHandler<ReadSiteNotificationMessageCommand, bool>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IRepository<NotificationMessage> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<bool> Handle(SendNotificationMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new NotificationMessage
        {
            // 截取100字符
            Title = request.Title.Length > 100 ? request.Title[..100] : request.Title,
            ParameterContent = request.ParameterContent,
            ReceivingChannel = request.ReceivingChannel,
            SendState = request.SendState,
            NotificationMessageType = request.NotificationMessageType,
            MessageTemplateId = request.MessageTemplateId,
            Failure = request.Failure,
            Receiver = request.Receiver,
            IsRead = request.IsRead,
            IdCard = request.IdCard,
            TenantId = EngineContext.Current.ClaimManager.GetTenantId().ToHasGuid() ?? Guid.Empty
        };

        await _repository.AddAsync(message);
        if (await Commit())
        {
            // var engityName = typeof(NotificationMessage).Name.ToLowerInvariant();
            // //清除考生端，通过身份证查询建立的缓存
            // var keyStr = string.Format("{0}:{1}", engityName, message.IdCard.ToMd5());
            // await _bus.RaiseEvent(new RemoveCacheListEvent(new CacheKey(keyStr).Create()), cancellationToken);
            // //清除管理端列表查询建立的缓存
            // await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<NotificationMessage>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ReadSiteNotificationMessageCommand request, CancellationToken cancellationToken)
    {
        var receiver = await _repository.GetByIdAsync(request.Id);
        if (receiver == null)
        {
            await _bus.RaiseEvent(new DomainNotification(request.Id.ToString(), "当前消息不属于你"), cancellationToken);
            return false;
        }
        receiver.IsRead = true;

        if (await Commit())
        {
            //清除考生端，通过身份证查询建立的缓存
            // var engityName = typeof(NotificationMessage).Name.ToLowerInvariant();
            // var keyStr = string.Format("{0}:{1}", engityName, receiver.IdCard.ToMd5());
            // await _bus.RaiseEvent(new RemoveCacheListEvent(new CacheKey(keyStr).Create()), cancellationToken);
            // //清除管理端列表查询建立的缓存
            // await _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<NotificationMessage>.TenantListCacheKey.Create()), cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(ReadAllSiteNotificationMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetWhereAsync(x => x.IdCard == request.IdCard && x.ReceivingChannel.HasFlag(ReceivingChannel.InSite));
        if (message.Any())
        {
            message.ForEach(receiver => { receiver.IsRead = true; });

            if (await Commit())
            {
                foreach (var item in message)
                {
                    var key = GirvsEntityCacheDefaults<NotificationMessage>.ByIdCacheKey.Create(item.Id.ToString());
                    await _bus.RaiseEvent(new SetCacheEvent(item, key, key.CacheTime), cancellationToken);
                }
                // _bus.RaiseEvent(new RemoveCacheListEvent(GirvsEntityCacheDefaults<NotificationMessage>.ListCacheKey.Create()), cancellationToken);
            }
        }

        return true;
    }
}