using HaoKao.CouponService.Application.ViewModels.UserCouponPerformance;

namespace HaoKao.CouponService.Application.Services.Management;

public interface ISystemStatisticsService : IAppWebApiService, IManager
{
    /// <summary>
    /// 销售人员业绩统计
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<QuerySalesPerformanceStatViewModel> GetStaticByPersonUerId(QuerySalesPerformanceStatViewModel model);
}