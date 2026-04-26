using Girvs.BusinessBasis;

namespace HaoKao.LearningPlanService.Domain.RemoteRepositories;

public interface IRemoteQuestionRepository : IManager
{
    /// <summary>
    /// 获取试卷基本信息
    /// </summary>
    /// <param name="chaperId"></param>
    /// <param name="questionCategoryId"></param>
    /// <returns></returns>
    Task<int> GetChapterCategorieQuestionCount(Guid chaperId, Guid questionCategoryId);
}