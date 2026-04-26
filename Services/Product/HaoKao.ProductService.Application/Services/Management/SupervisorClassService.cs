using Girvs.Driven.Extensions;
using HaoKao.ProductService.Application.ViewModels.SupervisorClass;
using HaoKao.ProductService.Domain.Commands.SupervisorClass;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 班级督学接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "班级督学",
    "95a50cfd-61cf-b7df-1220-117af042461f",
    "4096",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class SupervisorClassService : ISupervisorClassService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager;
    private readonly IMediatorHandler _bus;
    private readonly DomainNotificationHandler _notifications;
    private readonly ISupervisorClassRepository _repository;

    #endregion

    #region 构造函数

    /// <summary>
    /// 构造函数
    /// </summary>
    public SupervisorClassService(
        IStaticCacheManager cacheManager,
        IMediatorHandler bus,
        INotificationHandler<DomainNotification> notifications,
        ISupervisorClassRepository repository
    )
    {
        _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _notifications = (DomainNotificationHandler) notifications;
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseSupervisorClassViewModel> Get(Guid id)
    {
        var supervisorClass = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<SupervisorClass>.ByIdCacheKey.Create(id.ToString()),
              async () => await _repository.GetByIdAsync(id)
            ) ?? throw new GirvsException("对应的班级督学不存在", StatusCodes.Status404NotFound);
        return supervisorClass.MapToDto<BrowseSupervisorClassViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<SupervisorClassQueryViewModel> Get([FromQuery]SupervisorClassQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<SupervisorClassQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<SupervisorClass>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<SupervisorClassQueryViewModel, SupervisorClass>();
    }
            
    /// <summary>
    /// 创建班级督学
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody]CreateSupervisorClassViewModel model)
    {
        var command = model.MapToCommand<CreateSupervisorClassCommand>();
        
        await _bus.SendCommand(command);
        
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定班级督学
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteSupervisorClassCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定班级督学
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateSupervisorClassViewModel model)
    {
        var command = model.MapToCommand<UpdateSupervisorClassCommand>();
        
        await _bus.SendCommand(command);
        
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}