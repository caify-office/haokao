using HaoKao.FeedBackService.Application.ViewModels.FeedBackReply;

namespace HaoKao.FeedBackService.Application.Services.Management;

public interface IFeedBackReplyService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建答疑回复
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateFeedBackReplyViewModel model);
}