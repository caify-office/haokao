using Girvs.AuthorizePermission;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveComment;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播评论接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
public class LiveCommentService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveCommentRepository repository
) : ILiveCommentService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveCommentRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据直播查询评论
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<BrowseLiveCommentViewModel> Get(Guid liveId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();
        var liveVideo = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveComment>.ByIdCacheKey.Create($"{userId}:{liveId}"),
            async () => await _repository.GetAsync(x => x.CreatorId == userId && x.LiveId == liveId)
        );

        if (liveVideo == null)
        {
            return null;
        }

        return liveVideo.MapToDto<BrowseLiveCommentViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<QueryLiveCommentViewModel> Get([FromQuery] QueryLiveCommentViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveCommentQuery>();
        query.OrderBy = nameof(LiveComment.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveComment>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryLiveCommentViewModel, LiveComment>();
    }

    /// <summary>
    /// 创建直播评论
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateLiveCommentViewModel model)
    {
        var command = model.MapToCommand<CreateLiveCommentCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 查询综合评分
    /// </summary>
    /// <param name="liveId">直播Id</param>
    /// <returns></returns>
    [HttpGet("{liveId:guid}")]
    public async Task<double> GetAverageRating(Guid liveId)
    {
        var avg = await _repository.GetAverageRating(liveId);
        return Math.Round(avg, 2);
    }

    #endregion
}