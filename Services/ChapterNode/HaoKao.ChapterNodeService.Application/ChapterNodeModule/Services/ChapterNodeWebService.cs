using HaoKao.ChapterNodeService.Application.ChapterNodeModule.Interfaces;
using HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.Services;

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class ChapterNodeWebService(IChapterNodeAppService appService) : IChapterNodeWebService
{
    #region 初始参数

    private readonly IChapterNodeAppService _appService = appService ?? throw new ArgumentNullException(nameof(appService));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseChapterNodeViewModel> Get(Guid id)
    {
        return _appService.Get(id);
    }

    /// <summary>
    /// 根据主键获取指定父节点id
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    public Task<BrowseChapterNodeViewModel> GetBaseParentChapterNode(Guid id)
    {
        return _appService.GetBaseParentChapterNode(id);
    }

    /// <summary>
    /// 获取章节列表,不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeList(Guid subjectId)
    {
        return _appService.GetChapterNodeList(subjectId);
    }

    /// <summary>
    /// 获取子章节列表,不带分页
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet("{parentId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeListByParentId(Guid parentId)
    {
        return _appService.GetChapterNodeListByParentId(parentId);
    }

    /// <summary>
    /// 获取当前科目下所有章节和知识点信息
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<List<BrowseChapterNodeViewModel>> GetChapterNodeKnowledgePointTree(Guid subjectId)
    {
        return _appService.GetChapterNodeKnowledgePointTree(subjectId);
    }

    /// <summary>
    /// 按科目获取章节Id和名称的字典
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public Task<Dictionary<Guid, string>> GetChapterDict(Guid subjectId)
    {
        return _appService.GetChapterDict(subjectId);
    }

    #endregion
}