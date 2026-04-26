using Girvs.EventBus;
using Girvs.Infrastructure;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.LiveBroadcastService.Domain.Repositories;
using HaoKao.LiveBroadcastService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.LiveBroadcastService.WebApi.Controller;

[ApiController]
[Route("[controller]")]
public class LogController : ControllerBase
{
    [HttpGet("Information")]
    [AllowAnonymous]
    public Task<IActionResult> Information()
    {
        return DownloadLogFile("Information");
    }

    [HttpGet("Warning")]
    [AllowAnonymous]
    public Task<IActionResult> Warning()
    {
        return DownloadLogFile("Warning");
    }

    [HttpGet("Error")]
    [AllowAnonymous]
    public Task<IActionResult> Error()
    {
        return DownloadLogFile("Error");
    }

    private async Task<IActionResult> DownloadLogFile(string type)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs", type);
        if (!Directory.Exists(directoryPath))
        {
            return NotFound(directoryPath);
        }
        var files = Directory.GetFiles(directoryPath);
        var latestFile = files.Select(file => new FileInfo(file)).MaxBy(fileInfo => fileInfo.LastWriteTime);
        const string contentType = "application/octet-stream";
        var bytes = await System.IO.File.ReadAllBytesAsync(latestFile.FullName);
        return File(bytes, contentType, latestFile.Name);
    }

    [HttpGet(nameof(ChangeShardingContext)), AllowAnonymous]
    public void ChangeShardingContext([FromServices] IServiceProvider provider)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.TenantId, "08db5bf2-afae-4d40-8896-18e7e86b6b37" },
        });
        using var scope1 = provider.CreateScope();
        var dbContext1 = scope1.ServiceProvider.GetRequiredService<LiveBroadcastDbContext>();
        var bp1 = dbContext1.LiveMessages.ToQueryString();
        Console.WriteLine(bp1);

        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.TenantId, "08db5bf2-5b79-498b-8145-de0c5aae3271" },
        });
        using var scope2 = provider.CreateScope();
        var dbContext2 = scope2.ServiceProvider.GetRequiredService<LiveBroadcastDbContext>();
        var bp2 = dbContext2.LiveMessages.ToQueryString();
        Console.WriteLine(bp2);
    }

    [HttpGet(nameof(SendPhoneMessage)), AllowAnonymous]
    public Task SendPhoneMessage(string phone, string content)
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        return eventBus.PublishAsync(new SendMobileNotificationMessageEvent(
                                         Title: "直播预约提醒",
                                         EventNotificationMessageType: EventNotificationMessageType.Customize_1,
                                         IdCard: string.Empty,
                                         PhoneNumber: phone,
                                         Parameter: [content, $"{DateTime.Now.AddMinutes(5):yyyy-MM-dd HH:mm}",]
                                     ));
    }

    [HttpGet(nameof(SendWeChatMessage)), AllowAnonymous]
    public Task SendWeChatMessage(string openId, string content)
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        return eventBus.PublishAsync(new SendWechatNotificationMessageEvent(
                                         MessageType: EventNotificationMessageType.Customize_1,
                                         OpenId: openId,
                                         Parameter: new Dictionary<string, string>
                                         {
                                             { "thing6", new string(content.Take(20).ToArray()) },
                                             { "date7", $"{DateTime.Now:yyyy-MM-dd HH:mm}" },
                                         }
                                     ));
    }

    /// <summary>
    /// 测试按租户更新通知状态
    /// </summary>
    /// <returns></returns>
    [HttpGet("TestUpdateNotified"), AllowAnonymous]
    public Task TestUpdateNotified()
    {
        var tenantId = new Guid("08db5bf2-afae-4d40-8896-18e7e86b6b37");
        var repo = EngineContext.Current.Resolve<ILiveReservationRepository>();
        return repo.UpdateNotified(tenantId, [
            new("08dc5933-1572-49d2-8d2f-4877cf9d809c"),
            new("08dc5933-1c51-442a-8349-aeed97059a8b"),
            new("08dc5933-4edb-485e-8f98-cdb2fb02e966"),
            new("08dc5933-7b34-41ef-8f09-4a08e95264a6"),
            new("08dc5933-7ded-4aee-8ca0-2ac27542911c"),
            new("08dc5934-2dbb-41f1-8bec-18d32b285f31"),
            new("08dc5936-146f-4e11-8292-cb11d0c36d78"),
            new("08dc5937-b6ba-4b94-8f9b-04c4c677b64b"),
            new("08dc593b-6e69-4d63-8f46-ececaefeb6df"),
            new("08dc593e-8222-46c1-832a-a0a1dfa19abf"),
            new("08dc593f-64fa-404b-8de6-d5b81dbb68e9"),
            new("08dc593f-71a2-4339-874b-6277fa6cd414"),
        ]);
    }
}