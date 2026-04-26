using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseMaterialsModule.ViewModels;
using HaoKao.CourseService.Domain.CourseMaterialsModule;

namespace HaoKao.CourseService.Application.Modules.CourseMaterialsModule.Services;

/// <summary>
/// 课程讲义接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程讲义",
    "591e3551-93dd-4ac0-9300-f416faf1c5e8",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseMaterialsService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseMaterialsRepository repository
) : ICourseMaterialsService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseMaterialsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCourseMaterialsViewModel> Get(Guid id)
    {
        var courseMaterials = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseMaterials>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程讲义不存在", StatusCodes.Status404NotFound);

        return courseMaterials.MapToDto<BrowseCourseMaterialsViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryCourseMaterialsViewModel> Get([FromQuery] QueryCourseMaterialsViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseMaterialsQuery>();
        var cacheKey = GirvsEntityCacheDefaults<CourseMaterials>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryCourseMaterialsViewModel, CourseMaterials>();
    }

    /// <summary>
    /// 创建课程讲义
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateCourseMaterialsViewModel model)
    {
        var command = model.MapToCommand<CreateCourseMaterialsCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 保存课程讲义（智辅学习专用）
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Save([FromBody] SaveCourseMaterialsViewModel model)
    {
        var command = model.MapToCommand<SaveCourseMaterialsCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程讲义
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseMaterialsCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程讲义
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] SetCourseMaterialsSortViewModel model)
    {
        var command = new SetCourseMaterialsSortCommand(id, model.Sort);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}