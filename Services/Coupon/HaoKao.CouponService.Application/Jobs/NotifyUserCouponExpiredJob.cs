using Girvs.EventBus;
using Girvs.Extensions;
using Girvs.Quartz;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.CouponService.Domain.Enumerations;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System.Data;
using System.Linq;

namespace HaoKao.CouponService.Application.Jobs;

public class NotifyUserCouponExpiredJob(IServiceProvider serviceProvider, ILogger<NotifyUserCouponExpiredJob> logger) : GirvsJob(serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override void GirvsExecute(IJobExecutionContext context)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserCouponRepository>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        try
        {
            var data = repository.QueryAllExpiredUserCoupon().Result;
            var list = from DataRow dr in data.Rows
                       select new NotifyUserCouponModel
                       {
                           Id = dr["Id"].To<Guid>(),
                           CouponName = dr["CouponName"].ToString(),
                           EndDate = dr["EndDate"].To<DateTime>(),
                           ChannelType = (ChannelType)dr["ChannelType"].To<int>(),
                           OpenId = dr["OpenId"].ToString(),
                           TenantId = dr["TenantId"].To<Guid>()
                       };

            var flag = false;
            var tasks = new List<Task>(data.Rows.Count);
            foreach (var item in list)
            {
                // 微信小程序订阅消息
                if (item.ChannelType == ChannelType.WeChat && !string.IsNullOrEmpty(item.OpenId))
                {
                    var paraData = new Dictionary<string, Dictionary<string, string>>
                    {
                        // 小程序thing类型字段长度限制20
                        ["thing1"] = new() { { "value", "卡券即将到期，点击查看详情使用" } },
                        ["thing2"] = new() { { "value", new string(item.CouponName.Take(20).ToArray()) } },
                        ["time3"] = new() { { "value", $"{item.EndDate:yyyy-MM-dd HH:mm}" } },
                    };

                    var messageEvent = new SendWechatNotificationMessageEvent(
                        MessageType: EventNotificationMessageType.Customize_2,
                        OpenId: item.OpenId,
                        Parameter: new Dictionary<string, string>
                        {
                            ["Data"] = JsonConvert.SerializeObject(paraData),
                            ["Lang"] = "zh_CN",
                            ["Page"] = "pages/couponList/couponList",
                            ["Miniprogram_state"] = "formal",
                        }
                    );

                    tasks.Add(eventBus.PublishAsync(messageEvent));
                    flag = true;
                }
            }

            if (flag)
            {
                Task.WaitAll(tasks.ToArray());
                tasks.Clear();

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

public class NotifyUserCouponModel
{
    public Guid Id { get; set; }

    public string CouponName { get; set; }

    public DateTime EndDate { get; set; }

    public string OpenId { get; set; }

    public ChannelType ChannelType { get; set; }

    public Guid TenantId { get; set; }
}