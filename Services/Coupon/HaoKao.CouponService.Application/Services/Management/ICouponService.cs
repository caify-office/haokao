using HaoKao.CouponService.Application.ViewModels.Coupon;

namespace HaoKao.CouponService.Application.Services.Management;

public interface ICouponService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseCouponViewModel> Get(Guid id);

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

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateCouponViewModel model);

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateCouponViewModel model);
}