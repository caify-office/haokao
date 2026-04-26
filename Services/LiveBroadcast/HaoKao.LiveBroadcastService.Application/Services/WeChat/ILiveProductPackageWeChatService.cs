namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

public interface ILiveProductPackageWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveProductPackageQueryViewModel> Get(LiveProductPackageQueryViewModel queryViewModel);

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesUp(Guid[] id);

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="id">主键</param>
    Task ShelvesDown(Guid[] id);
}