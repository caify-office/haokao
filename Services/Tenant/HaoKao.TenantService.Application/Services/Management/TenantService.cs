using HaoKao.Common;
using HaoKao.TenantService.Application.ViewModels;
using HaoKao.TenantService.Domain.Commands;
using HaoKao.TenantService.Domain.Entities;
using HaoKao.TenantService.Domain.Queries;
using HaoKao.TenantService.Domain.Repositories;

namespace HaoKao.TenantService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "租户管理",
    "5a4fcf52-7696-47e0-b363-2acdd5735dc8",
    "1",
    SystemModule.All,
    1
)]
public class TenantService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ITenantRepository repository
) : ITenantService
{
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;

    /// <summary>
    /// 根据Id获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public async Task<UpdateTenantViewModel> GetAsync(Guid id)
    {
        var result = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Tenant>.ByIdCacheKey.Create(id.ToString()),
            () => repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("未找到对应的数据");

        return result.MapToDto<UpdateTenantViewModel>();
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="queryModel"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<TenantQueryViewModel> GetAsync([FromQuery] TenantQueryViewModel queryModel)
    {
        var query = queryModel.MapToQuery<TenantQuery>();
        var tempQuery = await cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Tenant>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<TenantQueryViewModel, Tenant>();
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<CreateTenantViewModel> CreateAsync([FromBody] CreateTenantViewModel model)
    {
        var command = new CreateTenantCommand(
            model.OtherId,
            model.OtherName,
            model.TenantName,
            model.TenantNo,
            model.AdminUserAcount,
            model.AdminUserName,
            model.AdminPhone,
            model.AdminPassWord,
            model.SystemModule,
            model.AnnualExamTime
        );
        await bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
        return model;
    }

    /// <summary>
    /// 系统管理员修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit_Extend1, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<UpdateTenantViewModel> UpdateAsync([FromBody] UpdateTenantViewModel model)
    {
        var command = new UpdateTenantCommand(
            model.Id,
            model.OtherId,
            model.OtherName,
            model.TenantName,
            model.TenantNo,
            model.AdminUserName,
            model.AdminPhone,
            model.AdminPassWord,
            model.SystemModule,
            model.AnnualExamTime
        );
        await bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 系统管理员设置收款账户
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit_Extend1, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<SetPaymentConfigViewModel> SetPaymentConfigAsync([FromBody] SetPaymentConfigViewModel model)
    {
        var command = new SetPaymentConfigTenantCommand(model.Id, model.PaymentConfigs);
        await bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("启用/禁用", Permission.Post_Extend1, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<EnableDisabledTenantViewModel> EnableDisabledAsync([FromBody] EnableDisabledTenantViewModel model)
    {
        var command = new EnableDisabledExamCommand(model.Id, model.StartState);
        await bus.SendCommand(command);
        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return model;
    }

    /// <summary>
    /// 检查租户代码是否存在
    /// </summary>
    /// <param name="no"></param>
    /// <returns></returns>
    [HttpGet("Exist/{no}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public Task<bool> ExistByNo(string no)
    {
        return repository.ExistEntityAsync(x => x.TenantNo == no);
    }
}