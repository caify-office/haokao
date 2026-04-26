using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.PlatformPayer;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.Services.App;

/// <summary>
/// 获取平台支付者--App
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PlatformPayerAppService(IPlatformPayerService platformPayerService) : IPlatformPayerAppService
{
    private readonly IPlatformPayerService _platformPayerService = platformPayerService ?? throw new ArgumentNullException(nameof(platformPayerService));

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<PlatformPayerQueryViewModel> Get(PlatformPayerQueryViewModel queryViewModel)
    {
        queryViewModel.UseState = true;
        queryViewModel.PlatformPayerScenes = PlatformPayerScenes.App;
        return _platformPayerService.Get(queryViewModel);
    }
}