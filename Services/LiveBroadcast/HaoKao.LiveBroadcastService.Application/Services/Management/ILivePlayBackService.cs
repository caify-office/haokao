using HaoKao.LiveBroadcastService.Application.ViewModels.LivePlayBack;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILivePlayBackService : IAppWebApiService, IManager
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

    /// <summary>
    /// 创建直播回放
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(List<CreateLivePlayBackViewModel> model);

    /// <summary>
    /// 根据主键删除指定直播回放
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定直播回放
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(List<UpdateLivePlayBackViewModel> model);
}