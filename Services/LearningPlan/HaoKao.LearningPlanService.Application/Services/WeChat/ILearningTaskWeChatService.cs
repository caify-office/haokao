using HaoKao.LearningPlanService.Application.ViewModels.LearningTask;

namespace HaoKao.LearningPlanService.Application.Services.WeChat;

public interface ILearningTaskWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseLearningTaskViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LearningTaskQueryViewModel> Get(LearningTaskQueryViewModel queryViewModel);
}