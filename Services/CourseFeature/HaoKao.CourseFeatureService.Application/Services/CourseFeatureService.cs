using HaoKao.CourseFeatureService.Application.Interfaces;
using HaoKao.CourseFeatureService.Application.ViewModels;
using HaoKao.CourseFeatureService.Domain;

namespace HaoKao.CourseFeatureService.Application.Services;

/// <summary>
/// 课程特色服务接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "特色服务管理",
    "f37b4f50-ce51-4135-bdac-eda499361a9d",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseFeatureService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseFeatureRepository repository,
    IMapper mapper
) : ICourseFeatureService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseFeatureRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseCourseFeatureViewModel> Get(Guid id)
    {
        var courseFeature = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseFeature>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程特色服务不存在", StatusCodes.Status404NotFound);

        return courseFeature.MapToDto<BrowseCourseFeatureViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryCourseFeatureViewModel> Get([FromQuery] QueryCourseFeatureViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseFeatureQuery>();
        query.OrderBy = nameof(CourseFeature.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseFeature>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryCourseFeatureViewModel, CourseFeature>();
    }

    /// <summary>
    /// 创建课程特色服务
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public Task Create([FromBody] CreateCourseFeatureViewModel model)
    {
        var command = _mapper.Map<CreateCourseFeatureCommand>(model);
        return SendCommand(command);
    }

    /// <summary>
    /// 批量删除课程特色服务
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public Task Delete([FromBody] List<Guid> ids)
    {
        var command = new DeleteCourseFeatureCommand(ids);
        return SendCommand(command);
    }

    /// <summary>
    /// 根据主键更新指定课程特色服务
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public Task Update([FromBody] UpdateCourseFeatureViewModel model)
    {
        var command = _mapper.Map<UpdateCourseFeatureCommand>(model);
        return SendCommand(command);
    }

    /// <summary>
    /// 批量启用/禁用课程特色服务
    /// </summary>
    /// <param name="model">启用/禁用模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public Task Enable([FromBody] EnableCourseFeatureViewModel model)
    {
        var command = _mapper.Map<EnableCourseFeatureCommand>(model);
        return SendCommand(command);
    }

    #endregion

    private async Task<T> SendCommand<T>(IRequest<T> command)
    {
        var result = await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return (T)result;
    }
}