using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveCoupon;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveCoupon;
using HaoKao.LiveBroadcastService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播优惠卷接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "直播优惠卷",
    "ca795da9-bf22-02f8-4b5f-f107c497bc8c",
    "256",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LiveCouponService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveCouponRepository repository,
    ILiveMessageService liveMessageService
) : ILiveCouponService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveCouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILiveMessageService _liveMessageService = liveMessageService ?? throw new ArgumentNullException(nameof(liveMessageService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseLiveCouponViewModel> Get(Guid id)
    {
        var LiveCoupon = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveCoupon>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的直播优惠卷不存在", StatusCodes.Status404NotFound);

        return LiveCoupon.MapToDto<BrowseLiveCouponViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<LiveCouponQueryViewModel> Get([FromQuery] LiveCouponQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<LiveCouponQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveCoupon>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<LiveCouponQueryViewModel, LiveCoupon>();
    }

    /// <summary>
    /// 创建直播优惠卷
    /// </summary>
    /// <param name="models">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] List<CreateLiveCouponViewModel> models)
    {
        var createModel = models.MapTo<List<CreateLiveCouponModel>>();
        var command = new CreateLiveCouponCommand(createModel);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定直播优惠卷
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLiveCouponCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定直播优惠卷
    /// </summary>
    /// <param name="models">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] List<UpdateLiveCouponViewModel> models)
    {
        var updateModels = models.MapTo<List<UpdateLiveCouponModel>>();

        var command = new UpdateLiveCouponCommand(updateModels);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("上架", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesUp(Guid[] id)
    {
        var command = new SetLiveCouponShelvesCommand(id, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var liveConpon = await Get(id.FirstOrDefault());
        await _liveMessageService.CouponPickUp(liveConpon.LiveVideoId, id);
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("下架", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ShelvesDown([FromBody] Guid[] id)
    {
        var command = new SetLiveCouponShelvesCommand(id, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var liveConpon = await Get(id.FirstOrDefault());
        await _liveMessageService.CouponPickDown(liveConpon.LiveVideoId, id);
    }

    #endregion
}