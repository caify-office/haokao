namespace HaoKao.DrawPrizeService.Application.Services.WeChat;

public interface IPrizeWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowsePrizeViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<PrizeQueryViewModel> Get(PrizeQueryViewModel queryViewModel);

    /// <summary>
    /// 抽奖
    /// </summary>
    Task<dynamic> Draw(DrawPrizeViewModel model);



}