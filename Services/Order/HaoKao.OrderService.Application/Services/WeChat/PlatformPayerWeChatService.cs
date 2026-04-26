using HaoKao.OrderService.Application.Services.Management;
using HaoKao.OrderService.Application.ViewModels.PlatformPayer;
using HaoKao.OrderService.Domain.Enums;

namespace HaoKao.OrderService.Application.Services.WeChat;

/// <summary>
/// 获取平台支付者--小程序
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class PlatformPayerWeChatService(IPlatformPayerService platformPayerService) : IPlatformPayerWeChatService
{
    private readonly IPlatformPayerService _platformPayerService = platformPayerService ?? throw new ArgumentNullException(nameof(platformPayerService));

    [HttpGet]
    public Task<PlatformPayerQueryViewModel> Get(PlatformPayerQueryViewModel queryViewModel)
    {
        queryViewModel.UseState = true;
        queryViewModel.PaymentMethod = PaymentMethod.Wechat;
        queryViewModel.PlatformPayerScenes = PlatformPayerScenes.WechatMiniProgram;
        return _platformPayerService.Get(queryViewModel);
    }
}