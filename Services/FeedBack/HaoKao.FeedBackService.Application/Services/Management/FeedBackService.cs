using HaoKao.FeedBackService.Application.ViewModels.FeedBack;
using HaoKao.FeedBackService.Domain.Commands.FeedBack;
using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Queries.EntityQuery;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Application.Services.Management;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "反馈建议",
    "e121b171-f88a-42d2-aed3-b8ee66feaeac",
    "64",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class FeedBackService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IFeedBackRepository repository
) : IFeedBackService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IFeedBackRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id:guid}")]
    public async Task<BrowseFeedBackViewModel> Get(Guid Id)
    {
        var feedBack = await _repository.GetByIdAsync(Id)
        ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);
        return feedBack.MapToDto<BrowseFeedBackViewModel>();
    }

    /// <summary>
    /// 读取当前用户的提交次数
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<int> GetUserCount()
    {
        var userid = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return await _repository.GetUserCount(userid);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<FeedBackQueryViewModel> Get([FromQuery] FeedBackQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<FeedBackQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<FeedBack>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<FeedBackQueryViewModel, FeedBack>();
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateFeedBackViewModel model)
    {
        var command = new CreateFeedBackCommand(
            model.Type,
            model.SourceType,
            model.Status,
            model.Contract,
            model.Description,
            model.FileUrls, model.ParentId
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        var command = new DeleteFeedBackCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id}")]
    public async Task Update(Guid id, [FromBody] UpdateFeedBackViewModel model)
    {
        var command = new UpdateFeedBackCommand(id, model.Status);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}