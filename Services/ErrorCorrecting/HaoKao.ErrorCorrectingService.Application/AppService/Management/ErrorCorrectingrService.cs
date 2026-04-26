using Girvs.AuthorizePermission.Enumerations;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;
using HaoKao.ErrorCorrectingService.Domain.Commands;
using HaoKao.ErrorCorrectingService.Domain.Entities;
using HaoKao.ErrorCorrectingService.Domain.Queries;
using HaoKao.ErrorCorrectingService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.ErrorCorrectingService.Application.AppService.Management;

/// <summary>
/// 本题纠错接口服务-ManageMent
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "错题反馈",
    "c767362f-c5fc-40ce-b06f-c6b40135a3aa",
    "64",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class ErrorCorrectingService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IErrorCorrectingRepository repository
) : IErrorCorrectingService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IErrorCorrectingRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseErrorCorrectingViewModel> Get(Guid id)
    {
        var questionChecker = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ErrorCorrecting>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的纠错不存在", StatusCodes.Status404NotFound);

        return questionChecker.MapToDto<BrowseErrorCorrectingViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<ErrorCorrectingQueryViewModel> Get([FromQuery] ErrorCorrectingQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<ErrorCorrectingQuery>();
        await _repository.GetByQueryAsync(query);
        return query.MapToQueryDto<ErrorCorrectingQueryViewModel, ErrorCorrecting>();
    }

    /// <summary>
    /// 根据主键删除指定本题纠错
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteErrorCorrectingCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定本题纠错
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateErrorCorrectingViewModel model)
    {
        var command = new UpdateErrorCorrectingCommand(id, model.Status);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}