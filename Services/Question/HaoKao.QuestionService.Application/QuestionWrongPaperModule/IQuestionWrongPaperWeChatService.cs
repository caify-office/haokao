namespace HaoKao.QuestionService.Application.QuestionWrongPaperModule;

/// <summary>
/// 错题组卷微信服务
/// </summary>
public interface IQuestionWrongPaperWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取错题组卷
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowseQuestionWrongPaperViewModel> Get(Guid id);

    /// <summary>
    /// 获取错题组卷列表
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    Task<QueryQuestionWrongPaperViewModel> Get(QueryQuestionWrongPaperViewModel viewModel);

    /// <summary>
    /// 创建错题组卷
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    Task Create(CreateQuestionWrongPaperViewModel viewModel);
}