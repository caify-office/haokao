using Girvs.EventBus;
using Girvs.Infrastructure;
using Girvs.Quartz;
using HaoKao.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Linq;

namespace HaoKao.LearningPlanService.Application.Jobs;

public class LearningRemindJob(
    IServiceProvider serviceProvider,
    ILogger<LearningRemindJob> logger) : GirvsJob(serviceProvider)
{
    private readonly ILogger<LearningRemindJob> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override async void GirvsExecute(IJobExecutionContext context)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var learningPlanRepository = scope.ServiceProvider.GetRequiredService<ILearningPlanRepository>();

        //分表问题
        var tableNames = await learningPlanRepository.GetTableNames();
        foreach (var table in tableNames)
        {
            table.SetTenantId();
            await RemindCurrentTenant(scope);
        }

        static async Task RemindCurrentTenant(AsyncServiceScope scope)
        {
            var learningPlanRepository = scope.ServiceProvider.GetRequiredService<ILearningPlanRepository>();
            var learningTaskrepository = scope.ServiceProvider.GetRequiredService<ILearningTaskRepository>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var staticCache = scope.ServiceProvider.GetRequiredService<IStaticCacheManager>();
            //读取所有需要提醒的学习计划到缓存中
            var now = DateTime.Now.Date;
            var cacheTime = 25 * 60;
            var tenantId = EngineContext.Current.ClaimManager.GetTenantId();
            var learningPlaneCacheKey = buildCacheKey(tenantId).Create(nameof(LearningPlan), now.ToString(), cacheTime);
            var needRemindPlan = await staticCache.GetAsync(learningPlaneCacheKey, () => { return learningPlanRepository.GetWhere(x => x.NeedReminder == true); });
            if (!needRemindPlan.Any()) return;
            //将需要提醒的任务计划中今日学习任务存入缓存中
            var learningTaskCacheCkey = buildCacheKey(tenantId).Create(nameof(LearningTask), now.ToString(), cacheTime);
            var todayNeedRemindLeranTask = await staticCache.GetAsync(learningTaskCacheCkey, () =>
            {
                var nowDateOnly = DateOnly.FromDateTime(now);
                return learningTaskrepository.GetWhere(x => x.LearningPlan.NeedReminder && x.ScheduledTime == nowDateOnly);
            });
            if (!todayNeedRemindLeranTask.Any()) return;
            //根据当前时间筛选对应的学习计划
            var needRemindPlanCurrent = needRemindPlan.Where(x => x.ReminderHours == now.Hour && x.ReminderMinutes == now.Minute);
            var needRemindPlanId = needRemindPlanCurrent.Select(x => x.Id).ToList();
            //用户id和手机号组成字典，方便快速查询
            var needRemindPlanCurrentDic = needRemindPlanCurrent.ToDictionary(x => x.CreatorId, x => x.ReminderPhone);

            var needRemindTask = todayNeedRemindLeranTask.Where(x => needRemindPlanId.Contains(x.LearningPlanId)).ToList();
            var needRemindTaskDic = needRemindTask.GroupBy(x => x.CreatorId).ToDictionary(x => x.Key, x => x.ToList().Select(x => x.TaskName));
            //通知这些计划相关学员今天要学习的学习内容
            foreach (var item in needRemindTaskDic)
            {
                if (needRemindPlanCurrentDic.ContainsKey(item.Key))
                {
                    //短信消息
                    //var phone = needRemindPlanCurrentDic[item.Key];
                    //var message = new SendMobileNotificationMessageEvent("学习提醒", EventNotificationMessageType.Customize_3,
                    //                                              string.Empty, phone,
                    //                                              item.Value.ToArray());
                    //eventBus.PublishAsync(message);


                    //站内消息
                    //var insiteMessage = new SendInSiteNotificationMessageEvent(
                    //     string.Empty,
                    //     model.EventNotificationMessageType, user.IdCard, user.PhoneNumber, []
                    // );
                    //eventBus.PublishAsync(insiteMessage);

                    //微信消息
                    // var messageEvent = new SendWechatNotificationMessageEvent(
                    //    MessageType: EventNotificationMessageType.Customize_2,
                    //    OpenId: item.OpenId,
                    //    Parameter: new Dictionary<string, string>
                    //    {
                    //        ["Data"] = JsonConvert.SerializeObject(paraData),
                    //        ["Lang"] = "zh_CN",
                    //        ["Page"] = "pages/couponList/couponList",
                    //        ["Miniprogram_state"] = "formal",
                    //    }
                    //);
                    // eventBus.PublishAsync(messageEvent);
                }
            }
        }

        static CacheKey buildCacheKey(string tenantId)
        {
            return new CacheKey($"{nameof(LearningRemindJob).ToLowerInvariant()}:TenantId_{tenantId}:all:{{0}}");
        }
    }
}