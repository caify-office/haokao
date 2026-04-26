using HaoKao.CouponService.Application.ViewModels.UserCoupon;

namespace HaoKao.CouponService.Application.Services.WeChat;

public interface IUserCouponWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseUserCouponViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<UserCouponQueryViewModel> Get(UserCouponQueryViewModel queryViewModel);

    /// <summary>
    /// 是否存在无法使用的优惠卷
    /// </summary>
    /// <param name="couponIds"></param>
    /// <returns></returns>
    Task<bool> IsExistDisenableCoupon([FromBody] Guid[] couponIds);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateUserCouponViewModel model);

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

}