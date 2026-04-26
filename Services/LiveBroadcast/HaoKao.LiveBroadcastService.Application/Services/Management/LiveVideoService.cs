using Girvs.AuthorizePermission;
using Girvs.AuthorizePermission.Enumerations;
using Girvs.Driven.Extensions;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.Common;
using HaoKao.LiveBroadcastService.Application.Hubs;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Services.Management;

/// <summary>
/// 视频直播接口服务
/// </summary>
[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "视频直播",
    "88a78514-e487-4145-9904-6063403d087d",
    "256",
    SystemModule.ExtendModule2 | SystemModule.ExtendModule3,
    3
)]
public class LiveVideoService(
    IStaticCacheManager cacheManager,
    IMediatorHandler bus,
    INotificationHandler<DomainNotification> notifications,
    ILiveVideoRepository repository,
    OnlineUserState onlineUserState
) : ILiveVideoService
{
    #region 初始参数

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
    private readonly IMediatorHandler _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    private readonly DomainNotificationHandler _notifications = (DomainNotificationHandler)notifications;
    private readonly ILiveVideoRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly OnlineUserState _onlineUserState = onlineUserState ?? throw new ArgumentNullException(nameof(onlineUserState));

    #endregion

    #region 服务方法

    /// <summary>
    /// 获取直播播流视频格式类型
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<string>> GetLiveUrlType()
    {
        var enumFields = _cacheManager.Get(
            GirvsEntityCacheDefaults<LiveVideo>.BuideCustomize("LiveUrlType").Create(),
             () =>
             {
                 Type enumType = typeof(LiveUrlType);

                 // 获取枚举的所有字段（在这个情况下，是枚举的值）
                 var enumFields = from field in enumType.GetFields()
                                  where field.IsLiteral // 确保字段是常量（枚举的值）
                                  select field.Name;
                 return enumFields;

             }
        );
        return await Task.FromResult(enumFields);
    }
    /// <summary>
    /// 根据主键获取指定
    /// </summary>
    /// <param name="id">主键</param>
    [HttpGet("{id:guid}")]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<BrowseLiveVideoViewModel> Get(Guid id)
    {
        var liveVideo = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveVideo>.ByIdCacheKey.Create(id.ToString()),
            async () => await _repository.GetByIdAsync(id)
        ) ?? throw new GirvsException("对应的视频直播不存在", StatusCodes.Status404NotFound);
        return liveVideo.MapToDto<BrowseLiveVideoViewModel>();
    }

    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="queryViewModel">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<LiveVideoQueryViewModel> Get([FromQuery] LiveVideoQueryViewModel queryViewModel)
    {
        var queryCacheKey = JsonConvert.SerializeObject(queryViewModel).ToMd5();
        var query = queryViewModel.MapToQuery<LiveVideoQuery>();
        var tempQuery = await _cacheManager.GetAsync(
            GirvsEntityCacheDefaults<LiveVideo>.QueryCacheKey.Create(queryCacheKey), async () =>
            {
                await _repository.GetByQueryAsync(query);
                return query;
            });

        if (!query.Equals(tempQuery))
        {
            query.RecordCount = tempQuery.RecordCount;
            query.Result = tempQuery.Result;
        }
        var result = query.MapToQueryDto<LiveVideoQueryViewModel, LiveVideo>();

        //统计在线人数
        result.Result.ForEach(x =>
        {
            _onlineUserState.OnlineCount.TryGetValue(x.Id, out var viewCount);
            x.ViewNumber = viewCount;
        });
        return result;
    }

    /// <summary>
    /// 创建视频直播
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPost]
    [ServiceMethodPermissionDescriptor("新增", Permission.Post, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Create([FromBody] CreateLiveVideoViewModel model)
    {
        var command = model.MapToCommand<CreateLiveVideoCommand>();
        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键删除指定视频直播
    /// </summary>
    /// <param name="id">主键</param>
    [HttpDelete("{id:guid}")]
    [ServiceMethodPermissionDescriptor("删除", Permission.Delete, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Delete(Guid id)
    {
        var command = new DeleteLiveVideoCommand(id);

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键更新指定视频直播
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("编辑", Permission.Edit, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task Update([FromBody] UpdateLiveVideoViewModel model)
    {
        var command = model.MapToCommand<UpdateLiveVideoCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }
    }

    /// <summary>
    /// 根据主键修改直播状态
    /// </summary>
    /// <param name="model">新增模型</param>
    [HttpPut]
    [ServiceMethodPermissionDescriptor("更新直播状态", Permission.Edit_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task SetLiveVideoStatus([FromBody] SetLiveVideoStatusViewModel model)
    {
        var command = model.MapToCommand<SetLiveVideoStatusCommand>();

        await _bus.SendCommand(command);

        if (_notifications.HasNotifications())
        {
            var errorMessage = _notifications.GetNotificationMessage();
            throw new GirvsException(StatusCodes.Status400BadRequest, errorMessage);
        }

        var hub = EngineContext.Current.Resolve<IHubContext<LiveChatHub, ILiveChatHub>>();
        await hub.Clients.Group(model.Id.ToString()).LiveStatusChanged(model.LiveStatus);
    }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("上传回看视频", Permission.Post_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void UploadRecordVideo() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("预上架产品", Permission.Post_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void PreloadProduct() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("直播优惠券", Permission.Delete_Extend1, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void LiveBroadcastCoupon() { }

    [HttpGet]
    [ServiceMethodPermissionDescriptor("直播列表地址", Permission.Delete_Extend2, UserType.TenantAdminUser | UserType.GeneralUser)]
    public void LiveBroadcastUrl() { }

    #endregion
}