using HaoKao.ChapterNodeService.Application.KnowledgePointModule.ViewModels;
using HaoKao.ChapterNodeService.Domain.ChapterNodeModule;
using HaoKao.ChapterNodeService.Domain.KnowledgePointModule;
using System.Linq;

namespace HaoKao.ChapterNodeService.Application.KnowledgePointModule.Services.Management;

/// <summary>
/// 知识点接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "知识点管理",
    "3526fc0e-03a4-3254-a0de-336a107af6fd",
    "1",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class KnowledgePointService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IKnowledgePointRepository repository,
    IChapterNodeRepository chapterNodeRepository
) : IKnowledgePointService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IKnowledgePointRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IChapterNodeRepository _chapterNodeRepository = chapterNodeRepository;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<KnowledgePointBrowseViewModel> Get(Guid id)
    {
        var registerUser = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<KnowledgePoint>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        );

        if (registerUser == null)
        {
            throw new GirvsException("对应的知识点不存在", StatusCodes.Status404NotFound);
        }

        return registerUser.MapToDto<KnowledgePointBrowseViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<KnowledgePointQueryViewModel> Get([FromQuery] KnowledgePointQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<KnowledgePointQuery>();
        if (query.ChapterNodeId.HasValue)
        {
            query.ChildrenChapterNodeIds =await cacheManager.GetAsync(GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create("AllChildrenChapterNodeId_" + query.ChapterNodeId.ToString()), async () => await _chapterNodeRepository.GetAllChildrenChapterNodeId(query.ChapterNodeId.Value));
        }
        if (query.SubjectId.HasValue)
        {
            query.SubjectIdAllChapterNodeIds = await cacheManager.GetAsync(GirvsEntityCacheDefaults<ChapterNode>.QueryCacheKey.Create("Subject_"+ query.SubjectId.ToString()), async () =>
            {
                var result = await _chapterNodeRepository.GetWhereAsync(x => x.SubjectId == queryViewModel.SubjectId.Value);
                return result.Select(x=>x.Id).ToArray();
            } );
        }
        query.QueryFields = typeof(KnowledgePointQueryListViewModel).GetTypeQueryFields();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<KnowledgePoint>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }
        return query.MapToQueryDto<KnowledgePointQueryViewModel, KnowledgePoint>();
    }

    /// <summary>
    /// 创建知识点
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] KnowledgePointCreateViewModel model)
    {
        var command = model.MapToCommand<CreateKnowledgePointCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定知识点
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteKnowledgePointCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定知识点
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] KnowledgePointUpdateViewModel model)
    {
        var command = model.MapToCommand<UpdateKnowledgePointCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}