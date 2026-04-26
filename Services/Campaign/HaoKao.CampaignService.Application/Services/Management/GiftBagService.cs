using HaoKao.CampaignService.Application.ViewModels.GiftBag;
using HaoKao.CampaignService.Domain.Commands;
using HaoKao.CampaignService.Domain.Entities;
using HaoKao.CampaignService.Domain.Queries;
using HaoKao.CampaignService.Domain.ReceiveRules;
using HaoKao.CampaignService.Domain.Repositories;

namespace HaoKao.CampaignService.Application.Services.Management;

/// <summary>
/// 活动管理-管理端接口
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "活动管理",
    "4f1b442b-f23e-4c92-83f8-82934965b008",
    "512",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class GiftBagService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IMapper mapper,
    IGiftBagRepository repository
) : IGiftBagService
{
    #region 私有参数

    private const UserType _BaseUserType = UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IGiftBagRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseGiftBagViewModel> Get(Guid id)
    {
        var entity = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GiftBag>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的礼品包不存在", StatusCodes.Status404NotFound);

        return entity.MapToDto<BrowseGiftBagViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryGiftBagViewModel> Get([FromQuery] QueryGiftBagViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<GiftBagQuery>();
        query.OrderBy = nameof(GiftBag.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<GiftBag>.QueryCacheKey.Create(query.GetCacheKey()),
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

        return query.MapToQueryDto<QueryGiftBagViewModel, GiftBag>();
    }

    /// <summary>
    /// 获取礼包领取规则
    /// </summary>
    /// <param name="rules">规则</param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public IReadOnlyList<IReceiveRule> GetReceiveRules([FromServices] IEnumerable<IReceiveRule> rules)
    {
        return rules.Where(x => !x.Internal).ToList();
    }

    /// <summary>
    /// 创建礼品包
    /// </summary>
    /// <param name="model">新增模型</param>
    /// <returns></returns>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public Task Create([FromBody] CreateGiftBagViewModel model)
    {
        var command = _mapper.Map<CreateGiftBagCommand>(model);
        return TrySendCommand(command);
    }

    /// <summary>
    /// 删除礼品包
    /// </summary>
    /// <param name="ids">主键</param>
    /// <returns></returns>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public Task Delete([FromBody] IReadOnlyList<Guid> ids)
    {
        var command = new DeleteGiftBagCommand(ids);
        return TrySendCommand(command);
    }

    /// <summary>
    /// 根据主键更新礼品包
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, _UserTypeOfEdit)]
    public Task Update([FromBody] UpdateGiftBagViewModel model)
    {
        var command = _mapper.Map<UpdateGiftBagCommand>(model);
        return TrySendCommand(command);
    }

    /// <summary>
    /// 修改发布状态
    /// </summary>
    /// <param name="model">更新模型</param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("发布", Permission.Publish, _UserTypeOfEdit)]
    public Task Publish([FromBody] UpdateGiftBagPublishedViewModel model)
    {
        var command = _mapper.Map<UpdateGiftBagPublishedCommand>(model);
        return TrySendCommand(command);
    }

    private async Task<T> TrySendCommand<T>(IRequest<T> command)
    {
        var result = await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        return (T)result;
    }
}