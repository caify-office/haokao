using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.OrderInvoice;

namespace HaoKao.OrderService.Application.Services.Web;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OrderInvoiceWebService(IOrderInvoiceService service) : IOrderInvoiceWebService
{
    [HttpPost]
    public Task Create([FromBody] CreateOrderInvoiceViewModel model)
    {
        return service.Create(model);
    }
}
