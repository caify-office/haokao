using HaoKao.DeepSeekService.Application.ViewModels;
using HaoKao.DeepSeekService.Domain;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HaoKao.DeepSeekService.Application.Services;

public interface IDeepSeekWebService : IAppWebApiService, IManager
{
    /// <summary>
    /// 根据输入的上下文，来让模型补全对话内容
    /// </summary>
    /// <param name="viewModel">包含会话Id和用户输入的请求参数</param>
    /// <returns>SSE 流响应</returns>
    Task CompletionAsync(ChatViewModel viewModel);

    /// <summary>
    /// 重新生成上一次的回答
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    Task RegenerateAsync(Guid sessionId);

    /// <summary>
    /// 根据最近的对话内容，生成三个建议性的后续问题
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含三个建议问题字符串的列表</returns>
    Task<IReadOnlyList<string>> GenerateSuggestedQuestionsAsync(Guid sessionId);

    /// <summary>
    /// 根据会话Id获取聊天历史记录
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含历史消息的列表（不含系统提示）</returns>
    Task<IReadOnlyList<ChatMessage>> GetChatHistoryAsync(Guid sessionId);
}

[DynamicWebApi(Module = ServiceAreaName.WebSite)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DeepSeekWebService(
    IMapper mapper,
    IStaticCacheManager cacheManager,
    IDeepSeekConfigService service,
    ISseService sseService
) : IDeepSeekWebService
{
    /// <summary>
    /// 根据输入的上下文，来让模型补全对话内容
    /// </summary>
    [HttpPost]
    public async Task CompletionAsync([FromBody] ChatViewModel viewModel)
    {
        var stream = CompletionStreamAsync(viewModel);
        await sseService.StreamResponseAsync(stream);
    }

    /// <summary>
    /// 重新生成上一次的回答
    /// </summary>
    /// <param name="sessionId"></param>
    [HttpGet]
    public async Task RegenerateAsync(Guid sessionId)
    {
        var stream = RegenerateStreamAsync(sessionId);
        await sseService.StreamResponseAsync(stream);
    }

    /// <summary>
    /// 生成回答
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async IAsyncEnumerable<StreamChunk> CompletionStreamAsync(ChatViewModel viewModel, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var config = await service.Get();
        var cacheTime = (int)TimeSpan.FromDays(1).TotalMinutes;
        var prefix = GirvsEntityCacheDefaults<DeepSeekConfig>.ByTenantKey.Create().Key;
        var cacheKey = new CacheKey($"{prefix}:chat:{viewModel.ChatSessionId}", cacheTime);

        var messages = await cacheManager.GetAsync(cacheKey, () => Task.FromResult((List<ChatMessage>)[ChatMessage.FromSystem(config.SystemPrompt)]));
        messages.Add(ChatMessage.FromUser(viewModel.Prompt));

        // 调用通用的处理部分
        await foreach (var chunk in GenerateStreamAsync(messages, cacheKey, config, cancellationToken))
        {
            yield return chunk;
        }
    }

    /// <summary>
    /// 重新生成上一次的回答
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async IAsyncEnumerable<StreamChunk> RegenerateStreamAsync(Guid sessionId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var config = await service.Get();
        var prefix = GirvsEntityCacheDefaults<DeepSeekConfig>.ByTenantKey.Create().Key;
        var cacheKey = new CacheKey($"{prefix}:chat:{sessionId}");
        var messages = await cacheManager.GetAsync(cacheKey, () => Task.FromResult(new List<ChatMessage>(0)));

        if (messages == null || messages.Count == 0)
        {
            yield return new StreamChunk { Data = "No history found.", EventType = "error", IsEvent = true };
            yield break;
        }

        var lastAssistantMessageIndex = messages.FindLastIndex(m => m.Role == ChatRole.Assistant);
        if (lastAssistantMessageIndex == -1)
        {
            yield return new StreamChunk { Data = "No assistant message to regenerate.", EventType = "error", IsEvent = true };
            yield break;
        }
        messages.RemoveAt(lastAssistantMessageIndex);

        // 调用通用的处理部分
        await foreach (var chunk in GenerateStreamAsync(messages, cacheKey, config, cancellationToken))
        {
            yield return chunk;
        }
    }

    /// <summary>
    /// 生成回答
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="cacheKey"></param>
    /// <param name="config"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async IAsyncEnumerable<StreamChunk> GenerateStreamAsync(List<ChatMessage> messages, CacheKey cacheKey, DeepSeekConfig config, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var client = CreateClient(config.ApiKey);
        using var request = CreateRequest(messages.ToArray(), config);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        var builder = new StringBuilder();
        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrEmpty(line)) continue;

            var json = line.StartsWith("data: ") ? line["data: ".Length..] : line;
            if (json.Equals("[DONE]", StringComparison.OrdinalIgnoreCase)) break;

            var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekResponse>(json);
            var deltaContent = deepSeekResponse?.Choices?.FirstOrDefault()?.Delta?.Content;

            if (!string.IsNullOrEmpty(deltaContent))
            {
                builder.Append(deltaContent);
                yield return new StreamChunk { Data = deltaContent };
            }
        }

        if (builder.Length > 0)
        {
            messages.Add(ChatMessage.FromAssistant(builder.ToString()));
            await cacheManager.SetAsync(cacheKey, messages);
        }

        yield return new StreamChunk { Data = "completed", EventType = "finish", IsEvent = true };
    }

    /// <summary>
    /// 根据最近的对话内容，生成三个建议性的后续问题
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含三个建议问题字符串的列表</returns>
    [HttpGet]
    public async Task<IReadOnlyList<string>> GenerateSuggestedQuestionsAsync(Guid sessionId)
    {
        var messages = await GetChatHistoryAsync(sessionId);
        if (messages.Count == 0) return [];

        var lastUserMessage = messages.LastOrDefault(m => m.Role == ChatRole.User);
        var lastAssistantMessage = messages.LastOrDefault(m => m.Role == ChatRole.Assistant);
        if (lastUserMessage == null || lastAssistantMessage == null) return [];
        var historyConversation = JsonSerializer.Serialize(new[] { lastUserMessage, lastAssistantMessage });

        var requestMessages = new[]
        {
            ChatMessage.FromSystem("你是一个善于提出追问的AI助手。你的任务是根据用户的最后一次对话内容，生成三个用户可能感兴趣的、相关的追问问题。问题需要简洁、有探索性，并且与对话内容高度相关。请严格按照JSON数组的格式返回，例如：[\"问题一？\", \"问题二？\", \"问题三？\"]。不要包含任何额外的解释或文本，只返回JSON数组。"),
            ChatMessage.FromUser($"最新对话内容如下：\n\n{historyConversation}\n\n请根据以上内容生成三个相关问题。")
        };

        var config = await service.Get();
        config.Stream = false;
        config.StreamOption = null;
        config.Temperature = (decimal?)0.5; // 使用较低的温度以获得更稳定的、格式正确的结果

        using var client = CreateClient(config.ApiKey);
        using var request = CreateRequest(requestMessages, config);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();
        var generatedContent = responseBody?.Choices?.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(generatedContent)) return [];

        try
        {
            var cleanedJson = generatedContent.Trim().Trim('`', 'j', 's', 'o', 'n', '\n', '\r', ' ');
            var suggestedQuestions = JsonSerializer.Deserialize<IReadOnlyList<string>>(cleanedJson);
            return suggestedQuestions ?? [];
        }
        catch (JsonException)
        {
            // 如果JSON解析失败，返回空列表
            return [];
        }
    }

    /// <summary>
    /// 根据会话Id获取聊天历史记录
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含历史消息的列表（不含系统提示）</returns>
    [HttpGet]
    public async Task<IReadOnlyList<ChatMessage>> GetChatHistoryAsync(Guid sessionId)
    {
        var prefix = GirvsEntityCacheDefaults<DeepSeekConfig>.ByTenantKey.Create().Key;
        var cacheKey = new CacheKey($"{prefix}:chat:{sessionId}");
        var messages = await cacheManager.GetAsync(cacheKey, () => Task.FromResult(new List<ChatMessage>(0)));

        // 不向客户端返回系统提示词
        return messages.Where(m => m.Role != ChatRole.System).ToList();
    }

    private static HttpClient CreateClient(string apiKey)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        return client;
    }

    private HttpRequestMessage CreateRequest(ChatMessage[] messages, DeepSeekConfig config)
    {
        var content = mapper.Map<DeepSeekRequest>(config);
        content.Messages = messages;
        return new HttpRequestMessage(HttpMethod.Post, config.Endpoint)
        {
            Content = JsonContent.Create(content, options: new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            })
        };
    }
}