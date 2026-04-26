using HaoKao.PaperService.Application.Interfaces;
using HaoKao.PaperService.Application.ViewModels;
using HaoKao.PaperService.Domain.Commands;
using HaoKao.PaperService.Domain.Entities;
using HaoKao.PaperService.Domain.Queries;
using HaoKao.PaperService.Domain.Repositories;

namespace HaoKao.PaperService.Application.Services;

/// <summary>
/// 试卷管理
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "试卷管理",
    "356e4b7c-f524-4f6f-b774-0b6677024a7e",
    "64",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class PaperService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IPaperRepository repository
) : IPaperService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IPaperRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowsePaperViewModel> Get(Guid id)
    {
        var paper = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Paper>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的试卷不存在", StatusCodes.Status404NotFound);

        var result = paper.MapToDto<BrowsePaperViewModel>();
        if (!string.IsNullOrEmpty(paper.StructJson))
        {
            result.StructJson = JsonSerializer.Deserialize<dynamic>(paper.StructJson);
        }
        return result;
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<PaperQueryViewModel> Get([FromQuery] PaperQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<PaperQuery>();
        query.QueryFields = typeof(PaperQueryListViewModel).GetTypeQueryFields();
        var cacheKey = GirvsEntityCacheDefaults<Paper>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<PaperQueryViewModel, Paper>();
    }

    [HttpGet("{id:guid}")]
    public  async Task<BrowsePaperViewModel> GetPaperDetailInfo(Guid id)
    {
       var paper = await cacheManager.GetAsync(
      GirvsEntityCacheDefaults<Paper>.ByIdCacheKey.Create(id.ToString()),
      () => repository.GetByIdAsync(id)
  ) ?? throw new GirvsException("对应的试卷不存在", StatusCodes.Status404NotFound);
        return paper.MapToDto<BrowsePaperViewModel>();
    }

    /// <summary>
    /// 创建试卷
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreatePaperViewModel model)
    {
        var command = model.MapToCommand<CreatePaperCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定试卷
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeletePaperCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定试卷
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("更新", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdatePaperViewModel model)
    {
        var command = model.MapToCommand<UpdatePaperCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 设置试题
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("设置试题", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task SetStructJson(Guid id, [FromBody] UpdatePaperStructViewModel model)
    {
        var questionCount = model.StructJson?.TempleteStructDatas?.Sum(x => x.SettingInfo.QuestionCount) ?? 0;
        var totalScore = model.StructJson?.TempleteStructDatas?.Sum(x => x.SettingInfo.TotalScore) ?? 0;
        var structJson = JsonSerializer.Serialize(model.StructJson);

        var command = new UpdatePaperStructCommand(id, structJson, questionCount, totalScore);
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量修改是否限免
    /// </summary>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("批量修改是否限免", Permission.Edit_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdateIsFree([FromBody] UpdateIsFreeViewModel model)
    {
        var command = new UpdateIsFreeCommand(model.Ids, model.IsFree);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 批量修改发布状态
    /// </summary>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("批量修改发布状态", Permission.Publish, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task UpdatePublishState([FromBody] UpdatePublishStateViewModel model)
    {
        var command = new UpdatePublishStateCommand(model.Ids, model.State);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据科目和分类获取试卷列表(Id和名称)
    /// </summary>
    [HttpGet]
    public Task<IReadOnlyList<Paper>> GetPaperList(Guid subjectId, Guid categoryId, Guid userId, [FromServices] IPaperWebService service)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new() { { GirvsIdentityClaimTypes.UserId, userId.ToString() } });
        return  service.Get(subjectId, categoryId);
    }

    #endregion
}