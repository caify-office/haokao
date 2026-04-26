using HaoKao.LearningPlanService.Application.ViewModels.LearningPlan;

namespace HaoKao.LearningPlanService.Application.Services.Web;

public interface ILearningPlanWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 获取当前用户学习计划
    /// </summary>
    Task<BrowseLearningPlanViewModel> GetCurrentLearningPlan(Guid productId, Guid subjectId);

    /// <summary>
    /// 创建学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateLearningPlanViewModel model);

    /// <summary>
    /// 根据主键删除指定学习计划主类，用于组织和管理一系列学习任务
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 获取任务总数和总时长
    /// </summary>
    /// <param name="model">新增模型</param>
    Task<StatisticsTaskCountDurationsViewModel> Get(QueryTaskCountAndDurationsViewModel model);
}