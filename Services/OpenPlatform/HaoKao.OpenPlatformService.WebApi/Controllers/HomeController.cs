using Girvs.Driven.Bus;
using HaoKao.OpenPlatformService.Domain.Commands.RegisterUser;
using HaoKao.OpenPlatformService.Domain.Repositories;
using System.IO;

namespace HaoKao.OpenPlatformService.WebApi.Controllers;

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

    // [HttpGet(nameof(CreateUser)), AllowAnonymous]
    [NonAction]
    public async Task<dynamic> CreateUser([FromServices] IMediatorHandler bus)
    {
        var dict = new Dictionary<string, string>();
        var lines = await System.IO.File.ReadAllLinesAsync("/Users/arcaify/Downloads/users.txt");
        foreach (var line in lines)
        {
            var x = line.Split(" | ");
            dict.TryAdd(x[0], x[1]);
            var command = new CreateRegisterUserCommand(x[0], null, null, NickName: x[1]);
            await bus.SendCommand(command);
        }
        return dict;
    }

    // [HttpGet(nameof(UserIdForMigration))]
    [NonAction]
    public async Task<dynamic> UserIdForMigration([FromServices] IRegisterUserRepository repository)
    {
        var dict = new Dictionary<string, KeyValuePair<string, Guid>>();
        var lines = await System.IO.File.ReadAllLinesAsync("/Users/arcaify/Downloads/users.txt");
        var users = await repository.GetAllAsync();
        foreach (var line in lines)
        {
            var x = line.Split(" | ");
            var user = users.First(u => u.Account == x[0]);
            dict.TryAdd(user.Account, new KeyValuePair<string, Guid>(user.NickName, user.Id));
        }
        return dict;
    }
}