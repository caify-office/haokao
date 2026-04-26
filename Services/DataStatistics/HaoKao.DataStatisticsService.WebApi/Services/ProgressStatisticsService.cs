using Girvs.AuthorizePermission.Enumerations;
using Girvs.Cache.Caching;
using HaoKao.DataStatisticsService.WebApi.CacheKeyManager;
using HaoKao.DataStatisticsService.WebApi.Models;
using HaoKao.DataStatisticsService.WebApi.Queries;
using Microsoft.Extensions.Logging;

namespace HaoKao.DataStatisticsService.WebApi.Services;

[DynamicWebApi(Module = ServiceAreaName.Management)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsJwt)]
[ServicePermissionDescriptor(
    "学员学习情况",
    "89d940a2-c767-40ef-bddd-8e3604875aaa",
    "4096",
    SystemModule.ExtendModule2,
    1
)]
public class ProgressStatisticsService(
    IStaticCacheManager cacheManager,
    IServiceProvider serviceProvider,
    ILogger<ProgressStatisticsService> logger
) : ControllerBase, IAppWebApiService
{
    /// <summary>
    /// 根据查询获取列表，用于分页
    /// </summary>
    /// <param name="query">查询对象</param>
    [HttpGet]
    [ServiceMethodPermissionDescriptor("浏览", Permission.View, UserType.TenantAdminUser | UserType.GeneralUser)]
    public async Task<ProgressStatisticsQuery> Get([FromQuery] ProgressStatisticsQuery query)
    {
        var list = await GetListFromCache();
        if (!list.Any())
        {
            return query;
        }
        query.RecordCount = list.Count(query.GetQueryWhere());
        if (query.RecordCount == 0)
        {
            query.Result = [];
        }
        query.Result = list.Where(query.GetQueryWhere()).Skip(query.PageStart).Take(query.PageSize).ToList();
        return query;
    }

    private async Task<IQueryable<ProgressStatistics>> GetListFromCache()
    {
        var timeSpan = DateTime.Now.AddDays(1.2) - DateTime.Now;
        var cacheKey = ProgressStatisticsCacheKeyManager.All.Create(cacheTime: (int)timeSpan.TotalMinutes);
        var tenantId = EngineContext.Current.ClaimManager.GetTenantId().To<Guid>();
        var list = await cacheManager.GetAsync(cacheKey, () => Task.FromResult(new List<ProgressStatistics>()));
        return list.Where(x => x.TenantId == tenantId).AsQueryable();
    }

    /// <summary>
    /// 发送指定消息
    /// </summary>
    /// <param name="query"></param>
    /// <param name="eventBus"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task SendAssignNotificationMessage([FromBody] SendNotificationMessageQuery query, [FromServices] IEventBus eventBus)
    {
        //if (query.Phones.Count == 0)
        //{
        //    var receiveUser = (await GetListFromCache()).Where(query.GetQueryWhere()).Select(x => x.Phone).Distinct().ToList();
        //    query.Phones = receiveUser;
        //}
        foreach (var phone in query.Phones)
        {
            try
            {
                SendNotificationMessageEvent messageEvent = query.EventReceivingChannel switch
                {
                    EventReceivingChannel.WebChat => new SendWechatNotificationMessageEvent(query.EventNotificationMessageType, phone, []),
                    EventReceivingChannel.InSite => new SendInSiteNotificationMessageEvent(string.Empty, query.EventNotificationMessageType, string.Empty, phone, []),
                    EventReceivingChannel.Mobile => new SendMobileNotificationMessageEvent(string.Empty, query.EventNotificationMessageType, string.Empty, phone, []),
                    _ => throw new ArgumentOutOfRangeException(nameof(query.EventReceivingChannel))
                };

                await eventBus.PublishAsync(messageEvent);
            }
            catch
            {
                // ignored
            }
        }
    }

    /// <summary>
    /// 重新统计学员学习情况并存入缓存
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task StatisticsToCache()
    {
        logger.LogInformation("接收到请求,开始统计数据");
        await using var tenantScopeSp = serviceProvider.CreateAsyncScope();
        await using var dbContext = tenantScopeSp.ServiceProvider.GetRequiredService<DataStatisticsDbContext>();
        var timeSpan = DateTime.Now.AddDays(1.1) - DateTime.Now;
        var cacheKey = ProgressStatisticsCacheKeyManager.All.Create(cacheTime: (int)timeSpan.TotalMinutes);

        dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        var data =  await dbContext.ProgressStatistics.ToListAsync().ConfigureAwait(false);
       if (data.Any())
        {
            cacheManager.SetAsync(cacheKey, data).Wait();
        }
        logger.LogInformation("完成统计数据,并加入到缓存中");
    }

    /// <summary>
    /// 重新统计学员学习情况并存入缓存
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public Task<string> ExecuteStatisticsToCache()
    {
        StatisticsToCache();
        return Task.FromResult("正在重新统计中,请稍后刷新数据");
    }
}