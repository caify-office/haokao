using HaoKao.KnowledgePointService.Application.ViewModels.KnowledgePoint;

namespace HaoKao.KnowledgePointService.Application.Services.App;

public interface IKnowledgePointAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<KnowledgePointBrowseViewModel> Get(Guid id);
}