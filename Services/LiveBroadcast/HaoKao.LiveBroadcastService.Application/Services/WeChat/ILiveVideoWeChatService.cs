namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ILiveVideoWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveVideoViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveVideoQueryViewModel> Get(LiveVideoQueryViewModel queryViewModel);

    /// <summary>
    /// 查询最新直播
    /// </summary>
    Task<LiveVideoQueryViewModel> GetNewLive();

    /// <summary>
    /// 根据主键修改直播状态
    /// </summary>
    /// <param name="model">模型</param>
    Task SetLiveVideoStatus([FromBody] SetLiveVideoStatusViewModel model);
}