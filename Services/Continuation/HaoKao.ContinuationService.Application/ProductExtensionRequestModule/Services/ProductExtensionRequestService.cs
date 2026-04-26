using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.Interfaces;
using HaoKao.ContinuationService.Application.ProductExtensionRequestModule.ViewModels;
using HaoKao.ContinuationService.Domain.ProductExtensionRequestModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionRequestModule.Services;

/// <summary>
/// 课程续读申请接口服务实现
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
// [ServicePermissionDescriptor(
//     "课程续读申请",
//     "d38eadda-b00e-40cc-b20f-dc47ac725551",
//     "128",
//     SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
//     3
// )]
public class ProductExtensionRequestService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IProductExtensionRequestRepository repository,
    IMapper mapper
) : IProductExtensionRequestService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IProductExtensionRequestRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;
    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定申请
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    // [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseProductExtensionRequestViewModel> Get(Guid id)
    {
        var request = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductExtensionRequest>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.Query.Include(r => r.AuditLogs).FirstOrDefaultAsync(r => r.Id == id)
        ) ?? throw new GirvsException("对应的课程续读申请不存在", StatusCodes.Status404NotFound);

        return request.MapToDto<BrowseProductExtensionRequestViewModel>();
    }

    /// <summary>
    /// 根据查询获取申请列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    // [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryProductExtensionRequestViewModel> Get([FromQuery] QueryProductExtensionRequestViewModel viewModel)
    {
        var query = _mapper.Map<ProductExtensionRequestQuery>(viewModel);
        query.OrderBy = nameof(ProductExtensionRequest.CreateTime); // 默认按创建时间排序

        var cacheKey = GirvsEntityCacheDefaults<ProductExtensionRequest>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryProductExtensionRequestViewModel, ProductExtensionRequest>();
    }

    /// <summary>
    /// 创建课程续读申请
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    // [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateProductExtensionRequestViewModel model)
    {
        var command = _mapper.Map<CreateProductExtensionRequestCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 更新课程续读申请状态 (审核)
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    // [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateProductExtensionRequestStateViewModel model)
    {
        var command = _mapper.Map<UpdateProductExtensionRequestStateCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}