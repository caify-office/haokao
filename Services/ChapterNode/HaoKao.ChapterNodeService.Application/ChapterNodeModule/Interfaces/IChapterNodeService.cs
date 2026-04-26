using HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.Interfaces;

public interface IChapterNodeService : IAppWebApiService, IManager
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
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    Task<ChapterNodeQueryViewModel> Get(ChapterNodeQueryViewModel queryViewModel);

    /// <summary>
    /// 获取章节列表,不带分页
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
    /// 章节树
    /// </summary>
    /// <param name="subjectId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<ChapterNodeTreeCacheItem>> GetTreeByQueryAsync(Guid? subjectId, Guid? id);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    Task Create(CreateChapterNodeViewModel model);

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    Task Delete(Guid id);

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    Task Update(Guid id, UpdateChapterNodeViewModel model);
}