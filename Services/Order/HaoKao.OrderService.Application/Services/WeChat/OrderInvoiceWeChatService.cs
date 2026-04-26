using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.OrderInvoice;

namespace HaoKao.OrderService.Application.Services.WeChat;

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OrderInvoiceWeChatService(IOrderInvoiceService service) : IOrderInvoiceWeChatService
{
    [HttpPost]
    public Task Create([FromBody] CreateOrderInvoiceViewModel model)
    {
        return service.Create(model);
    }
}
