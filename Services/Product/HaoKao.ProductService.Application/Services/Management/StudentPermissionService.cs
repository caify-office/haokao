using Girvs.EventBus;
using HaoKao.Common.Events.Student;
using HaoKao.ProductService.Application.ViewModels.StudentPermission;
using HaoKao.ProductService.Domain.Commands.StudentPermission;
using HaoKao.ProductService.Domain.Entities;
using HaoKao.ProductService.Domain.Queries;
using HaoKao.ProductService.Domain.Repositories;
using HaoKao.ProductService.Infrastructure;

namespace HaoKao.ProductService.Application.Services.Management;

/// <summary>
/// 学员权限表接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员权限管理",
    "c30fa7ac-bcba-febd-a8fe-e5f253c6f3cd",
    "2048",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentPermissionService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentPermissionRepository repository,
    IEventBus eventBus
) : IStudentPermissionService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentPermissionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseStudentPermissionViewModel> Get(Guid id)
    {
        var studentPermission = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentPermission>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的学员权限不存在", StatusCodes.Status404NotFound);

        return studentPermission.MapToDto<BrowseStudentPermissionViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<StudentPermissionQueryViewModel> Get([FromQuery] StudentPermissionQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<StudentPermissionQuery>();
        query.OrderBy = nameof(StudentPermission.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentPermission>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<StudentPermissionQueryViewModel, StudentPermission>();
    }

    /// <summary>
    /// 创建学员权限表
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateStudentPermissionViewModel model)
    {
        var command = new CreateStudentPermissionCommand(
            model.StudentName,
            model.StudentId,
            model.OrderNumber,
            model.ProductId,
            model.ProductName,
            model.ExpiryTime,
            model.Enable
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定学员权限表
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteStudentPermissionCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定学员权限状态
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPatch("{id}")]
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ChangeState(Guid id, [FromBody] UpdateStudentPermissionStateViewModel model)
    {
        var command = new UpdateStudentPermissionStateCommand(id, model.Enable);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定学员权限到期时间
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改到期时间", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task ChangeExpiryTime(Guid id, UpdateStudentPermissionExpiryTimeViewModel model)
    {
        var command = new UpdateStudentPermissionExpiryTimeCommand(id, model.ExpiryTime);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 获取学员权限操作日志
    /// </summary>
    /// <param name="model"></param>
    /// <param name="operateLogRepository"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QueryStudentPermissionOperateLogViewModel> GetOperateLog(
        [FromQuery] QueryStudentPermissionOperateLogViewModel model,
        [FromServices] IStudentPermissionOperateLogRepository operateLogRepository)
    {
        var query = model.MapToQuery<StudentPermissionOperateLogQuery>();
        query.OrderBy = nameof(StudentPermissionOperateLog.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentPermissionOperateLog>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await operateLogRepository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryStudentPermissionOperateLogViewModel, StudentPermissionOperateLog>();
    }

    /// <summary>
    /// 学员权限表向学员表同步数据(前端不需要调用)
    /// </summary>
    /// <returns></returns>
    [HttpPut, AllowAnonymous]
    public Task SynchronizeStudents()
    {
        var dbcontext = EngineContext.Current.Resolve<ProductDbContext>();
        dbcontext.StudentPermissions.ToList()
            .ForEach(student =>
            {
                _eventBus.PublishAsync(new CreateStudentEvent(student.StudentId, student.TenantId));
            });
        return Task.CompletedTask;
    }

    /// <summary>
    /// 将学员权限表数据移入分表中(前端不需要调用)
    /// 执行当前接口前的准备工作：
    /// 第一步：StudentPermission实体实现ITenantShardingTable 接口
    /// 第二步:将所有迁移文件删除重新生成一个迁移文件
    /// 第三步:将新生成迁移文件名称复制到__EFMigrationsHistory表中
    /// </summary>
    /// <returns></returns>
    [HttpPut, AllowAnonymous]
    public Task SplitDataToShareTable()
    {
        //暂时关闭此功能
        return Task.CompletedTask;
        return _repository.SplitDataToShareTable();
    }

    #endregion
}