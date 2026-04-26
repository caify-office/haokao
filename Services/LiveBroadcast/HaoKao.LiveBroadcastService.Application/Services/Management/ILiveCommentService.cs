using HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 直播评论接口服务
/// </summary>
public interface ILiveCommentService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据直播查询评论
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    Task<BrowseLiveCommentViewModel> Get(Guid liveId);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryLiveCommentViewModel> Get(QueryLiveCommentViewModel queryViewModel);

    /// <summary>
    /// 创建直播评论
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveCommentViewModel model);

    /// <summary>
    /// 查询综合评分
    /// </summary>
    /// <param name="liveId">直播Id</param>
    /// <returns></returns>
    Task<double> GetAverageRating(Guid liveId);
}