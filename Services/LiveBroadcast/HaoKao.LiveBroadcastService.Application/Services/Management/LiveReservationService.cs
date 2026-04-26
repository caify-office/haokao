using Girvs.AuthorizePermission;
using Girvs.Driven.Extensions;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveReservation;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播预约服务接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class LiveReservationService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveReservationRepository repository
) : ILiveReservationService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveReservationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseLiveReservationViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveMessage>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的直播消息不存在", StatusCodes.Status404NotFound);
        return entity.MapToDto<BrowseLiveReservationViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryLiveReservationViewModel> Get([FromQuery] QueryLiveReservationViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveReservationQuery>();
        query.OrderBy = nameof(LiveReservation.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveReservation>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryLiveReservationViewModel, LiveReservation>();
    }

    /// <summary>
    /// 创建直播预约
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task<bool> Create(CreateLiveReservationViewModel model)
    {
        var command = model.MapToCommand<CreateLiveReservationCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        return true;
    }

    #endregion
}