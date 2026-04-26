using HaoKao.Common;
using HaoKao.ErrorCorrectingService.Application.AppService.App;
using HaoKao.ErrorCorrectingService.Application.ViewModels.ErrorCorrecting;

namespace HaoKao.ErrorCorrectingService.Application.AppService.WeChat;

/// <summary>
/// 本题纠错接口服务-web
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ErrorCorrectingWeChatService(IErrorCorrectingAppService appService) : IErrorCorrectingWeChatService
{
    /// <summary>
    /// 创建本题纠错
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    public Task Create([FromBody] CreateErrorCorrectingViewModel model)
    {
        return appService.Create(model);
    }
}