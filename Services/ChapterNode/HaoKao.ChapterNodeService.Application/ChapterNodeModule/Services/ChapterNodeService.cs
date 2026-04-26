using HaoKao.ChapterNodeService.Application.ChapterNodeModule.Interfaces;
using HaoKao.ChapterNodeService.Application.ChapterNodeModule.ViewModels;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;

namespace HaoKao.ChapterNodeService.Application.ChapterNodeModule.Services;

/// <summary>
/// 章节管理接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "章节管理",
    "a24a012c-2db9-4b7d-8eaa-2abf2b140e8e",
    "1",
    SystemModule.ExtendModule2,
    3
)]
public class ChapterNodeService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IChapterNodeRepository repository
) : IChapterNodeService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IChapterNodeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseChapterNodeViewModel> Get(Guid id)
    {
        var chapterNode = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的不存在", StatusCodes.Status404NotFound);

        return chapterNode.MapToDto<BrowseChapterNodeViewModel>();
    }

    /// <summary>
    /// 根据主键获取指定父节点id
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseChapterNodeViewModel> GetBaseParentChapterNode(Guid id)
    {
        var chapterNode = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"GetBaseParentChapterNode_{id.ToString()}"),
            () => _repository.GetBaseParentChapterNode(id)
        ) ?? throw new GirvsException("对应的父节点不存在", StatusCodes.Status404NotFound);

        return chapterNode.MapToDto<BrowseChapterNodeViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<ChapterNodeQueryViewModel> Get([FromQuery] ChapterNodeQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<ChapterNodeQuery>();
        query.QueryFields = typeof(ChapterNodeQueryListViewModel).GetTypeQueryFields();
        var temp = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(temp))
        {
            query.RecordCount = temp.RecordCount;
            query.Result = temp.Result;
        }

        return query.MapToQueryDto<ChapterNodeQueryViewModel, ChapterNode>();
    }

    /// <summary>
    /// 获取章节列表,不带分页
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<List<BrowseChapterNodeViewModel>> GetChapterNodeList(Guid subjectId)
    {
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"{nameof(GetChapterNodeList)}:subjectId={subjectId}"),
            () => _repository.GetChapterNodeList(subjectId)
        );
        return result.MapTo<List<BrowseChapterNodeViewModel>>();
    }

    /// <summary>
    /// 获取子章节列表,不带分页
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    [HttpGet("{parentId:guid}")]
    public async Task<List<BrowseChapterNodeViewModel>> GetChapterNodeListByParentId(Guid parentId)
    {
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"{nameof(GetChapterNodeListByParentId)}:parentId={parentId}"),
            () => _repository.GetChapterNodeListByParentId(parentId)
        );
        return result.MapTo<List<BrowseChapterNodeViewModel>>();
    }

    /// <summary>
    /// 获取当前科目下所有章节和知识点信息
    ///
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet("{subjectId:guid}")]
    public async Task<List<BrowseChapterNodeViewModel>> GetChapterNodeKnowledgePointTree(Guid subjectId)
    {
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"{nameof(GetChapterNodeKnowledgePointTree)}:subjectId={subjectId}"),
            () => _repository.GetChapterNodeKnowledgePointTree(subjectId));
        return result.MapTo<List<BrowseChapterNodeViewModel>>();
    }

    /// <summary>
    /// 获取数据-章节树列表
    /// </summary>
    /// <param name="id">展开的节点</param>
    /// <param name="subjectId">根据科目搜索</param>
    /// <returns></returns>
    [HttpGet("tree")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public Task<List<ChapterNodeTreeCacheItem>> GetTreeByQueryAsync(Guid? subjectId, Guid? id)
    {
        var key = GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create($"subjectId={subjectId}Id={id}Tree");
        return _cacheManager.GetAsync(key, () => _repository.GetChapterNodeTreeByQueryAsync(subjectId, id));
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateChapterNodeViewModel model)
    {
        var command = model.MapToCommand<CreateChapterNodeCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteChapterNodeCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateChapterNodeViewModel model)
    {
        var command = new UpdateChapterNodeCommand(
            id,
            model.SubjectId,
            model.Code,
            model.Name,
            model.ParentId,
            model.ParentName,
            model.Sort
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}