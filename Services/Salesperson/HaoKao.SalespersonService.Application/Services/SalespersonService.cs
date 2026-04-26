using HaoKao.SalespersonService.Application.Interfaces;
using HaoKao.SalespersonService.Application.ViewModels;
using HaoKao.SalespersonService.Domain.Commands;
using HaoKao.SalespersonService.Domain.Entities;
using HaoKao.SalespersonService.Domain.Queries;
using HaoKao.SalespersonService.Domain.Repositories;

namespace HaoKao.SalespersonService.Application.Services;

/// <summary>
/// 销售人员管理端服务接口
/// </summary>
/// <param name="bus"></param>
/// <param name="cacheManager"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "营销人员管理",
    "08db9d6c-7416-49a3-81a2-59d3ce4aecbd",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class SalespersonService(
    IMediatorHandler bus,
    IStaticCacheManager cacheManager,
    INotificationHandler<DomainNotification> notifications,
    ISalespersonRepository repository
) : ISalespersonService
{
    # region 私有参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ISalespersonRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

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
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseSalespersonViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Salesperson>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的销售人员不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseSalespersonViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="model">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QuerySalespersonViewModel> Get([FromQuery] QuerySalespersonViewModel model)
    {
        var query = model.MapToQuery<SalespersonQuery>();
        query.TenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        query.OrderBy = nameof(Salesperson.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Salesperson>.QueryCacheKey.Create(query.GetCacheKey()),
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

        return query.MapToQueryDto<QuerySalespersonViewModel, Salesperson>();
    }

    /// <summary>
    /// 创建销售人员
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public Task Create([FromBody] CreateSalespersonViewModel model)
    {
        var command = model.MapToCommand<CreateSalespersonCommand>();
        return _bus.TrySendCommand(command, _notifications);
    }

    /// <summary>
    /// 根据主键更新销售人员
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, _UserTypeOfEdit)]
    public Task Update([FromBody] UpdateSalespersonViewModel model)
    {
        var command = model.MapToCommand<UpdateSalespersonCommand>();
        return _bus.TrySendCommand(command, _notifications);
    }

    /// <summary>
    /// 删除销售人员
    /// </summary>
    /// <param name="ids">主键</param>
    /// <returns></returns>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public Task Delete([FromBody] IReadOnlyList<Guid> ids)
    {
        var command = new DeleteSalespersonCommand(ids);
        return _bus.TrySendCommand(command, _notifications);
    }

    #endregion
}