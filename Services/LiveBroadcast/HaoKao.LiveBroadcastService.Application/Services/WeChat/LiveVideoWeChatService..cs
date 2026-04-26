using Girvs.AuthorizePermission;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 视频直播接口服务--微信小程序端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class LiveVideoWeChatService(ILiveVideoWebService liveVideoWebService) : ILiveVideoWeChatService
{
    #region 初始参数

    private readonly ILiveVideoWebService _liveVideoWebService = liveVideoWebService ?? throw new ArgumentNullException(nameof(liveVideoWebService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseLiveVideoViewModel> Get(Guid id)
    {
        return _liveVideoWebService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<LiveVideoQueryViewModel> Get([FromQuery] LiveVideoQueryViewModel queryViewModel)
    {
        return _liveVideoWebService.Get(queryViewModel);
    }
    /// <summary>
    /// 查询最新直播
    /// </summary>
    [HttpGet]
    public  Task<LiveVideoQueryViewModel> GetNewLive()
    {
        return _liveVideoWebService.GetNewLive();
    } 

    /// <summary>
    /// 根据主键修改直播状态(助教才有权限使用)
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    public  Task SetLiveVideoStatus([FromBody] SetLiveVideoStatusViewModel model)
    {
        return  _liveVideoWebService.SetLiveVideoStatus(model);
    }

    #endregion
}