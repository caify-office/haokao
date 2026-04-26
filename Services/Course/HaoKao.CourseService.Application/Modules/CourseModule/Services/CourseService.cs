using HaoKao.CourseService.Application.Modules.CourseModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseModule.ViewModels;
using HaoKao.CourseService.Domain.CourseModule;

namespace HaoKao.CourseService.Application.Modules.CourseModule.Services;

/// <summary>
/// 课程接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程管理",
    "611a4a5d-c1e1-97f1-6131-c6804b00b76f",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseRepository repository
) : ICourseService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCourseViewModel> Get(Guid id)
    {
        var course = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Course>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程不存在", StatusCodes.Status404NotFound);

        return course.MapToDto<BrowseCourseViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryCourseViewModel> Get([FromQuery] QueryCourseViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseQuery>();
        var cacheKey = GirvsEntityCacheDefaults<Course>.QueryCacheKey.Create(query.GetCacheKey());
        var temp = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });
        if (!query.Equals(temp))
        {
            query.RecordCount = temp.RecordCount;
            query.Result = temp.Result;
        }
        return query.MapToQueryDto<QueryCourseViewModel, Course>();
    }

    /// <summary>
    /// 根据课程ids集合拿到课程下面的所有的讲师ids(去重)
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<string>> GetTeacherIds([FromQuery] QueryCourseViewModel queryViewModel)
    {
        var vm = await Get(queryViewModel);
        return vm.Result?.Select(x => JsonSerializer.Deserialize<List<TeacherJsonViewModel>>(x.TeacherJson))
                 .SelectMany(x => x).Select(x => x.Id)
                 .Distinct().ToList();
    }

    /// <summary>
    /// 创建课程
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateCourseViewModel model)
    {
        var command = model.MapToCommand<CreateCourseCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改课程", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateCourseViewModel model)
    {
        var command = model.MapToCommand<UpdateCourseCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程讲义包
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改课程讲义包", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateCourseMaterialsPackageUrl([FromBody] UpdateCourseMaterialsPackageUrlViewModel model)
    {
        var command = model.MapToCommand<UpdateCourseMaterialsPackageUrlCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用(支持批量)
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("启用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task SetUpState([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateEnableState(ids, true);
    }

    /// <summary>
    ///禁用(支持批量)
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task SetDownState([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateEnableState(ids, false);
    }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    private async Task UpdateEnableState(IEnumerable<Guid> ids, bool state)
    {
        var command = new UpdateEnableStateCommand(ids, state);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("复制", Permission.Copy, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void Copy() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("添加视频", Permission.Post_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void AddVideo() { }

    /// <summary>
    /// 合并课程章节表和视频表
    /// </summary>
    /// <returns></returns>
    [HttpPatch]
    [AllowAnonymous]
    public Task MergeChaperAndVideo()
    {
        return _repository.MergeChapterAndVideoDo();
    }

    #endregion
}