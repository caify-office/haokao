namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveVideoService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取直播播流视频格式类型
    /// </summary>
    Task<IEnumerable<string>> GetLiveUrlType();
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
    /// 创建视频直播
    /// </summary>
    /// <param name="model">模型</param>
    Task Create(CreateLiveVideoViewModel model);

    /// <summary>
    /// 根据主键删除指定视频直播
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定视频直播
    /// </summary>
    /// <param name="model">模型</param>
    Task Update(UpdateLiveVideoViewModel model);

    /// <summary>
    /// 根据主键修改直播状态
    /// </summary>
    /// <param name="model">模型</param>
    Task SetLiveVideoStatus(SetLiveVideoStatusViewModel model);
}