using Girvs.AuthorizePermission.Enumerations;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Services.Management;

/// <summary>
///奖品接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "奖品接口管理",
    "e1c60350-a5a4-4b8b-584e-1f4e8f077629",
    "512",
    SystemModule.ExtendModule2,
    1
)]
public class PrizeService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPrizeRepository repository
) : IPrizeService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IPrizeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowsePrizeViewModel> Get(Guid id)
    {
        var prize = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<Prize>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的奖品不存在", StatusCodes.Status404NotFound);

        return prize.MapToDto<BrowsePrizeViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<PrizeQueryViewModel> Get([FromQuery] PrizeQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<PrizeQuery>();
        var cacheKey = GirvsEntityCacheDefaults<Prize>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<PrizeQueryViewModel, Prize>();
    }

    /// <summary>
    /// 创建奖品
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreatePrizeViewModel model)
    {
        var command = model.MapToCommand<CreatePrizeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定奖品
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeletePrizeCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定奖品
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdatePrizeViewModel model)
    {
        var command = model.MapToCommand<UpdatePrizeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定奖品保底
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("指定奖品保底", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task SetPrizeGuaranteed([FromBody] SetPrizeGuaranteedPrizeViewModel model)
    {
        var command = model.MapToCommand<SetPrizeGuaranteedCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键取消指定奖品保底
    /// </summary>
    /// <param name="id"></param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("取消奖品保底", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task CancelPrizeGuaranteed(Guid id)
    {
        var command = new CancelPrizeGuaranteedCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }


    #endregion
}