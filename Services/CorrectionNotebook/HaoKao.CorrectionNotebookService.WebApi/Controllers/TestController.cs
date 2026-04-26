using Girvs.Cache.Caching;
using Girvs.Driven.Bus;
using Girvs.Infrastructure;
using HaoKao.CorrectionNotebookService.Domain.Commands;
using HaoKao.CorrectionNotebookService.Domain.Repositories;
using HaoKao.CorrectionNotebookService.Infrastructure;
using HaoKao.CorrectionNotebookService.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaoKao.CorrectionNotebookService.WebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public Task<string> DeepSeekLLM(string content = "你好")
    {
        var instance = new DeepSeekLLM();
        return instance.CompletionAsync(content);
    }

    [HttpGet]
    public Task<string> QwenLLM(string content = "你好")
    {
        var instance = new QwenLLM();
        return instance.CompletionAsync(content);
    }

    [HttpGet]
    public async Task QwenLLMStream(string content = "你好")
    {
        var instance = new QwenLLM();
        await foreach (var chunk in instance.CompletionStreamAsync(content))
        {
            await HttpContext.Response.WriteAsync($"{chunk}\n");
        }
    }

    [HttpGet]
    public Task<string> QianFanLLM(string content = "你好")
    {
        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        var instance = new QianFanLLM(cacheManager);
        return instance.CompletionAsync(content);
    }

    [HttpGet]
    public async Task QianFanLLMStream(string content = "你好")
    {
        var cacheManager = EngineContext.Current.Resolve<IStaticCacheManager>();
        var instance = new QianFanLLM(cacheManager);
        await foreach (var chunk in instance.CompletionStreamAsync(content))
        {
            await HttpContext.Response.WriteAsync($"{chunk}\n");
        }
    }

    [HttpGet]
    public async Task<string> QianFanApp()
    {
        var url = new Uri("https://haokao-dev.oss-cn-shenzhen.aliyuncs.com/wx172230874880883.png");
        var path = await DownloadImage(url);

        var instance = new QianFanAppBuilder();
        var result = await instance.CompletionAsync(path);
        System.IO.File.Delete(path);

        return result;
    }

    [HttpGet]
    public async Task QianFanAppStream()
    {
        var url = new Uri("https://haokao-dev.oss-cn-shenzhen.aliyuncs.com/wx172230874880883.png");
        var path = await DownloadImage(url);

        HttpContext.Response.ContentType = "text/event-stream";
        var instance = new QianFanAppBuilder();
        await foreach (var chunk in instance.CompletionStreamAsync(path))
        {
            await HttpContext.Response.WriteAsync($"{chunk}\n");
        }

        System.IO.File.Delete(path);
    }

    [HttpGet]
    public async Task SaveAnswerAndAnalysis()
    {
        var repository = EngineContext.Current.Resolve<IQuestionRepository>();
        var question = await repository.GetByIdAsync(Guid.Parse("2dfe816b-bb69-44d0-b137-dcb6370ba196"));
        await question.GenerateAnswerAndAnalysis(new QwenLLM(), "你好");
        var command = new SaveAnswerAndAnalysisCommand(question);
        var bus = EngineContext.Current.Resolve<IMediatorHandler>();
        await bus.SendCommand(command);
    }

    private static async Task<string> DownloadImage(Uri uri)
    {
        var filename = Path.GetFileName(uri.LocalPath);
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

        using var client = new HttpClient();
        using var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = System.IO.File.Create(path);
        await stream.CopyToAsync(fileStream);

        return path;
    }

    private static async Task<byte[]> GetImageDataAsync(string imagePath)
    {
        await using var fs = new FileStream(imagePath, FileMode.Open);
        var buffer = new byte[fs.Length];
        await fs.ReadAsync(buffer);
        return buffer;
    }

    [HttpGet]
    public async Task<IActionResult> ReferenceLoop([FromServices] CorrectionNotebookDbContext context)
    {
        var question = await context.Questions.Include(x => x.Tags)
                                              .ThenInclude(x => x.Tag)
                                              .Where(x => x.Id == Guid.Parse("1b0b860a-3e1b-429b-9e79-db8ffbf72141"))
                                              .FirstOrDefaultAsync();
        return Ok(question);
    }
}