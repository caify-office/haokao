using HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;

namespace HaoKao.CouponService.Application.Services.Management;

public interface IUserCouponPerformanceService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseUserCouponPerformanceViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<UserCouponPerformanceQueryViewModel> Get(UserCouponPerformanceQueryViewModel queryViewModel);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateUserCouponPerformanceViewModel model);

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
    Task Update(Guid id, UpdateUserCouponPerformanceViewModel model);

    /// <summary>
    /// 销售人员业绩统计
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<QuerySalesPerformanceStatViewModel> GetStaticByPersonUerId(QuerySalesPerformanceStatViewModel model);
}