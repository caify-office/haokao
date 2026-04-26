using HaoKao.FeedBackService.Application.ViewModels.FeedBack;

namespace HaoKao.FeedBackService.Application.Services.WeChat;

public interface IFeedBackWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateFeedBackViewModel model);
}