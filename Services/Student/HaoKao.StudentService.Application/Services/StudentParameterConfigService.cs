using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员参数设置接口服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员参数设置",
    "7997426d-edef-4bef-9c49-7889a1cf4972",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentParameterConfigService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentParameterConfigRepository repository
) : IStudentParameterConfigService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentParameterConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseStudentParameterConfigViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<StudentParameterConfig>.ByIdCacheKey.Create(id.ToString()),
              () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的学员参数设置不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseStudentParameterConfigViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryStudentParameterConfigViewModel> Get([FromQuery] QueryStudentParameterConfigViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<StudentParameterConfigQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentParameterConfig>.QueryCacheKey.Create(query.GetCacheKey()),
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

        return query.MapToQueryDto<QueryStudentParameterConfigViewModel, StudentParameterConfig>();
    }

    /// <summary>
    /// 创建学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateStudentParameterConfigViewModel model)
    {
        var command = model.MapToCommand<CreateStudentParameterConfigCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定学员参数设置
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteStudentParameterConfigCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定学员参数设置
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateStudentParameterConfigViewModel model)
    {
        var command = model.MapToCommand<UpdateStudentParameterConfigCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}