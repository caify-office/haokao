using HaoKao.LiveBroadcastService.Application.ViewModels.LiveAdministrator;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveAdministratorService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLiveAdministratorViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LiveAdministratorQueryViewModel> Get(LiveAdministratorQueryViewModel queryViewModel);

    /// <summary>
    /// 创建直播管理员
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveAdministratorViewModel model);

    /// <summary>
    /// 根据主键删除指定直播管理员
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);
}