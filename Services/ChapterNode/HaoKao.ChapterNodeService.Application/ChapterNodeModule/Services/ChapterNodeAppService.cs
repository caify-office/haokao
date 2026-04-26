using HaoKao.ChapterNodeService.Application.ChapterNodeModule.Interfaces;
using HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.Services;

/// <summary>
/// App端章节接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.App)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ChapterNodeAppService(
    IStaticCacheManager cacheManager,
    IChapterNodeRepository repository,
    IChapterNodeService service
) : IChapterNodeAppService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IChapterNodeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseChapterNodeViewModel> Get(Guid id)
    {
        return service.Get(id);
    }

    /// <summary>
    /// 根据主键获取指定父节点id
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseChapterNodeViewModel> GetBaseParentChapterNode(Guid id)
    {
        return service.GetBaseParentChapterNode(id);
    }

    /// <summary>
    /// 获取章节列表,不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeList(Guid subjectId)
    {
        return service.GetChapterNodeList(subjectId);
    }

    /// <summary>
    /// 获取子章节列表,不带分页
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet("{parentId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeListByParentId(Guid parentId)
    {
        return service.GetChapterNodeListByParentId(parentId);
    }

    /// <summary>
    /// 获取当前科目下所有章节和知识点信息
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeKnowledgePointTree(Guid subjectId)
    {
        return service.GetChapterNodeKnowledgePointTree(subjectId);
    }

    /// <summary>
    /// 按科目获取章节Id和名称的字典
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<Dictionary<Guid, string>> GetChapterDict(Guid subjectId)
    {
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"{nameof(GetChapterDict)}:subjectId={subjectId}"),
            () => _repository.GetChapterDictionary(subjectId)
        );
    }

    #endregion
}