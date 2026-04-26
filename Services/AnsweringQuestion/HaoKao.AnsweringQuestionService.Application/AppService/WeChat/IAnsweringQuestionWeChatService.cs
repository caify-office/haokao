using HaoKao.AnsweringQuestionService.Application.ViewModels.AnsweringQuestion;

namespace HaoKao.AnsweringQuestionService.Application.AppService.WeChat;

public interface IAnsweringQuestionWeChatService : IAppWebApiService, IManager
{
     /// <summary>
     /// 根据主键获取指定
     /// </summary>
     /// <param name="id">主键</param>
     Task<BrowseAnsweringQuestionViewModel> Get(Guid id);

     /// <summary>
     /// 根据查询获取列表，用于分页
     /// </summary>
     /// <param name="queryViewModel">查询对象</param>
     Task<AnsweringQuestionQueryWebViewModel> GetList(AnsweringQuestionQueryWebViewModel queryViewModel);

     /// <summary>
     /// 创建答疑
     /// </summary>
     /// <param name="model">新增模型</param>
     Task Create(CreateAnsweringQuestionViewModel model);

    /// <summary>
    /// 根据主键更新指定答疑
    /// </summary>
    /// <param name="request">request</param>
    Task UpdateWatchCount(UpdateWatchCountModel request);
}