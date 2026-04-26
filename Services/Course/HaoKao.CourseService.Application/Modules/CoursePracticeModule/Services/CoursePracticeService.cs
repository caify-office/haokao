using HaoKao.CourseService.Application.Modules.CoursePracticeModule.Interfaces;
using HaoKao.CourseService.Application.Modules.CoursePracticeModule.ViewModels;
using HaoKao.CourseService.Domain.CoursePracticeModule;

namespace HaoKao.CourseService.Application.Modules.CoursePracticeModule.Services;

/// <summary>
/// 课后练习接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课后练习",
    "17d9b11c-8649-05eb-1cda-92ff9634c9ac",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CoursePracticeService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICoursePracticeRepository repository,
    IMapper mapper) : ICoursePracticeService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICoursePracticeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据课程章节id获取指定
    /// </summary>
    /// <param name="courseChapterId">课程章节id</param>
    [HttpGet("{courseChapterId}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCoursePracticeViewModel> Get(Guid courseChapterId)
    {
        var coursePractice = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CoursePractice>.ByIdCacheKey.Create(courseChapterId.ToString()),
            () => _repository.GetAsync(x => x.CourseChapterId == courseChapterId)
        );
        if (coursePractice == null) return null;

        return coursePractice.MapToDto<BrowseCoursePracticeViewModel>();
    }

    /// <summary>
    /// 根据课程id和章节id获取指定章节练习(智辅课程专用)
    /// </summary>
    /// <param name="courseId">课程id</param>
    /// <param name="courseChapterId">Id</param>
    [HttpGet("{courseId}/{courseChapterId}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseCoursePracticeViewModel> GetChapterPractice(Guid courseId, Guid courseChapterId)
    {
        var coursePractice = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CoursePractice>.QueryCacheKey.Create($"{courseId}-{courseChapterId}"),
            () => _repository.GetAsync(x => x.CourseId == courseId && x.CourseChapterId == courseChapterId)
        );

        if (coursePractice == null) return null;

        return coursePractice.MapToDto<BrowseCoursePracticeViewModel>();
    }

    /// <summary>
    /// 根据主键更新指定课后练习
    /// </summary>
    /// <param name="model">新增模型</param>
    private async Task Update([FromBody] UpdateCoursePracticeViewModel model)
    {
        var command = model.MapToCommand<UpdateCoursePracticeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 创建课后练习
    /// </summary>
    /// <param name="model">新增模型</param>
    private async Task Create([FromBody] CreateCoursePracticeViewModel model)
    {
        var command = model.MapToCommand<CreateCoursePracticeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 保存智辅课程练习（智辅课程专用）
    /// </summary>
    /// <param name="model">新增模型</param>
    public async Task SaveAssistantCoursePractice([FromBody] SaveAssistantCoursePracticeViewModel model)
    {
        var command = model.MapToCommand<SaveAssistantCoursePracticeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryCoursePracticeViewModel> Get([FromQuery] QueryCoursePracticeViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CoursePracticeQuery>();
        var cacheKey = GirvsEntityCacheDefaults<CoursePractice>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryCoursePracticeViewModel, CoursePractice>();
    }

    /// <summary>
    /// 保存课后练习
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Save([FromBody] SaveCoursePracticeViewModel model)
    {
        if (model.Id.HasValue)
        {
            return Update(_mapper.Map<UpdateCoursePracticeViewModel>(model));
        }
        return Create(_mapper.Map<CreateCoursePracticeViewModel>(model));
    }

    /// <summary>
    /// 根据主键删除指定课后练习
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("清除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCoursePracticeCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}