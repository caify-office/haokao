using HaoKao.CourseRatingService.Application.ViewModels;
using HaoKao.CourseRatingService.Domain.Commands;
using HaoKao.CourseRatingService.Domain.Entities;
using HaoKao.CourseRatingService.Domain.Enums;
using HaoKao.CourseRatingService.Domain.Queries;
using HaoKao.CourseRatingService.Domain.Repositories;

namespace HaoKao.CourseRatingService.Application.Services.Management;

/// <summary>
/// 课程评价接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程评价",
    "23887aa5-6650-4b59-9506-c453690eea61",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseRatingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IMapper mapper,
    ICourseRatingRepository repository
) : ICourseRatingService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseCourseRatingViewModel> Get(Guid id)
    {
        var entity = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseRating>.ByIdCacheKey.Create(id.ToString()),
            () => repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程评价不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseCourseRatingViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryCourseRatingViewModel> Get([FromQuery] QueryCourseRatingViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<CourseRatingQuery>();
        query.QueryFields = typeof(QueryCourseRatingListViewModel).GetTypeQueryFields();
        query.OrderBy = nameof(CourseRating.CreateTime);
        var tempQuery = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseRating>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await repository.GetByQueryAsync(query);
                return query;
            }
        );

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryCourseRatingViewModel, CourseRating>();
    }

    /// <summary>
    /// 创建课程评价
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateCourseRatingViewModel model)
    {
        var command = mapper.Map<CreateCourseRatingCommand>(model);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程评价
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseRatingCommand(id);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 审核课程评价
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="state">审核状态</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("审核", Permission.Audit, _UserTypeOfEdit)]
    public async Task Audit(Guid id, [FromQuery] AuditState state)
    {
        var command = new AuditCourseRatingCommand(id, state);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 置顶课程评价
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="sticky">是否置顶</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("置顶", Permission.Edit, _UserTypeOfEdit)]
    public async Task Sticky(Guid id, [FromQuery] bool sticky)
    {
        var command = new StickyCourseRatingCommand(id, sticky);

        await bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }
}