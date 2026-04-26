using HaoKao.Common;
using HaoKao.TenantService.Application.Services.App;
using HaoKao.TenantService.Application.ViewModels;
using System.Collections.Generic;

namespace HaoKao.TenantService.Application.Services.WeChat;

/// <summary>
/// 租户WeChat端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class TenantWeChatService(ITenantAppService appService) : ITenantWeChatService
{
    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public Task<List<TenantQueryListViewModel>> Get()
    {
        return appService.Get();
    }

    /// <summary>
    /// 获取当前租户详情
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<TenantQueryListViewModel> GetCurrent()
    {
        return appService.GetCurrent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [AllowAnonymous]
    public Task<TenantQueryListViewModel> GetAsync(Guid id)
    {
        return appService.GetAsync(id);
    }
}