using System.Threading;

namespace HaoKao.DeepSeekService.Application.Services;
/// <summary>
/// 提供处理 Server-Sent Events (SSE) 响应流的功能
/// </summary>
public interface ISseService : IManager
{
    /// <summary>
    /// 将一个异步流作为 Server-Sent Events 进行流式传输
    /// </summary>
    /// <param name="stream">要发送的数据块异步流</param>
    Task StreamResponseAsync(IAsyncEnumerable<StreamChunk> stream);
}

public class SseService(IHttpContextAccessor httpContextAccessor) : ISseService
{
    public async Task StreamResponseAsync(IAsyncEnumerable<StreamChunk> stream)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            // 在没有HttpContext的上下文中无法执行
            return;
        }

        var response = context.Response;
        SetupSseHeaders(response);

        try
        {
            // 使用 WithCancellation 将 HttpContext.RequestAborted 传递给异步流
            await foreach (var chunk in stream.WithCancellation(context.RequestAborted))
            {
                // 再次检查请求是否已取消
                if (context.RequestAborted.IsCancellationRequested) break;

                if (chunk.IsEvent && !string.IsNullOrEmpty(chunk.EventType))
                {
                    await WriteSseEventAsync(response, chunk.EventType, chunk.Data, context.RequestAborted);
                }
                else
                {
                    await WriteSseDataAsync(response, chunk.Data, context.RequestAborted);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // 客户端断开连接，这是正常情况，无需处理
        }
        catch (Exception ex)
        {
            // 如果发生其他异常，记录日志并通过SSE发送错误事件
            // 注意：如果响应头已发送，则无法再更改状态码
            if (!response.HasStarted)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            await WriteSseEventAsync(response, "error", ex.Message, CancellationToken.None);
        }
    }

    /// <summary>
    /// 设置 SSE 响应头
    /// </summary>
    private static void SetupSseHeaders(HttpResponse response)
    {
        response.ContentType = "text/event-stream; charset=utf-8";
        response.Headers.CacheControl = "no-cache";
        response.Headers.Connection = "keep-alive";
    }

    /// <summary>
    /// 写入 SSE 'message' 事件
    /// </summary>
    private static async Task WriteSseDataAsync(HttpResponse response, string data, CancellationToken cancellationToken)
    {
        var sseData = $"event: message\ndata: {{\"v\":\"{EscapeJson(data)}\"}}\n\n";
        await response.WriteAsync(sseData, cancellationToken);
        await response.Body.FlushAsync(cancellationToken); // 确保数据立即发送
    }

    /// <summary>
    /// 写入 SSE 自定义事件
    /// </summary>
    private static async Task WriteSseEventAsync(HttpResponse response, string eventType, string data, CancellationToken cancellationToken)
    {
        var sseEvent = $"event: {eventType}\ndata: {EscapeJson(data)}\n\n";
        await response.WriteAsync(sseEvent, cancellationToken);
        await response.Body.FlushAsync(cancellationToken); // 确保数据立即发送
    }

    /// <summary>
    /// 转义 JSON 字符串中的特殊字符
    /// </summary>
    private static string EscapeJson(string input)
    {
        return input.Replace("\\", @"\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
    }
}

/// <summary>
/// 用于在SSE流中传输的数据块
/// </summary>
public record StreamChunk
{
    /// <summary>
    /// 数据内容
    /// </summary>
    public string Data { get; init; } = string.Empty;

    /// <summary>
    /// 是否为自定义事件 (如果为 false, 则默认为 'message' 事件)
    /// </summary>
    public bool IsEvent { get; init; }

    /// <summary>
    /// 自定义事件的类型 (仅当 IsEvent 为 true 时有效)
    /// </summary>
    public string EventType { get; init; }
}

