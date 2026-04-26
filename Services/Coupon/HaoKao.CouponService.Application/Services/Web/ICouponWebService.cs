using HaoKao.CouponService.Application.ViewModels.Coupon;

namespace HaoKao.CouponService.Application.Services.Web;

public interface ICouponWebService : IAppWebApiService, IManager
{

    /// <summary>
    /// 根据主键数组获取指定
    /// </summary>
    /// <param name="ids">主键</param>
    Task<List<BrowseCouponViewModel>> GetByIds(Guid[] ids);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<CouponQueryViewModel> Get(CouponQueryViewModel queryViewModel);
}