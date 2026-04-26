using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record QianFanAppBuilderOptions : IAppModuleConfig
{
    public string AppId { get; init; } = "0bea3acb-46a4-47ce-9a19-4a0d90e2710f";

    public string AppSecret { get; init; } = "bce-v3/ALTAK-6cglzMHlHbmyylLuHEMQn/c738c5317ada4390aa2047f4287888cc3f0ad577";

    public Uri Endpoint { get; init; } = new("https://qianfan.baidubce.com");

    public void Init() { }
}

/// <summary>
/// 千帆AppBuilder服务
/// <see cref="https://appbuilder.cloud.baidu.com/"/>
/// </summary>
public class QianFanAppBuilder : ILargeLanguageModel
{
    public Guid Id => new("4d2324c6-83f8-4794-a95c-c27ae57b0da6");

    public string Name => "千帆AppBuilder";

    private readonly QianFanAppBuilderOptions _options = Singleton<AppSettings>.Instance.Get<QianFanAppBuilderOptions>();

    public async Task<string> CompletionAsync(string content)
    {
        // 千帆大模型(文心一言)通过图片提问

        // 1. 创建一个会话
        var conversationId = await CreateConversation();

        // 2. 上传图片
        var fileId = await UploadImage(conversationId, content);

        // 3. 提问
        return await GetAnswer(conversationId, fileId);
    }

    public async IAsyncEnumerable<string> CompletionStreamAsync(string content)
    {
        // 千帆大模型(文心一言)通过图片提问

        // 1. 创建一个会话
        var conversationId = await CreateConversation();

        // 2. 上传图片
        var fileId = await UploadImage(conversationId, content);

        // 3. 提问
        await foreach (var answer in GetAnswerStream(conversationId, fileId))
        {
            yield return answer;
        }
    }

    public string ReadStream(IReadOnlyList<string> chunks)
    {
        var list = chunks.Where(c => !string.IsNullOrEmpty(c))
        .Select(chunk => JsonSerializer.Deserialize<ConversationStreamResult>(chunk.Replace("data: ", ""))).ToList();
        return string.Join("", list.Where(x => !string.IsNullOrEmpty(x.Answer)).Select(x => x.Answer).ToList());
    }

    private HttpClient CreateClient()
    {
        var client = HttpClientFactory.Create();
        client.DefaultRequestHeaders.Add("X-Appbuilder-Authorization", $"Bearer {_options.AppSecret}");
        return client;
    }

    private async Task<string> CreateConversation()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.Endpoint}v2/app/conversation")
        {
            Content = new StringContent(JsonSerializer.Serialize(new CreateConversationRequest
            {
                AppId = _options.AppId
            }), Encoding.UTF8, "application/json")
        };
        using var client = CreateClient();
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<CreateConversationResult>(stream);
            return result.ConversationId;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    private async Task<string> UploadImage(string conversationId, string path)
    {
        var url = $"{_options.Endpoint}v2/app/conversation/file/upload";
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new MultipartFormDataContent
            {
                { new StringContent(_options.AppId), "app_id" },
                { new StringContent(conversationId), "conversation_id" },
                {
                    new StreamContent(File.OpenRead(path))
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("application/octet-stream") }
                    },
                    "file",
                    Path.GetFileName(path)
                }
            }
        };
        using var client = CreateClient();
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<UploadFileResult>(stream);
            return result.Id;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    private async Task<string> GetAnswer(string conversationId, string fileId)
    {
        using var client = CreateClient();
        using var request = CreateAnswerRequest(conversationId, fileId, false);
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<ConversationResult>(stream);
            return result.Answer;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    private async IAsyncEnumerable<string> GetAnswerStream(string conversationId, string fileId)
    {
        using var client = CreateClient();
        using var request = CreateAnswerRequest(conversationId, fileId, true);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        // 确保请求成功
        response.EnsureSuccessStatusCode();

        // 返回流式响应
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            yield return line;
            // var json = line.Replace("data: ", "");
            // if (string.IsNullOrWhiteSpace(json)) continue;
            // var result = JsonSerializer.Deserialize<ConversationStreamResult>(json);
            // yield return result.Answer;
        }
    }

    private HttpRequestMessage CreateAnswerRequest(string conversationId, string fileId, bool stream)
    {
        return new(HttpMethod.Post, $"{_options.Endpoint}v2/app/conversation/runs")
        {
            Content = new StringContent(JsonSerializer.Serialize(new ConversationRequest
            {
                AppId = _options.AppId,
                ConversationId = conversationId,
                Stream = stream,
                FileIds = [fileId],
                Query = "请作答",
            }), Encoding.UTF8, "application/json")
        };
    }
}

#region File Class

file record CreateConversationRequest
{
    [JsonPropertyName("app_id")]
    public string AppId { get; init; }
};

file record CreateConversationResult
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; init; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; init; }
}

file record UploadFileResult
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; init; }

    [JsonPropertyName("request_id")]
    public string RequestId { get; init; }
}

file record ConversationRequest
{
    [JsonPropertyName("app_id")]
    public string AppId { get; init; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; init; }

    [JsonPropertyName("stream")]
    public bool Stream { get; init; }

    [JsonPropertyName("file_ids")]
    public string[] FileIds { get; init; }

    [MaxLength(2000)]
    [JsonPropertyName("query")]
    public string Query { get; init; }
}

file record ConversationResult
{
    [JsonPropertyName("app_id")]
    public string AppId { get; init; }

    [JsonPropertyName("answer")]
    public string Answer { get; init; }
}

file record ConversationStreamResult
{
    [JsonPropertyName("request_id")]
    public string RequestId { get; init; }

    [JsonPropertyName("date")]
    public string Date { get; init; }

    [JsonPropertyName("answer")]
    public string Answer { get; init; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; init; }

    [JsonPropertyName("message_id")]
    public string MessageId { get; init; }

    [JsonPropertyName("is_completion")]
    public bool IsCompletion { get; init; }

    [JsonPropertyName("content")]
    public ConversationContent[] Content { get; init; }
}

file record ConversationContent
{
    [JsonPropertyName("result_type")]
    public string ResultType { get; init; }

    [JsonPropertyName("event_code")]
    public int EventCode { get; init; }

    [JsonPropertyName("event_message")]
    public string EventMessage { get; init; }

    [JsonPropertyName("event_type")]
    public string EventType { get; init; }

    [JsonPropertyName("event_id")]
    public string EventId { get; init; }

    [JsonPropertyName("event_status")]
    public string EventStatus { get; init; }

    [JsonPropertyName("content_type")]
    public string ContentType { get; init; }

    [JsonPropertyName("visible_scope")]
    public string VisibleScope { get; init; }

    [JsonPropertyName("outputs")]
    public ConversationOutput Outputs { get; init; }

    [JsonPropertyName("usage")]
    public ConversationUsage Usage { get; init; }
}

file record ConversationOutput
{
    [JsonPropertyName("text")]
    public dynamic Text { get; init; }
}

file record ConversationUsage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
}

#endregion