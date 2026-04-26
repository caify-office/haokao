using HaoKao.OrderService.Application.ViewModels.OrderInvoice;

namespace HaoKao.OrderService.Application.Services.Management;

public interface IOrderInvoiceService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseOrderInvoiceViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<OrderInvoiceQueryViewModel> Get(OrderInvoiceQueryViewModel queryViewModel);

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateOrderInvoiceViewModel model);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateOrderInvoiceViewModel model);
}