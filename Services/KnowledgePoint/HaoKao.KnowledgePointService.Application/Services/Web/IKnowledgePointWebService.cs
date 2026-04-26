using HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

namespace HaoKao.KnowledgePointService.Application.Services.Web;

public interface IKnowledgePointWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<KnowledgePointBrowseViewModel> Get(Guid id);
}