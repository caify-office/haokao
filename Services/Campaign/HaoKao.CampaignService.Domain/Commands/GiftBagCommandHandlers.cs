using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.ReceiveRules;
using HaoKao.CampaignService.Domain.Repositories;
using HaoKao.Common.Extensions;

namespace HaoKao.CampaignService.Domain.Commands;

public class GiftBagCommandHandlers(
    IUnitOfWork<GiftBag> uow,
    IMediatorHandler bus,
    IMapper mapper,
    IGiftBagRepository repository,
    IEnumerable<IReceiveRule> rules
) : CommandHandler(uow, bus),
    IRequestHandler<CreateGiftBagCommand, bool>,
    IRequestHandler<UpdateGiftBagCommand, bool>,
    IRequestHandler<DeleteGiftBagCommand, bool>,
    IRequestHandler<UpdateGiftBagPublishedCommand, bool>,
    IRequestHandler<ReceiveGiftBagCommand, GiftBag>
{
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IGiftBagRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IEnumerable<IReceiveRule> _rules = rules ?? throw new ArgumentNullException(nameof(rules));

    public async Task<bool> Handle(CreateGiftBagCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<GiftBag>(request);

        await _repository.AddAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(UpdateGiftBagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            var notification = new DomainNotification(request.Id.ToString(), "未找到对应礼品包的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return false;
        }

        entity = _mapper.Map(request, entity);
        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
        }

        return true;
    }

    public async Task<bool> Handle(DeleteGiftBagCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIds(request.Ids);

        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<bool> Handle(UpdateGiftBagPublishedCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdatePublished(request.Ids, request.IsPublished);

        await Task.WhenAll(request.Ids.Select(x => RemoveEntityCache(x, cancellationToken)));
        await RemoveListCache(cancellationToken);

        return true;
    }

    public async Task<GiftBag> Handle(ReceiveGiftBagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdWithLogs(request.GiftBagId);
        if (entity == null)
        {
            var notification = new DomainNotification(request.GiftBagId.ToString(), "未找到对应礼品包的数据", StatusCodes.Status404NotFound);
            await _bus.RaiseEvent(notification, cancellationToken);
            return new();
        }

        var ruleParam = new ReceiveRuleParam(entity, request.UserId, request.RegistrationTime);
        foreach (var rule in _rules.Where(r => r.Internal))
        {
            if (rule.IsBroken(ruleParam))
            {
                var notification = new DomainNotification(rule.RuleId.ToString(), rule.BrokenMessage, StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return entity;
            }
        }

        foreach (var ruleId in entity.ReceiveRules)
        {
            var rule = rules.First(r => r.RuleId == ruleId);
            if (rule.IsBroken(ruleParam))
            {
                var notification = new DomainNotification(rule.RuleId.ToString(), rule.BrokenMessage, StatusCodes.Status400BadRequest);
                await _bus.RaiseEvent(notification, cancellationToken);
                return entity;
            }
        }

        entity.GiftBagReceiveLogs.Add(new()
        {
            GiftType = entity.GiftType,
            CampaignName = entity.CampaignName,
            ProductId = entity.ProductId,
            ProductName = entity.ProductName,
            ReceiveTime = DateTime.Now,
            ReceiverId = request.UserId,
            ReceiverName = request.UserName,
            RegistrationTime = request.RegistrationTime
        });

        entity.ReceiveCount = entity.GiftBagReceiveLogs.Count;

        await _repository.UpdateAsync(entity);

        if (await Commit())
        {
            await UpdateEntityCache(entity, cancellationToken);
            await RemoveListCache(cancellationToken);
            await RemoveLogCache(cancellationToken);
        }

        return entity;
    }

    #region 缓存操作

    private Task UpdateEntityCache(GiftBag entity, CancellationToken cancellationToken)
    {
        return _bus.UpdateIdCacheEvent(entity, entity.Id.ToString(), cancellationToken);
    }

    private Task RemoveEntityCache(Guid id, CancellationToken cancellationToken)
    {
        return _bus.RemoveIdCacheEvent<GiftBag>(id.ToString(), cancellationToken);
    }

    private Task RemoveListCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<GiftBag>(cancellationToken);
    }

    private Task RemoveLogCache(CancellationToken cancellationToken)
    {
        return _bus.RemoveTenantListCacheEvent<GiftBagReceiveLog>(cancellationToken);
    }

    #endregion
}