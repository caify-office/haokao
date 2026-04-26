using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LivePlayBack;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播回放接口服务-微信小程序端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LivePlayBackWeChatService(
    ILivePlayBackService service) : ILivePlayBackWeChatService
{
    #region 初始参数

    private readonly ILivePlayBackService _service = service ?? throw new ArgumentNullException(nameof(service));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public  Task<BrowseLivePlayBackViewModel> Get(Guid id)
    {
        
        return _service.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public  Task<LivePlayBackQueryViewModel> Get([FromQuery] LivePlayBackQueryViewModel queryViewModel)
    {
        return _service.Get(queryViewModel);
    }

    #endregion
}