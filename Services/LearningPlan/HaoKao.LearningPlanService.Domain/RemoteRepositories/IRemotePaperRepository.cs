using Girvs.BusinessBasis;
using HaoKao.Common.RemoteModel;

namespace HaoKao.LearningPlanService.Domain.RemoteRepositories;

public interface IRemotePaperRepository : IManager
{
    /// <summary>
    /// 获取试卷基本信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BrowsePaperInfo> GetPaperDetailInfo(Guid id);
}