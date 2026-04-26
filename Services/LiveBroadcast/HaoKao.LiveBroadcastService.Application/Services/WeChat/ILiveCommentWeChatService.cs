using HaoKao.LiveBroadcastService.Application.ViewModels.LiveComment;

namespace HaoKao.LiveBroadcastService.Application.Services.WeChat;

/// <summary>
/// 直播评论接口服务
/// </summary>
public interface ILiveCommentWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据直播查询评论
    /// </summary>
    /// <param name="liveId"></param>
    /// <returns></returns>
    Task<BrowseLiveCommentViewModel> Get(Guid liveId);

    /// <summary>
    /// 创建直播评论
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLiveCommentViewModel model);
}