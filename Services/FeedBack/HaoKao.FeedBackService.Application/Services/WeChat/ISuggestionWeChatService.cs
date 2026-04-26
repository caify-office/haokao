using HaoKao.FeedBackService.Application.ViewModels.Suggestion;

namespace HaoKao.FeedBackService.Application.Services.WeChat;

public interface ISuggestionWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseSuggestionViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<QuerySuggestionViewModel> Get(int pageIndex, int pageSize);

    /// <summary>
    /// 创建意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateSuggestionViewModel model);
}