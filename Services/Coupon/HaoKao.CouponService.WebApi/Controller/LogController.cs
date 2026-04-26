using Girvs.EventBus;
using Girvs.Infrastructure;
using HaoKao.Common.Events.NotificationMessage;
using HaoKao.CouponService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HaoKao.CouponService.WebApi.Controller;

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

    [HttpGet(nameof(ManualMigration)), AllowAnonymous]
    public async Task ManualMigration()
    {
        var list = await EngineContext.Current.Resolve<IUserCouponRepository>().GetTenantIds();
        foreach (var id in list)
        {
            var httpClient = HttpClientFactory.Create();
            httpClient.DefaultRequestHeaders.Add("TenantId", id);
            await httpClient.GetAsync("http://localhost:5029/api/MigrationService/InitMigration/zhuofan%40168");
        }
    }

    [HttpGet(nameof(SendWeChatMessage)), AllowAnonymous]
    public Task SendWeChatMessage(string openId, string content)
    {
        var eventBus = EngineContext.Current.Resolve<IEventBus>();
        return eventBus.PublishAsync(new SendWechatNotificationMessageEvent(
                                         MessageType: EventNotificationMessageType.Customize_2,
                                         OpenId: openId,
                                         Parameter: new Dictionary<string, string>
                                         {
                                             { "thing1", "卡券即将到期，点击查看详情使用" },
                                             { "thing2", new string(content.Take(20).ToArray()) },
                                             { "time3", $"{DateTime.Now.AddHours(2):yyyy-MM-dd HH:mm}" },
                                         }
                                     ));
    }

}