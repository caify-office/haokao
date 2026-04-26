using HaoKao.OpenPlatformService.Application.ViewModels.RegisterUser;

namespace HaoKao.OpenPlatformService.Application.Services.Management;

public interface IProductMetricsService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取总注册用户数, 今日注册用户数, 今日活跃用户数
    /// </summary>
    /// <returns></returns>
    Task<dynamic> QueryRegisteredCountAndActiveCount();

    /// <summary>
    /// 每日注册用户走势
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    Task<List<DailyTrendListViewModel>> GetDailyRegisteredUserTrend(DailyTrendQueryViewModel queryViewModel);

    /// <summary>
    /// 查询用户注册客户端分组数据
    /// </summary>
    /// <returns></returns>
    Task<List<ClientGroupingListViewModel>> QueryRegisteredUserClientGrouping(ProductMetricsQueryViewModel queryViewModel);
}