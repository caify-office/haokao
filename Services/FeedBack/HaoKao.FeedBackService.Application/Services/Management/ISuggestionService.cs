using HaoKao.FeedBackService.Application.ViewModels.Suggestion;

namespace HaoKao.FeedBackService.Application.Services.Management;

public interface ISuggestionService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseSuggestionViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="vm">查询对象</param>
    Task<QuerySuggestionViewModel> Get(QuerySuggestionViewModel vm);

    /// <summary>
    /// 创建意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateSuggestionViewModel model);

    /// <summary>
    /// 回复意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Reply(ReplySuggestionViewModel model);

    /// <summary>
    /// 将意见反馈变更为完结
    /// </summary>
    /// <param name="id"></param>
    Task Close(Guid id);
}