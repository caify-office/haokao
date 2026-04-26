using HaoKao.LearnProgressService.Application.ViewModels.LearnProgress;

namespace HaoKao.LearnProgressService.Application.AppService.ManageMent;

public interface ILearnProgressService : IAppWebApiService, IManager
{
    /// <summary>
    /// 分产品分科目获取学习时长(单位小时)
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="subjectId"></param>
    /// <param name="userId"></param>
    Task<float> GetLearnDurations(Guid productId, Guid subjectId, Guid userId);
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<LearnProgressQueryViewModel> GetList([FromBody] LearnProgressQueryViewModel queryViewModel);
    Task UpdateThVideoProgress(UpdateThVideoProgressModel Input);

    /// <summary>
    /// 查询单一租户下特定用户特定产品特定科目下特定章节的听课进度数据
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<QueryCourseLearningProgressViewModel> QueryCourseLearningProgress([FromBody] QueryCourseLearningProgressViewModel queryViewModel);


    /// <summary>
    /// 统计学员学习进度
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<CourseLearningProgressStatisticsViewModel> CourseLearningProgressStatistics([FromBody] CourseLearningProgressStatisticsViewModel queryViewModel);
    /// <summary>
    /// 执行半夜任务
    /// </summary>
    /// <returns></returns>
    Task MergeLearnProgress();
}