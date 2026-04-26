using Girvs.BusinessBasis;

namespace HaoKao.ProductService.Domain.RemoteRepositories
{
    public interface IRemoteLearningPlanRepository : IManager
    {
        /// <summary>
        /// 统计任务总数和总时长存入缓存
        /// </summary>
        /// <returns></returns>
        Task StatisticsTaskCountAndDurations(ICollection<AssistantProductPermissionModel> AssistantProductPermissions);
    }
}
