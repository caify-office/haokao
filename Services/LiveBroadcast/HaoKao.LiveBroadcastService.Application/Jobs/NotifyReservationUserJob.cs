using Girvs.EventBus;
using Girvs.Extensions;
using Girvs.Quartz;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Jobs;

public class NotifyReservationUserJob(IServiceProvider serviceProvider, ILogger<NotifyReservationUserJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<ILiveReservationRepository>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        var flag = false;
        var tasks = new List<Task>();

        try
        {
            var data = repository.QueryAllReservation().Result;
            var list = from DataRow dr in data.Rows
                       select new NotifyReservationModel
                       {
                           Id = dr["Id"].To<Guid>(),
                           Name = dr["Name"].ToString(),
                           StartTime = dr["StartTime"].To<DateTime>(),
                           EndTime = dr["EndTime"].To<DateTime>(),
                           Phone = dr["Phone"].ToString(),
                           Source = (ReservationSource)dr["ReservationSource"].To<int>(),
                           TenantId = dr["TenantId"].To<Guid>(),
                           OpenId = dr["OpenId"].ToString()
                       };

            foreach (var item in list)
            {
                // PC端预约的发送短信
                // if (item.Source == ReservationSource.WebSite)
                {
                    var messageEvent = new SendMobileNotificationMessageEvent(
                        Title: "直播预约提醒",
                        EventNotificationMessageType: EventNotificationMessageType.Customize_1,
                        IdCard: string.Empty,
                        PhoneNumber: item.Phone,
                        Parameter: [item.Name, $"{item.StartTime:yyyy-MM-dd HH:mm}",]
                    );
                    tasks.Add(eventBus.PublishAsync(messageEvent));
                }

                // 微信小程序订阅消息
                if (item.Source == ReservationSource.WeChat)
                {
                    var paraData = new Dictionary<string, Dictionary<string, string>>
                    {
                        // 小程序thing类型字段长度限制20
                        ["thing6"] = new() { { "value", new string(item.Name.Take(20).ToArray()) } },
                        ["date7"] = new() { { "value", $"{item.StartTime:yyyy-MM-dd HH:mm}" } }
                    };

                    var messageEvent = new SendWechatNotificationMessageEvent(
                        MessageType: EventNotificationMessageType.Customize_1,
                        OpenId: item.OpenId,
                        Parameter: new Dictionary<string, string>
                        {
                            ["Data"] = JsonConvert.SerializeObject(paraData),
                            ["Lang"] = "zh_CN",
                            ["Page"] = "pages/live/live",
                            ["Miniprogram_state"] = "formal",
                        }
                    );
                    tasks.Add(eventBus.PublishAsync(messageEvent));
                }

                flag = true;
            }

            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            if (flag)
            {
                // 通知成功, 更新为已通知
                Task.WaitAll(list.GroupBy(x => x.TenantId)
                                 .Select(group => repository.UpdateNotified(group.Key, group.Select(g => g.Id).ToList()))
                                 .ToArray());
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}

public class NotifyReservationModel
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; set; }

    public string Phone { get; init; }

    public ReservationSource Source { get; init; }

    public Guid TenantId { get; init; }

    public string OpenId { get; init; }
}