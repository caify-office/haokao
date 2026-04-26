using HaoKao.AgreementService.Application.ViewModels.CourseAgreement;
using HaoKao.AgreementService.Domain.Commands;
using HaoKao.AgreementService.Domain.Entities;
using HaoKao.AgreementService.Domain.Queries;
using HaoKao.AgreementService.Domain.Repositories;

namespace HaoKao.AgreementService.Application.Services.Management;

/// <summary>
/// 课程协议接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "课程协议管理",
    "adff8a34-ef07-4a96-8e58-40e63db37c7f",
    "2048",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class CourseAgreementService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ICourseAgreementRepository repository,
    IMapper mapper
) : ICourseAgreementService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ICourseAgreementRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
    public async Task<BrowseCourseAgreementViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseAgreement>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程协议不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseCourseAgreementViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryCourseAgreementViewModel> Get([FromQuery] QueryCourseAgreementViewModel viewModel)
    {
        var query = viewModel.MapToQuery<CourseAgreementQuery>();
        query.OrderBy = nameof(CourseAgreement.CreateTime);
        var key = JsonSerializer.Serialize(query).ToMd5();
        var cacheKey = GirvsEntityCacheDefaults<CourseAgreement>.QueryCacheKey.Create(key);
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

        return query.MapToQueryDto<QueryCourseAgreementViewModel, CourseAgreement>();
    }

    /// <summary>
    /// 根据ids获取列表
    /// </summary>
    /// <param name="ids">查询对象</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<List<BrowseCourseAgreementViewModel>> GetByIds([FromBody] IReadOnlyList<Guid> ids)
    {
        var key = string.Join(',', ids).ToMd5();
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<CourseAgreement>.ByIdsCacheKey.Create(key),
            () => _repository.GetWhereAsync(x => ids.Contains(x.Id))
        );
        return _mapper.Map<List<BrowseCourseAgreementViewModel>>(list);
    }

    /// <summary>
    /// 创建课程协议
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateCourseAgreementViewModel model)
    {
        var command = _mapper.Map<CreateCourseAgreementCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定课程协议
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteCourseAgreementCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定课程协议
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update(Guid id, [FromBody] UpdateCourseAgreementViewModel model)
    {
        model.Id = id;
        var command = _mapper.Map<UpdateCourseAgreementCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}