using HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.Services.WeChat;

public interface IKnowledgePointWeChatService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<KnowledgePointBrowseViewModel> Get(Guid id);

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<KnowledgePointQueryViewModel> Get(KnowledgePointQueryViewModel queryViewModel);
}