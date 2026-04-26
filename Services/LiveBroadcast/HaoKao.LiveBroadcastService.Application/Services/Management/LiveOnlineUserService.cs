using Girvs.AuthorizePermission;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播在线用户数据服务接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class LiveOnlineUserService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveOnlineUserRepository repository,
    ILiveOnlineUserTrendRepository trendRepository
) : ILiveOnlineUserService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveOnlineUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILiveOnlineUserTrendRepository _trendRepository = trendRepository ?? throw new ArgumentNullException(nameof(trendRepository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryLiveOnlineUserViewModel> Get([FromQuery] QueryLiveOnlineUserViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveOnlineUserQuery>();
        query.OrderBy = nameof(LiveOnlineUser.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveOnlineUser>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryLiveOnlineUserViewModel, LiveOnlineUser>();
    }

    /// <summary>
    /// 根据直播Id获取用户
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    [NonAction]
    public Task<LiveOnlineUser> GetUserByLiveId(Guid liveId)
    {
        var userId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveOnlineUser>.QueryCacheKey.Create($"{nameof(GetUserByLiveId)}:{userId}:{liveId}"),
            async () => await _repository.GetAsync(x => x.CreatorId == userId && x.LiveId == liveId)
        );
    }

    /// <summary>
    /// 在线人数走势统计
    /// </summary>
    /// <param name="liveId">直播Id</param>
    /// <param name="interval">时间间隔(分钟)</param>
    [HttpGet("{liveId:guid}")]
    public async Task<List<LiveOnlineUserTrend>> GetOnlineTrendStat(Guid liveId, int interval)
    {
        var list = await _trendRepository.GetWhereAsync(x => x.LiveId == liveId && x.Interval == interval);
        return list.OrderBy(x => x.CreateTime).ToList();
    }

    /// <summary>
    /// 创建直播在线用户
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateLiveOnlineUserViewModel model)
    {
        var command = model.MapToCommand<CreateLiveOnlineUserCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorOnlineUser = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorOnlineUser);
        }
    }

    /// <summary>
    /// 根据主键更新指定在线用户
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Update(UpdateLiveOnlineUserViewModel model)
    {
        var command = model.MapToCommand<UpdateLiveOnlineUserCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}