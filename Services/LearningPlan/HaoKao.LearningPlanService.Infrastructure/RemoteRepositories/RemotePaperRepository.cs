using HaoKao.Common.RemoteModel;
using HaoKao.LearningPlanService.Domain.RemoteRepositories;
using HaoKao.LearningPlanService.Infrastructure.RemoteService;
using Refit;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteRepositories;

public class RemotePaperRepository : IRemotePaperRepository
{
    /// <summary>
    /// 获取试卷基本信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<BrowsePaperInfo> GetPaperDetailInfo(Guid id)
    {
        var remote = EngineContext.Current.Resolve<IRemotePaperService>();
        dynamic response;
        try
        {
            response = await remote.GetPaperDetailInfo(id);
        }
        catch (ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            return null;
        }
        var result = JsonConvert.DeserializeObject<BrowsePaperInfo>(response.GetRawText());
        return result;
    }
}