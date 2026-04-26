using HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.Interfaces;

public interface IChapterNodeAppService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseChapterNodeViewModel> Get(Guid id);

    /// <summary>
    /// 根据主键获取指定父节点id
    /// </summary>
    /// <param name="id">主键</param>
    Task<BrowseChapterNodeViewModel> GetBaseParentChapterNode(Guid id);

    /// <summary>
    /// 根据查询获取列表，不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<List<BrowseChapterNodeViewModel>> GetChapterNodeList(Guid subjectId);

    /// <summary>
    /// 获取子章节列表,不带分页
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<BrowseChapterNodeViewModel>> GetChapterNodeListByParentId(Guid parentId);

    /// <summary>
    /// 获取当前科目下所有章节和知识点信息
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<List<BrowseChapterNodeViewModel>> GetChapterNodeKnowledgePointTree(Guid subjectId);

    /// <summary>
    /// 按科目获取章节Id和名称的字典
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    Task<Dictionary<Guid, string>> GetChapterDict(Guid subjectId);
}