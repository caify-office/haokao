using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;
using HaoKao.CouponService.Domain.Commands.UserCouponPerformance;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Models;
using HaoKao.CouponService.Domain.Queries;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.Management;

/// <summary>
/// 业绩管理
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "业绩管理",
    "08db9d6c-7416-49ea-829a-423d996c3b65",
    "1024",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class UserCouponPerformanceService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IUserCouponPerformanceRepository repository,
    IUserCouponRepository userCouponRepository,
    IMapper mapper
) : IUserCouponPerformanceService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IUserCouponPerformanceRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IUserCouponRepository _userCouponRepository = userCouponRepository ?? throw new ArgumentNullException(nameof(userCouponRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseUserCouponPerformanceViewModel> Get(Guid Id)
    {
        var userCouponPerformance = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<UserCouponPerformance>.ByIdCacheKey.Create(Id.ToString()),
            () => _repository.GetByIdAsync(Id)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);
        return userCouponPerformance.MapToDto<BrowseUserCouponPerformanceViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<UserCouponPerformanceQueryViewModel> Get([FromQuery] UserCouponPerformanceQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<UserCouponPerformanceQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<UserCouponPerformance>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<UserCouponPerformanceQueryViewModel, UserCouponPerformance>();
    }

    /// <summary>
    /// 批量生成业绩记录
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GirvsException"></exception>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task CreateBatch()
    {
        var coupons = _userCouponRepository.GetUserCouponListForBatch().Result;
        if (coupons is { Count: > 0 })
        {
            foreach (var coupon in coupons)
            {
                //已经生成过的不在生成
                var userCouponPerformance = _repository.GetByIdAsync(coupon.Id).Result;
                //实名券才生成,非实名券不生成
                if (userCouponPerformance == null && coupon.Coupon.CouponType == CouponTypeEnum.SmCoupon)
                {
                    var command = new CreateUserCouponPerformanceCommand(
                        coupon.Id,
                        coupon.OrderNo,
                        coupon.OrderId,
                        coupon.TelPhone,
                        coupon.NickName,
                        coupon.ProductName,
                        coupon.FactAmount,
                        coupon.Amount,
                        coupon.PayTime,
                        coupon.Remark,
                        coupon.Coupon.PersonName,
                        coupon.Coupon.PersonUserId
                    );
                    await _bus.SendCommand(command);
                    if (_notifications.HasNotifications())
                    {
                        var errorMessage = _notifications.GetNotificationMessage();
                        throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("创建", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateUserCouponPerformanceViewModel model)
    {
        model.Id = Guid.Empty;
        var command = model.MapToCommand<CreateUserCouponPerformanceCommand>();
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
        var command = new DeleteUserCouponPerformanceCommand(id);

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
    public async Task Update(Guid id, [FromBody] UpdateUserCouponPerformanceViewModel model)
    {
        model.Id = id;
        var command = model.MapToCommand<UpdateUserCouponPerformanceCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 销售人员业绩统计
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("实名销售统计", Permission.Read, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<QuerySalesPerformanceStatViewModel> GetStaticByPersonUerId([FromQuery] QuerySalesPerformanceStatViewModel model)
    {
        model.TenantIds ??= [EngineContext.Current.ClaimManager.GetTenantId().To<Guid>()];

        var (count, data) = await _repository.GetSalesPerformanceStat(
            model.StartTime,
            model.EndTime,
            model.PageIndex,
            model.PageSize,
            [.. model.TenantIds]
        );

        model.RecordCount = count;
        model.Result = _mapper.Map<List<SalesPerformanceStatModel>>(data);

        return model;
    }

    #endregion
}