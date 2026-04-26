using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.BusinessBasis.QueryTypeFields;
using HaoKao.Common;
using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Entities;
using HaoKao.OpenPlatformService.Domain.Queries;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

/// <summary>
/// 注册用户接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "注册用户",
    "8e5204ea-ab3b-4908-88c4-db19d597ccbd",
    "32",
    SystemModule.SystemModule,
    3
)]
public class RegisterUserService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IRegisterUserRepository repository
) : IRegisterUserService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IRegisterUserRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<BrowseRegisterUserViewModel> Get(Guid id)
    {
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        return registerUser.MapToDto<BrowseRegisterUserViewModel>();
    }

    /// <summary>
    /// 根据手机号码获取用户信息
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<BrowseRegisterUserViewModel> GetByPhone(string phone)
    {
        var registerUser = await _repository.GetAsync(x => x.Phone == phone);
        return registerUser.MapToDto<BrowseRegisterUserViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<RegisterUserQueryViewModel> Get([FromQuery] RegisterUserQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<RegisterUserQuery>();
        var cacheKey = GirvsEntityCacheDefaults<RegisterUser>.QueryCacheKey.Create(query.GetCacheKey());
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
        return query.MapToQueryDto<RegisterUserQueryViewModel, RegisterUser>();
    }

    [HttpGet, AllowAnonymous]
    public async Task<dynamic> ExportWeiXinUser(DateTime start, DateTime end)
    {
        var users = await _repository.GetAllWithWeiXin(); ;
        return users.Where(x => x.CreateTime >= start && x.CreateTime <= end)
                    .Select(x => new { x.NickName, x.Phone, x.CreateTime, x.ExternalIdentities.First().UniqueIdentifier })
                    .ToList();
    }

    /// <summary>
    /// 根据主键删除指定注册用户
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.AdminUser | UserType.SpecialUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteRegisterUserCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 禁用用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Edit_Extend1, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<bool> Disable(Guid id)
    {
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        var command = new UpdateRegisterUserCommand(
            registerUser.Id,
            registerUser.Phone,
            null,
            registerUser.UserGender,
            registerUser.NickName,
            UserState.Disable,
            registerUser.EmailAddress,
            registerUser.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 启用用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("启用", Permission.Edit_Extend2, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<bool> Enable(Guid id)
    {
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        var command = new UpdateRegisterUserCommand(
            registerUser.Id,
            registerUser.Phone,
            null,
            registerUser.UserGender,
            registerUser.NickName,
            UserState.Enable,
            registerUser.EmailAddress,
            registerUser.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("重置密码", Permission.Relation, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<bool> ResetPassword(Guid id, string password)
    {
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        var command = new UpdateRegisterUserCommand(
            registerUser.Id,
            registerUser.Phone,
            password,
            registerUser.UserGender,
            registerUser.NickName,
            registerUser.UserState,
            registerUser.EmailAddress,
            registerUser.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }

    /// <summary>
    /// 修改用户手机号码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改手机号码", Permission.Relation, UserType.AdminUser | UserType.SpecialUser)]
    public async Task<bool> ModifyPhone(Guid id, string phone)
    {
        if (await _repository.ExistEntityAsync(x => x.Phone == phone))
        {
            throw new GirvsException("对应的手机号码已存在", StatusCodes.Status404NotFound);
        }

        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<RegisterUser>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的注册用户不存在", StatusCodes.Status404NotFound);

        var command = new UpdateRegisterUserCommand(
            registerUser.Id,
            phone,
            null,
            registerUser.UserGender,
            registerUser.NickName,
            registerUser.UserState,
            registerUser.EmailAddress,
            registerUser.HeadImage
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return true;
    }


    #endregion
}