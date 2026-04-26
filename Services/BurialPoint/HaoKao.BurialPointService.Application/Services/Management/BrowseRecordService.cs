using Girvs.AuthorizePermission.Enumerations;
using Girvs.AuthorizePermission;
using HaoKao.BurialPointService.Application.ViewModels.BrowseRecord;
using Girvs.Infrastructure;
using HaoKao.BurialPointService.Domain.Commands;
using HaoKao.BurialPointService.Domain.Entities;
using HaoKao.Common;
using Microsoft.AspNetCore.Authorization;

namespace HaoKao.BurialPointService.Application.Services.Management;

/// <summary>
/// 浏览记录接口服务-管理端
/// </summary>
/// <summary>
/// 数据埋点分析服务-管理端
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "数据埋点分析",
    "08e9ffb7-a5ed-4f9f-9df8-4eaeaf4791db",
    "512",
    SystemModule.ExtendModule2,
    3
)]
public class BrowseRecordService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    IBrowseRecordRepository repository
) : IBrowseRecordService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly IBrowseRecordRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    #endregion

    #region 服务方法

    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseBrowseRecordViewModel> Get(Guid id)
    {
        var browseRecord = await _cacheManager.GetAsync(
              GirvsEntityCacheDefaults<BrowseRecord>.ByIdCacheKey.Create(id.ToString()),
              async () => await _repository.GetByIdAsync(id)
            );

        if (browseRecord == null)
            throw new GirvsException("对应的浏览记录不存在", StatusCodes.Status404NotFound);

        return browseRecord.MapToDto<BrowseBrowseRecordViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseRecordQueryViewModel> Get([FromQuery] BrowseRecordQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<BrowseRecordQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<BrowseRecord>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }

        return query.MapToQueryDto<BrowseRecordQueryViewModel, BrowseRecord>();
    }

    /// <summary>
    /// 创建浏览记录
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post,
                                       UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateBrowseRecordViewModel model)
    {
        var command = new CreateBrowseRecordCommand(
            model.BurialPointName,
            model.BelongingPortType,
            model.BrowseData,
            model.IsPaidUser
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