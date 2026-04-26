using HaoKao.OrderService.Application.ViewModels.PlatformPayer;
using HaoKao.OrderService.Domain.Commands.PlatformPayer;
using HaoKao.OrderService.Domain.Entities;
using HaoKao.OrderService.Domain.Enums;
using HaoKao.OrderService.Domain.Queries;
using HaoKao.OrderService.Domain.Repositories;

namespace HaoKao.OrderService.Application.Services.Management;

/// <summary>
/// 平台配置的支付列表接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "收款账户",
    "2f0151d9-9a6f-44f5-b22f-6e653aa7a658",
    "32",
    SystemModule.SystemModule,
    3
)]
public class PlatformPayerService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPlatformPayerRepository repository
) : IPlatformPayerService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IPlatformPayerRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<BrowsePlatformPayerViewModel> Get(Guid id)
    {
        var platformPayer = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<PlatformPayer>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        );

        if (platformPayer == null) throw new GirvsException("对应的平台配置的支付列表不存在", StatusCodes.Status404NotFound);

        return platformPayer.MapToDto<BrowsePlatformPayerViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<PlatformPayerQueryViewModel> Get([FromQuery] PlatformPayerQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<PlatformPayerQuery>();
        query.OrderBy = nameof(PlatformPayer.CreateTime);
        var key = JsonConvert.SerializeObject(query).ToMd5();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<PlatformPayer>.QueryCacheKey.Create(key),
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

        var dto = query.MapToQueryDto<PlatformPayerQueryViewModel, PlatformPayer>();

        foreach (var x in dto.Result)
        {
            if (x.PaymentMethod == PaymentMethod.Huabei)
            {
                x.ExtendParams = tempQuery.Result.First(i => i.Id == x.Id).Config["InstallmentConfig"];
            }
        }

        return dto;
    }

    /// <summary>
    /// 创建平台配置的支付列表
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Create([FromBody] CreatePlatformPayerViewModel model)
    {
        var command = new CreatePlatformPayerCommand(
            model.Name,
            model.PayerId,
            model.PayerName,
            model.PlatformPayerScenes,
            model.PaymentMethod,
            model.IosIsOpen,
            model.Config
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定平台配置的支付列表
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Update(Guid id, [FromBody] UpdatePlatformPayerViewModel model)
    {
        var command = new UpdatePlatformPayerCommand(
            id,
            model.Name,
            model.PayerId,
            model.PayerName,
            model.PaymentMethod,
            model.IosIsOpen,
            model.PlatformPayerScenes,
            model.Config
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定平台配置的支付列表
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeletePlatformPayerCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("启用", Permission.Edit_Extend1, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Enable(Guid id)
    {
        await SetUseState(id, true);
    }

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="id">主键</param>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Edit_Extend2, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Disable(Guid id)
    {
        await SetUseState(id, false);
    }

    private async Task SetUseState(Guid id, bool useState)
    {
        var command = new SetPlatformPayerUseStateCommand(
            id,
            useState
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}