using HaoKao.LearningPlanService.Domain.RemoteRepositories;
using HaoKao.LearningPlanService.Infrastructure.RemoteService;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteRepositories;

public class RemoteQuestionRepository : IRemoteQuestionRepository
{
    /// <summary>
    /// 获取试卷基本信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> GetChapterCategorieQuestionCount(Guid chaperId, Guid questionCategoryId)
    {
        var remote = EngineContext.Current.Resolve<IRemoteQuestionService>();
        var count = await remote.GetChaperCategorieQuestionCount(chaperId, questionCategoryId);
        return count;
    }
}