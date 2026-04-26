using Girvs.Driven.Bus;
using Girvs.Infrastructure;
using HaoKao.ProductService.Domain.Commands.StudentPermission;
using HaoKao.ProductService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.ProductService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
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

    // [HttpGet(nameof(MigrateUserPermission)), AllowAnonymous]
    [NonAction]
    public async Task<dynamic> MigrateUserPermission([FromServices] IMediatorHandler bus, [FromServices] IProductRepository repository)
    {
        EngineContext.Current.ClaimManager.SetFromDictionary(new Dictionary<string, string>
        {
            { GirvsIdentityClaimTypes.TenantId, "08db4161-8d38-4f30-8b44-f855967c7f27" },
            // { GirvsIdentityClaimTypes.TenantId, "08db449e-4161-442a-8887-cb626dbb7682" },
            // { GirvsIdentityClaimTypes.TenantId, "08db449e-307f-410b-890b-bf62eb9f5f0c" },
        });

        var json = await System.IO.File.ReadAllTextAsync("/Users/arcaify/Downloads/users.json");
        var users = JsonConvert.DeserializeObject<Dictionary<string, KeyValuePair<string, Guid>>>(json);

        var lines = await System.IO.File.ReadAllLinesAsync("/Users/arcaify/Downloads/user_product.txt");
        var products = await repository.GetWhereAsync(x => true);
        var dict = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            var x = line.Split(" | ");
            var product = products.FirstOrDefault(p => p.Name == x[1]);
            if (product != null)
            {
                dict.TryAdd(x[0], product.Name);
                var user = users[x[0]];
                var command = new CreateStudentPermissionCommand(
                    user.Key,
                    user.Value,
                    "",
                    product.Id,
                    product.Name,
                    product.ExpiryTime
                );
                await bus.SendCommand(command);
            }
        }
        return dict;
    }
}