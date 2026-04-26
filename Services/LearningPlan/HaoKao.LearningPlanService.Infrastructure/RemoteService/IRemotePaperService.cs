using Girvs.BusinessBasis.Dto;
using Girvs.Refit;
using HaoKao.Common.RemoteService;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace HaoKao.LearningPlanService.Infrastructure.RemoteService;

[RefitService(RefitServiceNames.PaperService)]
public interface IRemotePaperService: IGirvsRefit
{
    /// <summary>
    /// ��ȡ�Ծ�������Ϣ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Get("/api/WeChat/PaperWeChatService/PaperDetailInfo/{id}")]
    Task<dynamic> GetPaperDetailInfo([FromHeader]Guid id);
}
