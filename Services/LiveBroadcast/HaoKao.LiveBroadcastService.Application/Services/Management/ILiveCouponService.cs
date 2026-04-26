using HaoKao.LiveBroadcastService.Application.ViewModels.LiveCoupon;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveCouponService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveCouponViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveCouponQueryViewModel> Get(LiveCouponQueryViewModel queryViewModel);

    /// <summary>
    /// 创建直播优惠卷
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(List<CreateLiveCouponViewModel> model);

    /// <summary>
    /// 根据主键删除指定直播优惠卷
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定直播优惠卷
    /// </summary>
    /// <param name="models">新增模型</param>
    Task Update(List<UpdateLiveCouponViewModel> models);

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesUp(Guid[] id);

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesDown(Guid[] id);
}