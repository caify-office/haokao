using HaoKao.ContinuationService.Application.ContinuationAuditModule.Interfaces;
using HaoKao.ContinuationService.Application.ContinuationAuditModule.ViewModels;
using HaoKao.ContinuationService.Application.ContinuationSetupModule.Interfaces;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;

namespace HaoKao.ContinuationService.Application.ContinuationAuditModule.Services;

/// <summary>
/// 续读审核接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "续读审核",
    "97006461-302a-4d50-b284-b2116cb41f4a",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ContinuationAuditService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IContinuationAuditRepository repository,
    IMapper mapper,
    IContinuationSetupService continuationSetupService
) : IContinuationAuditService
{
    #region 初始参数

    private readonly IContinuationSetupService _continuationSetupService = continuationSetupService ?? throw new ArgumentNullException(nameof(continuationSetupService));
    private readonly IContinuationAuditRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;
    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseContinuationAuditViewModel> Get(Guid id)
    {
        var entity = await GetEntity(id);
        return entity.MapToDto<BrowseContinuationAuditViewModel>();
    }

    /// <summary>
    /// 根据Id获取实体
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [NonAction]
    public async Task<ContinuationAudit> GetEntity(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ContinuationAudit>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的续读审核不存在", StatusCodes.Status404NotFound);

        return entity;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryContinuationAuditViewModel> Get([FromQuery] QueryContinuationAuditViewModel viewModel)
    {
        var query = viewModel.MapToQuery<ContinuationAuditQuery>();
        query.QueryFields = typeof(QueryContinuationAuditListViewModel).GetTypeQueryFields();
        query.OrderBy = nameof(ContinuationAudit.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<ContinuationAudit>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryContinuationAuditViewModel, ContinuationAudit>();
    }

    /// <summary>
    /// 创建续读审核
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateContinuationAuditViewModel model)
    {
        // 验证是续读配置否启用状态
        var setup = await _continuationSetupService.Get(model.SetupId);
        if (setup.Enable == false)
        {
            throw new GirvsException(
                StatusCodes.Status400BadRequest,
                new Dictionary<string, string[]> { { setup.Id.ToString(), ["续读申请已禁用",] } }
            );
        }

        var command = _mapper.Map<CreateContinuationAuditCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定续读审核
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("审核", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateContinuationAuditViewModel model)
    {
        var command = _mapper.Map<UpdateContinuationAuditCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}