using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

public interface ILiveOnlineUserService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryLiveOnlineUserViewModel> Get(QueryLiveOnlineUserViewModel queryViewModel);

    /// <summary>
    /// 根据直播Id获取用户在线数据
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    Task<LiveOnlineUser> GetUserByLiveId(Guid liveId);

    /// <summary>
    /// 在线人数走势统计
    /// </summary>
    /// <param name="liveId">直播Id</param>
    /// <param name="interval">时间间隔(分钟)</param>
    Task<List<LiveOnlineUserTrend>> GetOnlineTrendStat(Guid liveId, int interval);

    /// <summary>
    /// 创建直播在线用户
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveOnlineUserViewModel model);

    /// <summary>
    /// 根据主键更新指定在线用户
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Update(UpdateLiveOnlineUserViewModel model);
}