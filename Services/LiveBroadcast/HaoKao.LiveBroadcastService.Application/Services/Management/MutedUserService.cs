using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Hubs;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Application.ViewModels.MutedUser;
using HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 禁言用户接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "禁言用户",
    "d719974e-395f-422b-8750-602e977c65ca",
    "256",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class MutedUserService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IMutedUserRepository repository,
    IHubContext<LiveChatHub, ILiveChatHub> hubContext,
    OnlineUserState onlineUserState,
    ILiveOnlineUserRepository onlineUserRepository
) : IMutedUserService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IMutedUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IHubContext<LiveChatHub, ILiveChatHub> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    private readonly OnlineUserState _onlineUserState = onlineUserState;
    private readonly ILiveOnlineUserRepository _onlineUserRepository = onlineUserRepository;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryMutedUserViewModel> Get([FromQuery] QueryMutedUserViewModel viewModel)
    {
        var query = viewModel.MapToQuery<MutedUserQuery>();
        var cacheKey = GirvsEntityCacheDefaults<MutedUser>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryMutedUserViewModel, MutedUser>();
    }

    /// <summary>
    /// 创建禁言用户
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateMutedUserViewModel model)
    {
        var command = model.MapToCommand<CreateMutedUserCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        // 向租户下该用户所在直播发送禁言通知
        await SendMuted(model.UserId, true);
    }

    /// <summary>
    /// 根据主键删除指定禁言用户
    /// </summary>
    /// <param name="userId">主键</param>
    [HttpDelete("{userId:guid}")]
    [ServiceMethodPermissionDescriptor("解除禁言", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid userId)
    {
        var command = new DeleteMutedUserCommand(userId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        // 向租户下该用户所在直播发送解除禁言通知
        await SendMuted(userId, false);
    }

    private async Task SendMuted(Guid userId, bool muted)
    {
        var liveIds = await _onlineUserRepository.Query.Where(x => x.CreatorId == userId).Select(x => x.LiveId).ToListAsync();
        var connectionIds = _onlineUserState.Connections.Where(x => x.Key.UserId == userId && liveIds.Contains(x.Key.LiveId)).SelectMany(x => x.Value);
        await _hubContext.Clients.Clients(connectionIds).Muted(new(muted));
    }

    /// <summary>
    /// 是否被禁言
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public Task<bool> IsMuted()
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<MutedUser>.QueryCacheKey.Create(nameof(IsMuted) + userId),
            () => _repository.ExistEntityAsync(x => x.UserId == userId)
        );
    }

    #endregion
}