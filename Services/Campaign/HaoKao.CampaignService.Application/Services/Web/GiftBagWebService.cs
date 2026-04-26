using HaoKao.CampaignService.Application.ViewModels.GiftBag;
using HaoKao.CampaignService.Domain.Commands;
using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Enums;
using HaoKao.CampaignService.Domain.Queries;
using HaoKao.CampaignService.Domain.ReceiveRules;
using HaoKao.CampaignService.Domain.Repositories;
using HaoKao.Common.Events.Coupon;
using HaoKao.Common.Events.StudentPermission;

namespace HaoKao.CampaignService.Application.Services.Web;

/// <summary>
/// 活动管理-PC端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class GiftBagWebService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    IEventBus eventBus,
    INotificationHandler<DomainNotification> notifications,
    IGiftBagRepository repository,
    IEnumerable<IReceiveRule> rules
) : IGiftBagWebService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IGiftBagRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IEnumerable<IReceiveRule> _rules = rules ?? throw new ArgumentNullException(nameof(rules));
    private ReceivePlatform _platform = ReceivePlatform.WebSite;

    /// <summary>
    /// 按Id获取礼包
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}"), AllowAnonymous]
    public async Task<GetGiftBagImageSetViewModel> Get(Guid id)
    {
        var entity = await GetFromCache(id);
        return new GetGiftBagImageSetViewModel(entity.Id, entity.WebSiteImageSet, true);
    }

    /// <summary>
    /// 获取用户可领取的礼包列表
    /// </summary>
    /// <param name="registrationTime"></param>
    /// <returns>礼包配置的图片</returns>
    [HttpGet, AllowAnonymous]
    public Task<IReadOnlyList<GetGiftBagImageSetViewModel>> Get([FromQuery] DateTime registrationTime)
    {
        return GetGiftBagImageSetViewModel(ReceivePlatform.WebSite, registrationTime);
    }

    /// <summary>
    /// 领取礼包
    /// </summary>
    /// <param name="id">礼包Id</param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{id:guid}")]
    public async Task Receive(Guid id, [FromBody] ReceiveGiftBagViewModel model)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var command = new ReceiveGiftBagCommand(id, userId, model.UserName, model.RegistrationTime);
        var entity = await TrySendCommand(command);

        await PublishEventBasedOnGiftType(entity, model.UserName, userId);
    }

    private async Task<T> TrySendCommand<T>(IRequest<T> command)
    {
        var result = await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return (T)result;
    }

    private async Task PublishEventBasedOnGiftType(GiftBag entity, string userName, Guid userId)
    {
        if (entity.GiftType == GiftType.Coupon)
        {
            var couponEvent = new CreateUserCouponEvent(entity.ProductId, userName, (int)_platform);
            await _eventBus.PublishAsync(couponEvent);
        }
        else
        {
            var productEvent = CreateStudentPermissionEvent(userName, userId, entity);
            await _eventBus.PublishAsync(productEvent);
        }
    }

    private static CreateStudentPermissionEvent CreateStudentPermissionEvent(string userName, Guid userId, GiftBag entity)
    {
        return new CreateStudentPermissionEvent
        (
            userName,
            userId,
            "",
            entity.ProductId,
            entity.ProductName,
            JsonSerializer.Serialize(new List<object> { new { ContentId = entity.ProductId } }),
            6
        );
    }

    /// <summary>
    /// 获取礼包图片
    /// </summary>
    /// <param name="receivePlatform"></param>
    /// <param name="registrationTime"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<IReadOnlyList<GetGiftBagImageSetViewModel>> GetGiftBagImageSetViewModel
        (ReceivePlatform receivePlatform, DateTime registrationTime = default)
    {
        var isAuthenticated = EngineContext.Current.IsAuthenticated;
        Guid? userId = isAuthenticated ? EngineContext.Current.ClaimManager.GetUserId().To<Guid>() : null;
        var cacheKey = $"{nameof(GiftBagPublishedByUserQuery)}:{userId?.ToString() ?? "all"}";
        var query = new GiftBagPublishedByUserQuery(userId);

        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GiftBag>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetPublishedGiftBagsByUser(query)
        );

        return list.Select(item => new GetGiftBagImageSetViewModel(
                               item.Id,
                               receivePlatform == ReceivePlatform.WebSite ? item.WebSiteImageSet : item.WeChatMiniProgramImageSet,
                               CanReceiveGiftBag(item, isAuthenticated, userId, registrationTime)
                           )).ToList();
    }

    private bool CanReceiveGiftBag(GiftBag item, bool isAuthenticated, Guid? userId, DateTime registrationTime)
    {
        if (!isAuthenticated) return true; // 用户未登录，假定可领取

        var receiveRuleParam = new ReceiveRuleParam(item, userId.Value, registrationTime);

        // 验证内置的领取规则
        if (_rules.Where(r => r.Internal).Any(rule => rule.IsBroken(receiveRuleParam)))
        {
            return false;
        }

        // 验证自定义的领取规则
        return item.ReceiveRules.All(ruleId =>
        {
            var rule = _rules.First(r => r.RuleId == ruleId);
            return !rule.IsBroken(receiveRuleParam);
        });
    }

    /// <summary>
    /// 从缓存中获取礼包列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [NonAction]
    public Task<GiftBag> GetFromCache(Guid id)
    {
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GiftBag>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的礼品包不存在", StatusCodes.Status404NotFound);
    }

    [NonAction]
    public void SetPlatform(ReceivePlatform platform)
    {
        _platform = platform;
    }
}