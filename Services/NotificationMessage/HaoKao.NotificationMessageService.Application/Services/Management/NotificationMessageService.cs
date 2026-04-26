using HaoKao.Common;
using HaoKao.Common.Extensions;
using System.Text.Json;

namespace HaoKao.NotificationMessageService.Application.Services.Management;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "消息管理",
    "d3630823-e57f-45d9-a351-c10f7702490c",
    "32",
    SystemModule.All,
    1
)]
public class NotificationMessageService(
    IRepository<NotificationMessage> repository,
    IStaticCacheManager staticCacheManager,
    IEventBus eventBus)
    : INotificationMessageService
{
    private readonly IRepository<NotificationMessage> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager ?? throw new ArgumentNullException(nameof(staticCacheManager));
    private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

    /// <summary>
    /// 根据查询条件获取消息
    /// </summary>
    /// <param name="queryViewModel"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("浏览", Permission.View)]
    [HttpGet]
    public async Task<NotificationMessageQueryViewModel> Get([FromQuery] NotificationMessageQueryViewModel queryViewModel)
    {
        var query = queryViewModel.MapToQuery<NotificationMessageQuery>();
        query.QueryFields = typeof(NotificationMessageQueryListViewModel).GetTypeQueryFields();
        query.OrderBy = nameof(NotificationMessage.CreateTime);

        // var tempQuery = await _staticCacheManager.GetAsync(
        //     GirvsEntityCacheDefaults<NotificationMessage>.QueryCacheKey.Create(query.GetCacheKey()), async () =>
        //     {
        //         await _repository.GetByQueryAsync(query);
        //         return query;
        //     });
        //
        // if (!query.Equals(tempQuery))
        // {
        //     query.RecordCount = tempQuery.RecordCount;
        //     query.Result = tempQuery.Result;
        // }

        await _repository.GetByQueryAsync(query);

        return query.MapToQueryDto<NotificationMessageQueryViewModel, NotificationMessage>();
    }

    /// <summary>
    /// 根据Excel表格发送指定消息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ServiceMethodPermissionDescriptor("发送指定消息", Permission.Post, UserType.AdminUser | UserType.SpecialUser)]
    [HttpPost]
    public async Task SendAssignNotificationMessage(AssignNotificationMessageViewModel model)
    {
        foreach (var user in model.ReceiveUser)
        {
            try
            {
                SendNotificationMessageEvent messageEvent = model.EventReceivingChannel switch
                {
                    EventReceivingChannel.WebChat => new SendWechatNotificationMessageEvent(
                        model.EventNotificationMessageType, user.PhoneNumber,
                        new Dictionary<string, string>()
                    ),
                    EventReceivingChannel.InSite => new SendInSiteNotificationMessageEvent(
                        string.Empty,
                        model.EventNotificationMessageType, user.IdCard, user.PhoneNumber, []
                    ),
                    EventReceivingChannel.Mobile => new SendMobileNotificationMessageEvent(
                        string.Empty,
                        model.EventNotificationMessageType, user.IdCard, user.PhoneNumber, []
                    ),
                    _ => throw new ArgumentOutOfRangeException(nameof(model.EventReceivingChannel))
                };

                await _eventBus.PublishAsync(messageEvent);
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// 获取消息通知类型
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.AdminUser | UserType.SpecialUser | UserType.TenantAdminUser | UserType.GeneralUser)]
    public dynamic GetEventNotificationMessageType()
    {
        var cacheKey = GirvsEntityCacheDefaults<MessageSetting>.BuideCustomize("EventNotificationMessageType:{0}")
          .Create("Enum");
        var result = staticCacheManager.Get<string>(cacheKey, () =>
        {
            var r = typeof(EventNotificationMessageType).GetEnumTypeKeyValue();
            return System.Text.Json.JsonSerializer.Serialize(r, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        });
        return System.Text.Json.JsonSerializer.Deserialize<dynamic>(result);
    }
}