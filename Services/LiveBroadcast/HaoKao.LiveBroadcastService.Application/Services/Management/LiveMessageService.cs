using Girvs.AuthorizePermission;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Hubs;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播消息服务接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class LiveMessageService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveMessageRepository repository,
    ILiveAdministratorRepository liveAdministratorRepository,
    IHubContext<LiveChatHub, ILiveChatHub> hubContext
) : ILiveMessageService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [NonAction]
    public async Task<BrowseLiveMessageViewModel> Get(Guid id)
    {
        var entity = await GetEntity(id);
        var result = entity.MapToDto<BrowseLiveMessageViewModel>();
        result.IsLiveAdmin = await liveAdministratorRepository.ExistEntityAsync(x => x.UserId == result.CreatorId);
        return result;
    }

    private async Task<LiveMessage> GetEntity(Guid id)
    {
        var entity = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveMessage>.ByIdCacheKey.Create(id.ToString()),
            () => repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的直播消息不存在", StatusCodes.Status404NotFound);

        return entity;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [NonAction]
    public async Task<QueryLiveMessageViewModel> Get(QueryLiveMessageViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveMessageQuery>();
        query.OrderBy = nameof(LiveMessage.CreateTime);

        // 实时消息不走缓存
        await repository.GetByQueryAsync(query);
        var vm = query.MapToQueryDto<QueryLiveMessageViewModel, LiveMessage>();

        if (vm.RecordCount > 0)
        {
            // 是否直播管理员
            var admins = await liveAdministratorRepository.GetAllAsync();
            foreach (var r in vm.Result)
            {
                r.IsLiveAdmin = admins.Any(x => x.UserId == r.CreatorId);
            }
        }

        return vm;
    }

    /// <summary>
    /// 获取直播间置顶的消息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<PinTopMessageOutput> GetPinedMessage(PinTopMessageRequest request)
    {
        var cacheKey = GirvsEntityCacheDefaults<LiveMessage>.ByIdCacheKey.Create(request.LiveId.ToString());

        //if (!cacheManager.IsSet(cacheKey)) return new PinTopMessageOutput();

        var message = await cacheManager.GetAsync(cacheKey, () => Task.FromResult(new PinTopMessageOutput()));
        if (message == default) return message;

        if (message.LiveId != Guid.Empty)
        {
            // 存入缓存, 缓存时间为置顶时长
            // 倒计时方案: 置顶倒计时 Countdown, 置顶时间 PinTopTime, 系统当前时间 SystemTime
            // 计算置顶持续时间, Duration = Countdown - SystemTime - PinTopTime
            message.SystemTime = DateTime.Now;
            message.Duration = (int)(message.Countdown - (message.SystemTime - message.PinTopTime).TotalSeconds);
            return message;
        }
        return message;
    }

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    [HttpGet("{liveId:guid}")]
    public Task ProductOnSell(Guid liveId, Guid[] ids)
    {
        return hubContext.Clients.Group(liveId.ToString()).ProductOnSell(ids);
    }

    /// <summary>
    /// 产品上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    [HttpGet("{liveId:guid}")]
    public Task ProductDownSell(Guid liveId, Guid[] ids)
    {
        return hubContext.Clients.Group(liveId.ToString()).ProductDownSell(ids);
    }

    /// <summary>
    /// 优惠券上架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    [HttpGet("{liveId:guid}")]
    public Task CouponPickUp(Guid liveId, Guid[] ids)
    {
        if (ids is null)
        {
            throw new ArgumentNullException(nameof(ids));
        }

        return hubContext.Clients.Group(liveId.ToString()).CouponPickUp(ids);
    }

    /// <summary>
    /// 优惠券下架通知
    /// </summary>
    /// <param name="liveId"></param>
    /// <param name="ids"></param>
    [HttpGet("{liveId:guid}")]
    public Task CouponPickDown(Guid liveId, Guid[] ids)
    {
        return hubContext.Clients.Group(liveId.ToString()).CouponPickDown(ids);
    }

    /// <summary>
    /// 创建直播消息
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateLiveMessageViewModel model)
    {
        var command = model.MapToCommand<CreateLiveMessageCommand>();

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除直播消息
    /// </summary>
    /// <param name="id"></param>
    [NonAction]
    public async Task Delete(Guid id)
    {
        var msg = await GetEntity(id);

        var command = new DeleteLiveMessageCommand(id);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        await hubContext.Clients.Group(msg.LiveId.ToString()).RevokeMessage(id);
    }
}