using HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.Interfaces;
using HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.ViewModels;
using HaoKao.ContinuationService.Domain.ProductExtensionPolicyModule;

namespace HaoKao.ContinuationService.Application.ProductExtensionPolicyModule.Services;

/// <summary>
/// 课程续读策略接口服务实现
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
// [ServicePermissionDescriptor(
//     "课程续读策略",
//     "d38eadda-b00e-40cc-b20f-dc47ac725550",
//     "128",
//     SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
//     3
// )]
public class ProductExtensionPolicyService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IProductExtensionPolicyRepository repository,
    IMapper mapper
) : IProductExtensionPolicyService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IProductExtensionPolicyRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;
    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定策略
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    // [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseProductExtensionPolicyViewModel> Get(Guid id)
    {
        var policy = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ProductExtensionPolicy>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的课程续读策略不存在", StatusCodes.Status404NotFound);

        return policy.MapToDto<BrowseProductExtensionPolicyViewModel>();
    }

    /// <summary>
    /// 根据查询获取策略列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    // [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryProductExtensionPolicyViewModel> Get([FromQuery] QueryProductExtensionPolicyViewModel viewModel)
    {
        var query = _mapper.Map<ProductExtensionPolicyQuery>(viewModel);
        query.OrderBy = nameof(ProductExtensionPolicy.CreateTime); // 默认按创建时间排序

        var cacheKey = GirvsEntityCacheDefaults<ProductExtensionPolicy>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryProductExtensionPolicyViewModel, ProductExtensionPolicy>();
    }

    /// <summary>
    /// 创建课程续读策略
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    // [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateProductExtensionPolicyViewModel model)
    {
        var command = _mapper.Map<CreateProductExtensionPolicyCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 更新课程续读策略
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    // [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateProductExtensionPolicyViewModel model)
    {
        var command = _mapper.Map<UpdateProductExtensionPolicyCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 删除课程续读策略
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    // [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteProductExtensionPolicyCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用/禁用课程续读策略
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="isEnable">是否启用</param>
    [HttpPut("{id:guid}")]
    // [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public async Task Enable(Guid id, bool isEnable)
    {
        var command = new EnableProductExtensionPolicyCommand(id, isEnable);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}