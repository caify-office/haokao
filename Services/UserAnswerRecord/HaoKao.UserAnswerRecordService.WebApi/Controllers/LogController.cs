using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaoKao.UserAnswerRecordService.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LogController : ControllerBase
{
    [HttpGet("Information"), AllowAnonymous]
    public Task<IActionResult> Information()
    {
        return DownloadLogFile("Information");
    }

    [HttpGet("Warning"), AllowAnonymous]
    public Task<IActionResult> Warning()
    {
        return DownloadLogFile("Warning");
    }

    [HttpGet("Error"), AllowAnonymous]
    public Task<IActionResult> Error()
    {
        return DownloadLogFile("Error");
    }

    private async Task<IActionResult> DownloadLogFile(string type)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs", type);
        if (!Directory.Exists(directoryPath)) return NotFound(directoryPath);
        var files = Directory.GetFiles(directoryPath);
        var latestFile = files.Select(file => new FileInfo(file)).MaxBy(fileInfo => fileInfo.LastWriteTime);
        const string contentType = "application/octet-stream";
        var bytes = await System.IO.File.ReadAllBytesAsync(latestFile.FullName);
        return File(bytes, contentType, latestFile.Name);
    }
}