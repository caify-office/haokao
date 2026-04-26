using HaoKao.CourseService.Application.Modules.CourseChapterModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CourseChapterModule.ViewModels;
using HaoKao.CourseService.Domain.CourseChapterModule;

namespace HaoKao.CourseService.Application.Modules.CourseChapterModule.Services;

/// <summary>
/// 课程章节接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程章节",
    "703bb06b-5b9c-4b01-af6f-0d95bf6a52c6",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseChapterService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseChapterRepository repository
) : ICourseChapterService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseChapterRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCourseChapterViewModel> Get(Guid id)
    {
        var courseChapter = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseChapter>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程章节不存在", StatusCodes.Status404NotFound);

        return courseChapter.MapToDto<BrowseCourseChapterViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryCourseChapterViewModel> Get([FromQuery] QueryCourseChapterViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseChapterQuery>();
        var cacheKey = GirvsEntityCacheDefaults<CourseChapter>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryCourseChapterViewModel, CourseChapter>();
    }

    /// <summary>
    /// 获取数据-章节树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("tree")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<List<dynamic>> GetTreeByQueryAsync(Guid? id, Guid? courseId)
    {
        return _repository.GetChapterNodeTreeByQueryAsync(id, courseId);
    }

    /// <summary>
    /// 按照课程id获取所有课程章节
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <returns></returns>
    [HttpGet("{courseId:guid}")]
    public async Task<List<BrowseCourseChapterViewModel>> GetAllAsync(Guid courseId)
    {
        var result = await _repository.GetWhereAsync(x => x.CourseId == courseId);
        return result.OrderBy(x => x.Sort).MapTo<List<BrowseCourseChapterViewModel>>();
    }

    /// <summary>
    /// 创建课程章节
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateCourseChapterViewModel model)
    {
        var command = model.MapToCommand<CreateCourseChapterCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量创建课程章节
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task BatchCreate([FromBody] List<CreateCourseChapterViewModel> models)
    {
        var command = new CreateCourseChapterBatchCommand(models.MapTo<List<CourseChapter>>());

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程章节
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseChapterCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 清空目录
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task BatchDelete(Guid id)
    {
        var command = new BatchDeleteCourseChapterCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程章节
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateCourseChapterViewModel model)
    {
        var command = new UpdateCourseChapterCommand(
            id,
            model.Name,
            model.ParentId,
            model.CourseId,
            model.IsLeaf, model.Sort
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}