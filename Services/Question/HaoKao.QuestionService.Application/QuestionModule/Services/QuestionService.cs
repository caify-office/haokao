using HaoKao.QuestionService.Application.QuestionModule.Interfaces;
using HaoKao.QuestionService.Application.QuestionModule.ViewModels;
using HaoKao.QuestionService.Domain.QuestionModule;
using HaoKao.QuestionService.Domain.Works;

namespace HaoKao.QuestionService.Application.QuestionModule.Services;

/// <summary>
/// 试题实体接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor
    (
        serviceName: "试题管理",
        serviceId: "9eccef4d-1eb5-453e-9270-97125c1d318d",
        tag: "64",
        SystemModule.ExtendModule2,
        order: 3
    ),
]
public class QuestionService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IQuestionRepository repository,
    IMapper mapper
) : IQuestionService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IQuestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseQuestionViewModel> Get(Guid id)
    {
        var question = await GetByIdAsync(id);
        return question.MapToDto<BrowseQuestionViewModel>();
    }

    [NonAction]
    public async Task<Question> GetByIdAsync(Guid id)
    {
        var question = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的试题不存在", StatusCodes.Status404NotFound);

        return question;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryQuestionViewModel> Get([FromQuery] QueryQuestionViewModel viewModel)
    {
        var query = viewModel.MapToQuery<QuestionQuery>();
        query.QueryFields = typeof(BrowseQuestionViewModel).GetTypeQueryFields();
        query.OrderBy = nameof(Question.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(cacheKey, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<QueryQuestionViewModel, Question>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页(用于试卷引入试题的列表)
    /// </summary>
    /// <param name="viewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryPaperViewModel> GetQuestionListForPaper([FromQuery] QueryPaperViewModel viewModel)
    {
        var query = viewModel.MapToQuery<QuestionQuery>();
        query.ParentId = null;
        query.EnableState = EnableState.Enable;
        query.QueryFields = typeof(QueryPaperListViewModel).GetTypeQueryFields();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(query.GetCacheKey()),
            async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            }
        );
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }
        return query.MapToQueryDto<QueryPaperViewModel, Question>();
    }

    /// <summary>
    /// 用于获取试卷题目列表
    /// </summary>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<List<BrowsePaperViewModel>> GetPaperQuestionList([FromBody] IEnumerable<Guid> ids)
    {
        var cacheKey = $"{nameof(GetPaperQuestionList)}:ids={string.Join(",", ids).ToMd5()}";
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey), () =>
            {
                return _repository.Query.Where(x => ids.Contains(x.Id))
                                  .Select(x => new BrowsePaperViewModel
                                  {
                                      Id = x.Id,
                                      QuestionTypeId = x.QuestionTypeId,
                                      QuestionText = x.QuestionText,
                                      QuestionCount = x.QuestionCount,
                                  }).ToListAsync();
            }
        );
        return result;
    }

    /// <summary>
    /// 根据 案例分析题id集合 获取下面所有小题题干和id
    /// </summary>
    /// <param name="parentIds"></param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<dynamic> GetPaperSubQuestionList([FromBody] IEnumerable<Guid> parentIds)
    {
        var cacheKey = $"{nameof(GetPaperSubQuestionList)}:parentIds={string.Join(",", parentIds).ToMd5()}";
        var result = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            async () =>
            {
                var list = await _repository.GetPaperSubQuestionListByParentIdsAsync(parentIds);
                return _mapper.Map<List<BrowsePaperViewModel>>(list);
            }
        );

        return result.GroupBy(x => x.ParentId).Select(x => new
        {
            Id = x.Key,
            Questions = x.ToList(),
        }).ToList();
    }

    /// <summary>
    /// 根据试题Id获取试题选项
    /// </summary>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<dynamic> GetQuestionOptions(Guid id)
    {
        var cacheKey = GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create($"{nameof(GetQuestionOptions)}:{id}");
        var jsonStr = await _cacheManager.GetAsync(cacheKey, () => _repository.GetQuestionOptionsByIdAsync(id));
        return JsonSerializer.Deserialize<dynamic>(jsonStr);
    }

    /// <summary>
    /// 按试题章节和试题分类查询试题数量
    /// </summary>
    /// <param name="chapterId"></param>
    /// <param name="questionCategoryId"></param>
    /// <returns></returns>
    [HttpGet("{chapterId:guid}/{questionCategoryId:guid}")]
    [AllowAnonymous]
    public Task<int> GetChaperCategorieQuestionCount(Guid chapterId, Guid questionCategoryId)
    {
        var cacheKey = $"{nameof(GetChaperCategorieQuestionCount)}:{$"{chapterId}_{questionCategoryId}".ToMd5()}";
        return _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Question>.QueryCacheKey.Create(cacheKey),
            () => _repository.GetChaperCategorieQuestionCount(chapterId, questionCategoryId)
        );
    }

    /// <summary>
    /// 创建试题实体
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateQuestionViewModel model)
    {
        var command = model.MapToCommand<CreateQuestionCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定试题实体
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete([FromBody] IEnumerable<Guid> ids)
    {
        var command = new DeleteQuestionCommand(ids);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定试题实体
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateQuestionViewModel model)
    {
        var command = model.MapToCommand<UpdateQuestionCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设为免费专区
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("设为免费", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public Task SetFreeState([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateFreeState(ids, FreeState.Yes);
    }

    /// <summary>
    /// 取消免费专区
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("取消免费", Permission.Edit_Extend1, _UserTypeOfEdit)]
    public Task CancelFreeState([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateFreeState(ids, FreeState.No);
    }

    /// <summary>
    /// 修改免费专区状态
    /// </summary>
    private async Task UpdateFreeState(IEnumerable<Guid> ids, FreeState freeState)
    {
        var command = new UpdateFreeStateCommand(ids, freeState);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 启用
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("启用", Permission.Publish, _UserTypeOfEdit)]
    public Task EnableQuestion([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateEnableState(ids, EnableState.Enable);
    }

    /// <summary>
    /// 禁用
    /// </summary>
    [HttpPatch]
    [ServiceMethodPermissionDescriptor("禁用", Permission.Publish, _UserTypeOfEdit)]
    public Task DisableQuestion([FromBody] IEnumerable<Guid> ids)
    {
        return UpdateEnableState(ids, EnableState.Disable);
    }

    /// <summary>
    /// 修改启用状态
    /// </summary>
    private async Task UpdateEnableState(IEnumerable<Guid> ids, EnableState enableState)
    {
        var command = new UpdateEnableStateCommand(ids, enableState);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 修改排序
    /// </summary>
    [HttpPatch("{id:guid}")]
    public async Task UpdateSort(Guid id, int sort)
    {
        var command = new UpdateSortCommand(id, sort);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据科目获取试题的数量
    /// </summary>
    /// <param name="subjectId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<int> GetQuestionCount(Guid subjectId)
    {
        return _repository.Query.CountAsync(x => x.SubjectId == subjectId && x.QuestionTypeId != QuestionType.CaseAnalysis);
    }

    /// <summary>
    /// 根据科目和分类获取章节列表和题目数量(包含节)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IReadOnlyList<ChapterViewModel>> GetChapterList([FromQuery] QueryChapterViewModel input, Guid userId)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.UserId, userId.ToString() } });
        var service = EngineContext.Current.Resolve<IQuestionAppService>();
        return service.GetChapterList(input);
    }

    #endregion

    [HttpGet, AllowAnonymous]
    public Task InitQuestionTitle([FromServices] IInitQuestionTitleWork work)
    {
        return work.ExecuteAsync();
    }

    [HttpGet, AllowAnonymous]
    public Task InitQuestionCount([FromServices] IInitQuestionCountWork work)
    {
        return work.ExecuteAsync();
    }
}