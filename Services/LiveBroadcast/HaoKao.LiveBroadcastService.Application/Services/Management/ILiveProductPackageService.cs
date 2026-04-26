namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveProductPackageService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveProductPackageViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveProductPackageQueryViewModel> Get(LiveProductPackageQueryViewModel queryViewModel);

    /// <summary>
    /// 创建直播产品包
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(List<CreateLiveProductPackageViewModel> model);

    /// <summary>
    /// 根据主键删除指定直播产品包
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定直播产品包
    /// </summary>
    /// <param name="models">新增模型</param>
    Task Update(List<UpdateLiveProductPackageViewModel> models);

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