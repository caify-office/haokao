using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.Services.Management;
using HaoKao.CouponService.Application.ViewModels.UserCoupon;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.CouponService.Application.Services.WeChat;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class UserCouponWeChatService(
     IUserCouponRepository userCouponRepository,
    IUserCouponService userCouponService
) : IUserCouponWeChatService
{
    #region 初始参数
    private readonly IUserCouponService _usercouponService = userCouponService ?? throw new ArgumentNullException(nameof(userCouponService));
    private readonly IUserCouponRepository _userCouponRepository = userCouponRepository ?? throw new ArgumentNullException(nameof(userCouponRepository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="Id">主键</param>
    [HttpGet("{id}")]
    public async Task<BrowseUserCouponViewModel> Get(Guid Id)
    {
        return await _usercouponService.Get(Id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页(我的优惠券列表使用)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<UserCouponQueryViewModel> Get([FromQuery] UserCouponQueryViewModel queryViewModel)
    {
        queryViewModel.UserId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return await _usercouponService.Get(queryViewModel);
    }
    /// <summary>
    /// 是否存在无法使用的优惠卷(存在无效优惠卷返回true，不存在无效优惠卷，返回false)
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<bool> IsExistDisenableCoupon([FromBody] Guid[] couponIds)
    {
        return _usercouponService.IsExistDisenableCoupon(couponIds);
    }
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public async Task Create([FromBody] CreateUserCouponViewModel model)
    {
         model.ChannelType = ChannelType.WeChat;
         await _usercouponService.Create(model);
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        await _usercouponService.Delete(id);
    }

    /// <summary>
    ///  查询当前时间可用优惠券(下单选择使用)（下单查询使用）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<BrowseUserCouponViewModel>> GetUserCouponList(ProductType type)
    {
        var result = await _userCouponRepository.GetUserCouponList(type);
        var data = result.MapTo<List<BrowseUserCouponViewModel>>();
        return data;
    }
    #endregion
}