using HaoKao.StudyMaterialService.Application.Interfaces;
using HaoKao.StudyMaterialService.Application.ViewModels;
using HaoKao.StudyMaterialService.Domain.Commands;
using HaoKao.StudyMaterialService.Domain.Entities;
using HaoKao.StudyMaterialService.Domain.Queries;
using HaoKao.StudyMaterialService.Domain.Repositories;

namespace HaoKao.StudyMaterialService.Application.Services;

/// <summary>
/// 学习资料接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学习资料",
    "049ec733-f67e-44fa-86a6-7e59ae589a92",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudyMaterialService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudyMaterialRepository repository,
    IMapper mapper
) : IStudyMaterialService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudyMaterialRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
    public async Task<BrowseStudyMaterialViewModel> Get(Guid id)
    {
        var studyMaterial = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudyMaterial>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的学习资料不存在", StatusCodes.Status404NotFound);

        return studyMaterial.MapToDto<BrowseStudyMaterialViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryStudyMaterialViewModel> Get([FromQuery] QueryStudyMaterialViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<StudyMaterialQuery>();
        query.OrderBy = nameof(StudyMaterial.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<StudyMaterial>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryStudyMaterialViewModel, StudyMaterial>();
    }

    /// <summary>
    /// 创建学习资料
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateStudyMaterialViewModel model)
    {
        var command = _mapper.Map<CreateStudyMaterialCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量删除指定学习资料
    /// </summary>
    /// <param name="ids">ids</param>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete([FromBody] List<Guid> ids)
    {
        var command = new DeleteStudyMaterialCommand(ids);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定学习资料
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateStudyMaterialViewModel model)
    {
        var command = _mapper.Map<UpdateStudyMaterialCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量启用/禁用学习资料
    /// </summary>
    /// <param name="model">启用/禁用模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public async Task Enable([FromBody] EnableStudyMaterialViewModel model)
    {
        var command = _mapper.Map<EnableStudyMaterialCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}