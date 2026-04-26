using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员分配设置服务--管理端
/// </summary>
/// <param name="bus"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员分配设置",
    "066e2b2e-8ffd-7039-8000-3e687bebd826",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentAllocationConfigService(
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentAllocationConfigRepository repository
) : IStudentAllocationConfigService
{
    #region 初始参数

    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentAllocationConfigRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly Guid _tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();

    #endregion

    #region 服务方法

    /// <summary>
    /// 获取当前租户下的学员分配配置
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseStudentAllocationConfigViewModel> Get()
    {
        var entity = await _repository.GetAsync(x => x.TenantId == _tenantId);
        return entity?.MapToDto<BrowseStudentAllocationConfigViewModel>();
    }

    /// <summary>
    /// 保存学员分配配置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("保存", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] SaveStudentAllocationConfigViewModel model)
    {
        var command = new SaveStudentAllocationConfigCommand(model.Data, model.WaysOfAllocation, _tenantId);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}