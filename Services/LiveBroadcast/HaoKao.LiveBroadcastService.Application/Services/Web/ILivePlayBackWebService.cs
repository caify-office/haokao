using HaoKao.LiveBroadcastService.Application.ViewModels.LivePlayBack;

namespace HaoKao.LiveBroadcastService.Application.Services.Web;

public interface ILivePlayBackWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLivePlayBackViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LivePlayBackQueryViewModel> Get(LivePlayBackQueryViewModel queryViewModel);

   
}