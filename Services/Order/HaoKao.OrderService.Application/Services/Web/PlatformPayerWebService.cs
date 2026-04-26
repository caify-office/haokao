using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.PlatformPayer;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.Services.Web;

/// <summary>
/// 支付方式服务--Web
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PlatformPayerWebService(IPlatformPayerService platformPayerService) : IPlatformPayerWebService
{
    private readonly IPlatformPayerService _platformPayerService = platformPayerService ?? throw new ArgumentNullException(nameof(platformPayerService));

    /// <summary>
    /// 获取当前租户指定的支付方式
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<PlatformPayerQueryViewModel> Get(PlatformPayerQueryViewModel queryViewModel)
    {
        queryViewModel.UseState = true;
        queryViewModel.PlatformPayerScenes = PlatformPayerScenes.WebSite;
        return _platformPayerService.Get(queryViewModel);
    }
}