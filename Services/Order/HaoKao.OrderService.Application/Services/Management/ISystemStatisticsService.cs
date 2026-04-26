using HaoKao.OrderService.Application.ViewModels.Order;

namespace HaoKao.OrderService.Application.Services.Management;

public interface ISystemStatisticsService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取销售统计列表
    /// </summary>
    Task<SalesStatQueryViewModel> GetSalesStatList(SalesStatQueryViewModel model);

    /// <summary>
    /// 获取销售统计详情列表
    /// </summary>
    Task<SalesStatDetailQueryViewModel> GetSalesStatDetailList(SalesStatDetailQueryViewModel model);
}