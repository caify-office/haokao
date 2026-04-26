using HaoKao.SubjectService.Application.Interfaces;
using HaoKao.SubjectService.Application.ViewModels;
using HaoKao.SubjectService.Domain.SubjectModule;

namespace HaoKao.SubjectService.Application.Services;

/// <summary>
/// 科目接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "科目管理",
    "a5a69498-2c78-4ee2-994c-4df1fae5d1d2",
    "1",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    4
)]
public class SubjectService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ISubjectRepository repository
) : ISubjectService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ISubjectRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseSubjectViewModel> Get(Guid id)
    {
        var subject = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Subject>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的科目不存在", StatusCodes.Status404NotFound);

        return subject.MapToDto<BrowseSubjectViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<SubjectQueryViewModel> Get([FromQuery] SubjectQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<SubjectQuery>();
        var key = GirvsEntityCacheDefaults<Subject>.QueryCacheKey.Create(query.GetCacheKey());
        var tempQuery = await _cacheManager.GetAsync(key, async () =>
        {
            await _repository.GetByQueryAsync(query);
            return query;
        });
        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<SubjectQueryViewModel, Subject>();
    }

    /// <summary>
    /// 读取科目列表,不带分页
    /// </summary>
    [HttpGet]
    public async Task<IReadOnlyList<BrowseSubjectViewModel>> GetSubjectList()
    {
        var key = GirvsEntityCacheDefaults<Subject>.QueryCacheKey.Create("All");
        var result = await _cacheManager.GetAsync(key, async () =>
        {
            var ts = await _repository.GetAllAsync();
            return ts.Where(x => x.IsShow).ToList();
        });
        return result.MapTo<List<BrowseSubjectViewModel>>();
    }

    /// <summary>
    /// 创建科目
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateSubjectViewModel model)
    {
        var command = new CreateSubjectCommand(
            model.Name,
            model.IsCommon,
            model.Sort,
            model.IsShow
        );

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定科目
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteSubjectCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定科目
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="model">新增模型</param>
    [HttpPut("{id:guid}")]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update(Guid id, [FromBody] UpdateSubjectViewModel model)
    {
        var command = new UpdateSubjectCommand(
            id,
            model.Name,
            model.IsCommon,
            model.Sort,
            model.IsShow
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