using Girvs.AuthorizePermission.Enumerations;
using HaoKao.FeedBackService.Application.ViewModels.Suggestion;
using HaoKao.FeedBackService.Domain.Commands.Suggestion;
using HaoKao.FeedBackService.Domain.Entities;
using HaoKao.FeedBackService.Domain.Queries.EntityQuery;
using HaoKao.FeedBackService.Domain.Repositories;

namespace HaoKao.FeedBackService.Application.Services.Management;

/// <summary>
/// 意见反馈接口服务 - Management
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "意见反馈",
    "0675f8b8-75a9-739e-8000-194f20699c77",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class SuggestionService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ISuggestionRepository repository
) : ISuggestionService
{
    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ISuggestionRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;
    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseSuggestionViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Suggestion>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        );

        return entity?.MapToDto<BrowseSuggestionViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="vm">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QuerySuggestionViewModel> Get([FromQuery] QuerySuggestionViewModel vm)
    {
        var query = vm.MapToQuery<SuggestionQuery>();
        query.OrderBy = nameof(Suggestion.CreateTime);
        var cacheKey = GirvsEntityCacheDefaults<Suggestion>.QueryCacheKey.Create(query.GetCacheKey());
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

        return query.MapToQueryDto<QuerySuggestionViewModel, Suggestion>();
    }

    /// <summary>
    /// 创建意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    [NonAction]
    public async Task Create(CreateSuggestionViewModel model)
    {
        var command = model.MapToCommand<CreateSuggestionCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 回复意见反馈
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    public async Task Reply([FromBody] ReplySuggestionViewModel model)
    {
        var command = model.MapToCommand<ReplySuggestionCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 将意见反馈变更为完结
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("{id:guid}")]
    public async Task Close(Guid id)
    {
        var command = new CloseSuggestionCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("回复", Permission.Reply, _UserTypeOfEdit)]
    public void ReplyAuth() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("完结", Permission.Audit, _UserTypeOfEdit)]
    public void CloseAuth() { }
}