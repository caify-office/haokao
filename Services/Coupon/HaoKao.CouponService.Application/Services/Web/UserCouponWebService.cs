using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.CouponService.Application.Services.Management;
using HaoKao.CouponService.Application.ViewModels.UserCoupon;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HaoKao.CouponService.Application.Services.Web;

/// <summary>
/// 接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class UserCouponWebService(IUserCouponService userCouponService, IUserCouponRepository userCouponRepository) : IUserCouponWebService
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
    /// 查询用户是否具备当前产品包的优惠券
    /// </summary>
    /// <param name="type"></param>
    /// <param name="Productids">键：产品包id，值：产品包下面的产品id拼接字符串</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<Dictionary<string, bool>> HasCoupon(ProductType type, Dictionary<string, string> Productids)
    {
        var dic = new Dictionary<string, bool>();
        var coupons = await _userCouponRepository.GetUserCouponList(type);

        if (coupons.Count == 0)
        {
            Productids.ToList().ForEach(x => { dic.Add(x.Key, false); });
            return dic;
        }

        if (coupons.Count > 0)
        {
            //通用和对应类型优惠卷判定
            var count = 0;
            if (type == ProductType.QuestionBlank)
                count = coupons.Where(x => x.Coupon.Scope == ScopeEnum.All || x.Coupon.Scope == ScopeEnum.QuestionBank).Count();
            else
                count = coupons.Where(x => x.Coupon.Scope == ScopeEnum.All || x.Coupon.Scope == ScopeEnum.Course).Count();

            if (count > 0)
            {
                Productids.ToList().ForEach(x => { dic.Add(x.Key, true); });
                return dic;
            }
            //指定优惠卷判定
            var usercoupons = coupons.Where(x => x.Coupon.Scope == ScopeEnum.custom);
            var scopecount = usercoupons.Count();
            //var productids = string.Join(",", usercoupons.SelectMany(x => x.Coupon.ProductIds).Distinct());
            var couponProductIds = usercoupons.SelectMany(x => x.Coupon.ProductIds).Distinct().ToList();
            if (scopecount > 0)
            {
                Productids.ToList().ForEach(x =>
                {
                    if (string.IsNullOrEmpty(x.Value))
                    {
                        dic.Add(x.Key, false);
                    }
                    else
                    {
                        var productIds = x.Value.Split(',').Select(x => Guid.Parse(x));
                        var hasCoupon = couponProductIds.Intersect(productIds).Count() > 0;
                        dic.Add(x.Key, hasCoupon);
                    }
                });
            }
        }
        return dic;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页(我的优惠券使用)
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public async Task<UserCouponQueryViewModel> Get([FromQuery] UserCouponQueryViewModel queryViewModel)
    {
        queryViewModel.UserId = EngineContext.Current.ClaimManager.IdentityClaim.GetUserId<Guid>();
        return await _usercouponService.Get(queryViewModel);
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
        model.ChannelType = ChannelType.WebSite;
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
    /// 提交订单时候验证优惠券有没有被锁定（主要是用于处理同时提交订单的并发验证方式）
    /// </summary>
    /// <param name="CouponIds"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<int> GetUserCouponCountForOrderLock(string CouponIds)
    {
        return await _userCouponRepository.GetUserCouponCountForOrderLock(CouponIds);
    }

    #endregion
}