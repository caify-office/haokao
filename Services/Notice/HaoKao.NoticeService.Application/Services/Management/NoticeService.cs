using HaoKao.NoticeService.Application.ViewModels;
using HaoKao.NoticeService.Domain.Commands;
using HaoKao.NoticeService.Domain.Models;
using HaoKao.NoticeService.Domain.Queries;
using HaoKao.NoticeService.Domain.Repositories;

namespace HaoKao.NoticeService.Application.Services.Management;

/// <summary>
/// 公告接口服务 - 管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "公告管理",
    "903777d6-b729-484e-ba2c-29c4a12b31be",
    "32",
    SystemModule.All,
    3
)]
public class NoticeService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    INoticeRepository repository,
    IMapper mapper
) : INoticeService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly INoticeRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    private const UserType _BaseUserType =UserType.AdminUser|UserType.SpecialUser| UserType.TenantAdminUser | UserType.GeneralUser;

    private const UserType _UserTypeOfView = _BaseUserType;
    private const UserType _UserTypeOfPost = _BaseUserType;
    private const UserType _UserTypeOfEdit = _BaseUserType;
    private const UserType _UserTypeOfDelete = _BaseUserType;

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定公告
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<BrowseNoticeViewModel> Get(Guid id)
    {
        var notice = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Notice>.ByIdCacheKey.Create(id.ToString()),
            () => _repository.GetByIdAsync(id)
        );

        if (notice == null)
        {
            throw new GirvsException("对应的公告不存在", StatusCodes.Status404NotFound);
        }

        return _mapper.Map<BrowseNoticeViewModel>(notice);
    }

    /// <summary>
    /// 根据查询获取公告列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, _UserTypeOfView)]
    public async Task<QueryNoticeViewModel> Get([FromQuery] QueryNoticeViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<NoticeQuery>();
        query.OrderBy = nameof(Notice.CreateTime);
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<Notice>.QueryCacheKey.Create(query.GetCacheKey()),
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

        return query.MapToQueryDto<QueryNoticeViewModel, Notice>();
    }

    /// <summary>
    /// 创建公告
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, _UserTypeOfPost)]
    public async Task Create([FromBody] CreateNoticeViewModel model)
    {
        var command = _mapper.Map<CreateNoticeCommand>(model);
        await SendCommand(command);
    }

    /// <summary>
    /// 删除公告
    /// </summary>
    /// <param name="ids">主键</param>
    [HttpDelete]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, _UserTypeOfDelete)]
    public async Task Delete([FromBody] List<Guid> ids)
    {
        var command = new DeleteNoticeCommand(ids);
        await SendCommand(command);
    }

    /// <summary>
    /// 更新公告
    /// </summary>
    /// <param name="model">更新模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task Update([FromBody] UpdateNoticeViewModel model)
    {
        var command = _mapper.Map<UpdateNoticeCommand>(model);
        await SendCommand(command);
    }

    /// <summary>
    /// 修改公告是否弹出
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task UpdatePopup(UpdateNoticePopupViewModel model)
    {
        var command = _mapper.Map<UpdateNoticePopupCommand>(model);
        await SendCommand(command);
    }

    /// <summary>
    /// 修改公告是否发布
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("修改", Permission.Edit, _UserTypeOfEdit)]
    public async Task UpdatePublished(UpdateNoticePublishedViewModel model)
    {
        var command = _mapper.Map<UpdateNoticePublishedCommand>(model);
        await SendCommand(command);
    }

    #endregion

    #region 私有方法

    private async Task SendCommand(IBaseRequest command)
    {
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    #endregion
}