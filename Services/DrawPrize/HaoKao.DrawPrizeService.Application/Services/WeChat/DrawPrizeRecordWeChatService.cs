using Girvs.Extensions;
using HaoKao.DrawPrizeService.Application.Services.Management;

namespace HaoKao.DrawPrizeService.Application.Services.WeChat;

/// <summary>
/// 抽奖记录服务-Web端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DrawPrizeRecordWeChatService(IDrawPrizeRecordService drawPrizeRecordService) : IDrawPrizeRecordWeChatService
{
    #region 初始参数

    private readonly IDrawPrizeRecordService _drawPrizeRecordService = drawPrizeRecordService ?? throw new ArgumentNullException(nameof(drawPrizeRecordService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    public Task<BrowseDrawPrizeRecordViewModel> Get(Guid id)
    {
        return _drawPrizeRecordService.Get(id);
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    public Task<DrawPrizeRecordQueryViewModel> Get([FromQuery] DrawPrizeRecordQueryViewModel queryViewModel)
    {
        //防止访问他们数据
        queryViewModel.CreatorId = EngineContext.Current.ClaimManager.GetUserId().ToGuid();
        return _drawPrizeRecordService.Get(queryViewModel);
    }

    #endregion
}