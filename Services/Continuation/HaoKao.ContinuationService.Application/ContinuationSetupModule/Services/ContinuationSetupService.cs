using HaoKao.ContinuationService.Application.ContinuationSetupModule.Interfaces;
using HaoKao.ContinuationService.Application.ContinuationSetupModule.ViewModels;
using HaoKao.ContinuationService.Domain.ContinuationAuditModule;
using HaoKao.ContinuationService.Domain.ContinuationSetupModule;

namespace HaoKao.ContinuationService.Application.ContinuationSetupModule.Services;

/// <summary>
/// 续读配置接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "续读配置",
    "d38eadda-b00e-40cc-b20f-dc47ac725550",
    "128",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ContinuationSetupService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IContinuationSetupRepository repository,
    IMapper mapper
) : IContinuationSetupService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IContinuationSetupRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
    public async Task<BrowseContinuationSetupViewModel> Get(Guid id)
    {
        var continuationSetup = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ContinuationSetup>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的续读配置不存在", StatusCodes.Status404NotFound);

        return continuationSetup.MapToDto<BrowseContinuationSetupViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryContinuationSetupViewModel> Get([FromQuery] QueryContinuationSetupViewModel viewModel)
    {
        var query = viewModel.MapToQuery<ContinuationSetupQuery>();
        query.OrderBy = nameof(ContinuationAudit.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<ContinuationSetup>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QueryContinuationSetupViewModel, ContinuationSetup>();
    }

    /// <summary>
    /// 检查同一个产品是否同时出现在有交集的时间段内
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<IEnumerable<Guid>> CheckProducts(CheckProductsViewModel viewModel)
    {
        var queryable = _repository.Query.Where(x => !x.IsDelete && x.StartTime <= viewModel.EndTime && x.EndTime >= viewModel.StartTime);
        var key = queryable.ToQueryString().ToMd5();
        var list = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ContinuationAudit>.QueryCacheKey.Create(key),
            () => queryable.ToListAsync()
        );
        return list.Where(x => x.Id != viewModel.Id).SelectMany(x => x.Products.Select(p => p.ProductId)).Intersect(viewModel.ProductIds);
    }

    /// <summary>
    /// 创建续读配置
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateContinuationSetupViewModel model)
    {
        // 1、同一个产品不能同时出现在有交集的时间段内。
        var products = await CheckProducts(new CheckProductsViewModel
        {
            Id = Guid.Empty,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            ProductIds = model.Products.Select(x => x.ProductId).ToList()
        });
        // 2、保存时判断是否存在“1”的问题，如存在则给出相应提示，提示信息如下：”检测到该时间段内已存在“+产品名称 +”无法重复添加“
        if (products.Any())
        {
            throw new GirvsException(StatusCodes.Status400BadRequest, new { ProductIds = new[] { "检测到该时间段内已存在相同产品", } });
        }

        var command = _mapper.Map<CreateContinuationSetupCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定续读配置
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteContinuationSetupCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定续读配置
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update(Guid id, [FromBody] UpdateContinuationSetupViewModel model)
    {
        model.Id = id;

        // 1、同一个产品不能同时出现在有交集的时间段内。
        var products = await CheckProducts(new CheckProductsViewModel
        {
            Id = id,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            ProductIds = model.Products.Select(x => x.ProductId).ToList()
        });
        // 2、保存时判断是否存在“1”的问题，如存在则给出相应提示，提示信息如下：”检测到该时间段内已存在“+产品名称 +”无法重复添加“
        if (products.Any())
        {
            throw new GirvsException(StatusCodes.Status400BadRequest, new { ProductIds = new[] { "检测到该时间段内已存在相同产品", } });
        }

        var command = _mapper.Map<UpdateContinuationSetupCommand>(model);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用/禁用续读配置
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="enable">启用/禁用</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public async Task Enable(Guid id, bool enable)
    {
        var command = new EnableContinuationSetupCommand(id, enable);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置续读后会员到期时间
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="expiryTime">会员到期时间</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设置续读时间", Permission.Edit_Extend2, _UserTypeOfEdit)]
    public async Task UpdateExpiryTime(Guid id, DateTime expiryTime)
    {
        var command = new UpdateExpiryTimeCommand(id, expiryTime);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}