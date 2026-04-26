using Girvs.AuthorizePermission.Enumerations;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Services.Management;

/// <summary>
///社区抽奖服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "抽奖接口管理",
    "362b1944-0029-f6e2-0bda-8606091257a7",
    "512",
    SystemModule.ExtendModule2,
    1
)]
public class DrawPrizeService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IDrawPrizeRepository repository
) : IDrawPrizeService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IDrawPrizeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseDrawPrizeViewModel> Get(Guid id)
    {
        var drawPrize = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<DrawPrize>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的抽奖不存在", StatusCodes.Status404NotFound);

        return drawPrize.MapToDto<BrowseDrawPrizeViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<DrawPrizeQueryViewModel> Get([FromQuery] DrawPrizeQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<DrawPrizeQuery>();
        var cacheKey = GirvsEntityCacheDefaults<DrawPrize>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<DrawPrizeQueryViewModel, DrawPrize>();
    }

    /// <summary>
    /// 创建抽奖
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateDrawPrizeViewModel model)
    {
        var command = model.MapToCommand<CreateDrawPrizeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定抽奖
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete([FromBody] Guid[] ids)
    {
        var command = new DeleteDrawPrizeCommand(ids);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定抽奖
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateDrawPrizeViewModel model)
    {
        var command = model.MapToCommand<UpdateDrawPrizeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置抽奖规则
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("设置抽奖规则", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task SetDrawPrizeRule([FromBody] SetDrawPrizeRuleViewModel model)
    {
        var command = model.MapToCommand<SetDrawPrizeRuleCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("启用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Enable([FromBody] Guid[] modelIds)
    {
        var command = new SetDrawPrizeEnableCommand(modelIds, true);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="modelIds"></param>
    /// <returns></returns>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Disable([FromBody] Guid[] modelIds)
    {
        var command = new SetDrawPrizeEnableCommand(modelIds, false);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion


    [HttpGet]
    [ServiceMethodPermissionDescriptor("设置奖品", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void SetPrize() { }
}