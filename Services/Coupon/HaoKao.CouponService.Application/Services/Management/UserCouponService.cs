using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.ViewModels.UserCoupon;
using HaoKao.CouponService.Domain.Commands.UserCoupon;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.Management;

/// <summary>
/// 用户优惠券
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "用户优惠券",
    "08dba222-ee3d-420b-8a2d-cc2b7d086bbf",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class UserCouponService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IUserCouponRepository repository
) : IUserCouponService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IUserCouponRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseUserCouponViewModel> Get(Guid Id)
    {
        var userCoupon = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<UserCoupon>.ByIdCacheKey.Create(Id.ToString()),
            () => _repository.GetByIdAsync(Id)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return userCoupon.MapToDto<BrowseUserCouponViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<UserCouponQueryViewModel> Get([FromQuery] UserCouponQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<UserCouponQuery>();
        await _repository.GetByQueryAsync(query);
        var result = query.MapToQueryDto<UserCouponQueryViewModel, UserCoupon>();

        return result;
    }

    /// <summary>
    /// 后台查询订单详情使用
    /// </summary>
    /// <param name="OrderId"></param>
    /// <param name="ProductId"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<List<BrowseUserCouponViewModel>> GetUserCouponByOrderIdAndProductId(Guid OrderId, string ProductId)
    {
        var result = await _repository.GetUserCouponByOrderIdAndProductId(OrderId, ProductId);
        return result.MapTo<List<BrowseUserCouponViewModel>>();
    }

    /// <summary>
    /// 是否存在无法使用的优惠卷(存在无效优惠卷返回true，不存在无效优惠卷，返回false)
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<bool> IsExistDisenableCoupon([FromBody] Guid[] couponIds)
    {
        return _repository.IsExistDisableCoupon(couponIds);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateUserCouponViewModel model)
    {
        var command = model.MapToCommand<CreateUserCouponCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteUserCouponCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateUserCouponViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateUserCouponCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}