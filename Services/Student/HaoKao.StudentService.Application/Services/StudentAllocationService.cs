using HaoKao.StudentService.Application.Interfaces;
using HaoKao.StudentService.Application.ViewModels;
using HaoKao.StudentService.Domain.Commands;
using HaoKao.StudentService.Domain.Entities;
using HaoKao.StudentService.Domain.Queries;
using HaoKao.StudentService.Domain.Repositories;

namespace HaoKao.StudentService.Application.Services;

/// <summary>
/// 学员分配服务--管理端
/// </summary>
/// <param name="cacheManager"></param>
/// <param name="bus"></param>
/// <param name="notifications"></param>
/// <param name="repository"></param>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员分配管理",
    "c5a60b56-3063-4de5-8940-e8b0ffe4f715",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class StudentAllocationService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IStudentAllocationRepository repository
) : IStudentAllocationService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IStudentAllocationRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;
    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 分页查询学员分配
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryStudentAllocationViewModel> Get([FromQuery] QueryStudentAllocationViewModel input)
    {
        var query = input.MapToQuery<StudentAllocationQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<StudentAllocation>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByStudentAllocationQueryAsync(query);
                return query;
            });
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }
        return query.MapToQueryDto<QueryStudentAllocationViewModel, StudentAllocation>();
    }

    /// <summary>
    /// 批量修改分配
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, _UserTypeOfEdit)]
    public async Task UpdateAllocateTo([FromBody] UpdateAllocateToViewModel input)
    {
        var command = new UpdateAllocateToCommand(input.Ids, input.SalespersonId, input.SalespersonName);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 备注
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, _UserTypeOfEdit)]
    public async Task UpdateRemark([FromBody] UpdateRemarkViewModel input)
    {
        var command = new UpdateRemarkCommand(input.Id, input.Remark);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}