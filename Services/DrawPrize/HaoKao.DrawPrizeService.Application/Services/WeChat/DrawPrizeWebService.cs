using HaoKao.DrawPrizeService.Application.Services.Management;

namespace HaoKao.DrawPrizeService.Application.Services.WeChat;

/// <summary>
/// 社区抽奖服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
[AllowAnonymous]
public class DrawPrizeWeChatService(IDrawPrizeService drawPrizeService) : IDrawPrizeWeChatService
{
    #region 初始参数
    private readonly IDrawPrizeService _drawPrizeService = drawPrizeService ?? throw new ArgumentNullException(nameof(drawPrizeService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseDrawPrizeViewModel> Get(Guid id)
    {
        return _drawPrizeService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<DrawPrizeQueryViewModel> Get([FromQuery] DrawPrizeQueryViewModel queryViewModel)
    {
        return _drawPrizeService.Get(queryViewModel);
    }
    #endregion
}