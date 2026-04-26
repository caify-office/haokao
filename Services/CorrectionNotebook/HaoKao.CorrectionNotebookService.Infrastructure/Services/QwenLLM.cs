using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record QwenLLMOptions : IAppModuleConfig
{
    public string ApiKey { get; init; } = "sk-bcb0200b4f3948c6887e9a6f7f075544";

    public string ModelId { get; init; } = "qwen2-57b-a14b-instruct";

    public string ModelName { get; init; } = "通义千问2-开源版-57B";

    public Uri Endpoint { get; init; } = new("https://dashscope.aliyuncs.com/compatible-mode/v1/chat/completions");

    public void Init() { }
}

/// <summary>
/// 通义千问大模型
/// <see cref="https://bailian.console.aliyun.com/"/>
/// </summary>
public class QwenLLM : ILargeLanguageModel
{
    private readonly QwenLLMOptions _options = Singleton<AppSettings>.Instance.Get<QwenLLMOptions>();

    public Guid Id => new("4dc0092d-dd20-48b1-ad8f-c54054aeed2d");

    public string Name => _options.ModelName;

    public async Task<string> CompletionAsync(string content)
    {
        using var client = CreateClient();
        using var request = CreateRequest(content, false);
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            var result = await response.Content.ReadAsAsync<QwenResponse>();
            return result.Choices[0].Message.Content;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    public async IAsyncEnumerable<string> CompletionStreamAsync(string content)
    {
        using var client = CreateClient();
        using var request = CreateRequest(content, true);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        // 返回流式响应
        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            yield return line;
        }
    }

    public string ReadStream(IReadOnlyList<string> chunks)
    {
        throw new NotImplementedException();
    }

    private HttpRequestMessage CreateRequest(string content, bool stream)
    {
        return new(HttpMethod.Post, _options.Endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(new QwenRequestBody
            {
                Model = _options.ModelId,
                Messages = [new QwenMessage { Role = "user", Content = content }],
                Stream = stream
            }), Encoding.UTF8, "application/json")
        };
    }

    private HttpClient CreateClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", _options.ApiKey);
        return client;
    }
}

file record QwenRequestBody
{
    [JsonPropertyName("model")]
    public string Model { get; init; }

    [JsonPropertyName("messages")]
    public QwenMessage[] Messages { get; init; }

    [JsonPropertyName("stream")]
    public bool Stream { get; init; } = false;
}

file record QwenMessage
{
    [JsonPropertyName("role")]
    public string Role { get; init; }

    [JsonPropertyName("content")]
    public string Content { get; init; }
}

file record QwenResponse
{
    [JsonPropertyName("choices")]
    public QwenResponseChoice[] Choices { get; init; }
}

file record QwenResponseChoice
{
    [JsonPropertyName("message")]
    public QwenMessage Message { get; init; }
}