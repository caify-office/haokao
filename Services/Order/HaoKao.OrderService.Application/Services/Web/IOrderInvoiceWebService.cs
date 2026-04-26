using HaoKao.OrderService.Application.ViewModels.OrderInvoice;

namespace HaoKao.OrderService.Application.Services.Web;

public interface IOrderInvoiceWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateOrderInvoiceViewModel model);
}