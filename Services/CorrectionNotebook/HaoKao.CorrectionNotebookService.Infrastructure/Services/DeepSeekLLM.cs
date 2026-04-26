using HaoKao.CorrectionNotebookService.Domain.Services;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record DeepSeekLLMOptions : IAppModuleConfig
{
    public string ApiKey { get; init; } = "sk-1058bdb03323443686242de19de17b80";

    public string ModelName { get; init; } = "deepseek-chat";

    public Uri Endpoint { get; init; } = new("https://api.deepseek.com/chat/completions");

    public void Init() { }
}

public class DeepSeekLLM // : ILargeLanguageModel
{
    public string Name => "DeepSeek";

    public Guid Id => Guid.Parse("067a46bd-8366-7ab9-8000-86f20f417e0f");

    private readonly DeepSeekLLMOptions _options = Singleton<AppSettings>.Instance.Get<DeepSeekLLMOptions>();

    public async Task<string> CompletionAsync(ChatMessage[] messages)
    {
        // messages 不能为空
        if (messages == null || messages.Length == 0)
        {
            throw new ArgumentException("messages 不能为空");
        }

        // 第一次对话加入提示词
        if (messages.Length == 1)
        {
            messages = [
                new ChatMessage
                {
                    Role = ChatRole.System,
                    Content = "你是一个助手，请回答问题"
                },
                messages[0]
            ];
        }

        using var client = CreateClient();
        using var request = CreateRequest(messages);
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            var result = await response.Content.ReadAsAsync<DeepSeekResponse>();
            return result.Choices[0].Message.Content;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    public IAsyncEnumerable<string> CompletionStreamAsync(ChatMessage[] messages)
    {
        throw new NotImplementedException();
    }

    public string ReadStream(IReadOnlyList<string> chunks)
    {
        throw new NotImplementedException();
    }

    private HttpClient CreateClient()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", _options.ApiKey);
        return client;
    }

    private HttpRequestMessage CreateRequest(ChatMessage[] messages)
    {
        return new(HttpMethod.Post, _options.Endpoint)
        {
            Content = new StringContent(JsonSerializer.Serialize(new RequestBody
            {
                Model = ChatModel.Chat,
                Messages = messages,
            }), Encoding.UTF8, "application/json")
        };
    }
}

public class DeepSeekConfig
{
    /// <summary>
    /// Api密钥
    /// </summary>
    public string ApiKey { get; init; } = "sk-1058bdb03323443686242de19de17b80";

    /// <summary>
    /// 接口地址
    /// </summary>
    public Uri Endpoint { get; init; } = new("https://api.deepseek.com/chat/completions");

    /// <summary>
    /// Possible values: [deepseek-chat, deepseek-reasoner]
    /// 使用的模型的 ID。您可以使用 deepseek-chat。
    /// </summary>
    public string Model { get; init; }

    /// <summary>
    /// Possible values: >= -2 and <= 2
    /// Default value: 0
    /// 介于 -2.0 和 2.0 之间的数字。
    /// 如果该值为正，那么新 token 会根据其在已有文本中的出现频率受到相应的惩罚，降低模型重复相同内容的可能性。
    /// </summary>
    public decimal? FrequencyPenalty { get; init; } = 0;

    /// <summary>
    /// Possible values: > 1
    /// 介于 1 到 8192 间的整数，限制一次请求中模型生成 completion 的最大 token 数。
    /// 输入 token 和输出 token 的总长度受模型的上下文长度的限制。
    /// 如未指定 max_tokens参数，默认使用 4096。
    /// </summary>
    public int MaxTokens { get; init; } = 4096;

    /// <summary>
    /// Possible values: >= -2 and <= 2
    /// Default value: 0
    /// 介于 -2.0 和 2.0 之间的数字。
    /// 如果该值为正，那么新 token 会根据其是否已在已有文本中出现受到相应的惩罚，从而增加模型谈论新主题的可能性。
    /// </summary>
    public decimal? PresencePenalty { get; init; } = 0;

    /// <summary>
    /// 一个 object，指定模型必须输出的格式。
    /// 设置为 { "type": "json_object" } 以启用 JSON 模式，该模式保证模型生成的消息是有效的 JSON。
    /// 注意: 使用 JSON 模式时，你还必须通过系统或用户消息指示模型生成 JSON。
    /// 否则，模型可能会生成不断的空白字符，直到生成达到令牌限制，从而导致请求长时间运行并显得“卡住”。
    /// 此外，如果 finish_reason="length"，这表示生成超过了 max_tokens 或对话超过了最大上下文长度，消息内容可能会被部
    /// </summary>
    public ResponseFormat ResponseFormat { get; init; } = new();

    /// <summary>
    /// 一个 string 或最多包含 16 个 string 的 list，在遇到这些词时，API 将停止生成更多的 token。
    /// </summary>
    public string[] Stop { get; init; } = null;

    /// <summary>
    /// 如果设置为 True，将会以 SSE（server-sent events）的形式以流式发送消息增量。
    /// 消息流以 data: [DONE] 结尾。
    /// </summary>
    public bool? Stream { get; init; } = false;

    /// <summary>
    /// 流式输出相关选项。只有在 stream 参数为 true 时，才可设置此参数。
    /// </summary>
    public StreamOption StreamOption { get; init; } = new();

    /// <summary>
    /// Possible values: <= 2
    /// Default value: 1
    /// 采样温度，介于 0 和 2 之间。
    /// 更高的值，如 0.8，会使输出更随机，而更低的值，如 0.2，会使其更加集中和确定。
    /// 我们通常建议可以更改这个值或者更改 top_p，但不建议同时对两者进行修改。
    /// </summary>
    public decimal? Temperature { get; init; } = 1;

    /// <summary>
    /// Possible values: <= 1
    /// Default value: 1
    /// 作为调节采样温度的替代方案，模型会考虑前 top_p 概率的 token 的结果。
    /// 所以 0.1 就意味着只有包括在最高 10% 概率中的 token 会被考虑。
    /// 我们通常建议修改这个值或者更改 temperature，但不建议同时对两者进行修改。
    /// </summary>
    public decimal? TopP { get; init; } = 1;

    /// <summary>
    /// 是否返回所输出 token 的对数概率。
    /// 如果为 true，则在 message 的 content 中返回每个输出 token 的对数概率。
    /// </summary>
    public bool? Logprobs { get; init; }

    /// <summary>
    /// Possible values: <= 20
    /// 一个介于 0 到 20 之间的整数 N，指定每个输出位置返回输出概率 top N 的 token，且返回这些 token 的对数概率。
    /// 指定此参数时，logprobs 必须为 true。
    /// </summary>
    public int? TopLogprobs { get; init; }
}


public static class ChatModel
{
    public const string Chat = "deepseek-chat";

    public const string Reasoner = "deepseek-reasoner";
}

public static class ChatRole
{
    public const string System = "system";

    public const string User = "user";

    public const string Assistant = "assistant";
}

public record RequestBody
{
    /// <summary>
    /// Possible values: [deepseek-chat, deepseek-reasoner]
    /// 使用的模型的 ID。您可以使用 deepseek-chat。
    /// </summary>
    [JsonPropertyName("model")]
    public string Model
    {
        get => _model;
        init
        {
            if (value != "deepseek-chat" && value != "deepseek-reasoner")
            {
                throw new ArgumentException("Invalid model value. The value must be one of 'deepseek-chat' or 'deepseek-reasoner'.");
            }
            _model = value;
        }
    }
    private string _model;

    /// <summary>
    /// 对话的消息列表。
    /// </summary>
    [JsonPropertyName("messages")]
    public ChatMessage[] Messages { get; init; }

    /// <summary>
    /// Possible values: >= -2 and <= 2
    /// Default value: 0
    /// 介于 -2.0 和 2.0 之间的数字。
    /// 如果该值为正，那么新 token 会根据其在已有文本中的出现频率受到相应的惩罚，降低模型重复相同内容的可能性。
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public decimal? FrequencyPenalty
    {
        get => _frequencyPenalty;
        init
        {
            if (value < -2.0m || value > 2.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(FrequencyPenalty), "The value must be between -2.0 and 2.0.");
            }
            _frequencyPenalty = value;
        }
    }
    private decimal? _frequencyPenalty = 0;

    /// <summary>
    /// Possible values: > 1
    /// 介于 1 到 8192 间的整数，限制一次请求中模型生成 completion 的最大 token 数。
    /// 输入 token 和输出 token 的总长度受模型的上下文长度的限制。
    /// 如未指定 max_tokens参数，默认使用 4096。
    /// </summary>
    [JsonPropertyName("max_tokens")]
    public int MaxTokens
    {
        get => _maxTokens;
        init
        {
            if (value < 1 || value > 8192)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxTokens), "The value must be between 1 and 8192.");
            }

            _maxTokens = value;
        }
    }
    private int _maxTokens = 4096;

    /// <summary>
    /// Possible values: >= -2 and <= 2
    /// Default value: 0
    /// 介于 -2.0 和 2.0 之间的数字。
    /// 如果该值为正，那么新 token 会根据其是否已在已有文本中出现受到相应的惩罚，从而增加模型谈论新主题的可能性。
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public decimal? PresencePenalty
    {
        get => _presencePenalty;
        init
        {
            if (value < -2 || value > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(PresencePenalty), "The value must be between -2 and 2.");
            }
            _presencePenalty = value;
        }
    }
    private decimal? _presencePenalty = 0;

    /// <summary>
    /// 一个 object，指定模型必须输出的格式。
    /// 设置为 { "type": "json_object" } 以启用 JSON 模式，该模式保证模型生成的消息是有效的 JSON。
    /// 注意: 使用 JSON 模式时，你还必须通过系统或用户消息指示模型生成 JSON。
    /// 否则，模型可能会生成不断的空白字符，直到生成达到令牌限制，从而导致请求长时间运行并显得“卡住”。
    /// 此外，如果 finish_reason="length"，这表示生成超过了 max_tokens 或对话超过了最大上下文长度，消息内容可能会被部
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormat ResponseFormat { get; init; } = new();

    /// <summary>
    /// 一个 string 或最多包含 16 个 string 的 list，在遇到这些词时，API 将停止生成更多的 token。
    /// </summary>
    [JsonPropertyName("stop")]
    public string[] Stop
    {
        get => _stop;
        init
        {
            if (value is not null && value.Length > 16)
            {
                throw new ArgumentException("The stop array cannot contain more than 16 elements.");
            }
            _stop = value;
        }
    }
    private string[] _stop;

    /// <summary>
    /// 如果设置为 True，将会以 SSE（server-sent events）的形式以流式发送消息增量。
    /// 消息流以 data: [DONE] 结尾。
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; init; } = false;

    /// <summary>
    /// 流式输出相关选项。只有在 stream 参数为 true 时，才可设置此参数。
    /// </summary>
    [JsonPropertyName("stream_option")]
    public StreamOption StreamOption { get; init; }

    /// <summary>
    /// Possible values: <= 2
    /// Default value: 1
    /// 采样温度，介于 0 和 2 之间。
    /// 更高的值，如 0.8，会使输出更随机，而更低的值，如 0.2，会使其更加集中和确定。
    /// 我们通常建议可以更改这个值或者更改 top_p，但不建议同时对两者进行修改。
    /// </summary>
    [JsonPropertyName("temperature")]
    public decimal? Temperature
    {
        get => _temperature;
        init
        {
            if (value < 0 || value > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(Temperature), "The value must be between 0 and 2.");
            }
            _temperature = value;
        }
    }
    private decimal? _temperature = 1;

    /// <summary>
    /// Possible values: <= 1
    /// Default value: 1
    /// 作为调节采样温度的替代方案，模型会考虑前 top_p 概率的 token 的结果。
    /// 所以 0.1 就意味着只有包括在最高 10% 概率中的 token 会被考虑。
    /// 我们通常建议修改这个值或者更改 temperature，但不建议同时对两者进行修改。
    /// </summary>
    [JsonPropertyName("top_p")]
    public decimal? TopP
    {
        get => _topP;
        init
        {
            if (value < 0 || value > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(TopP), "The value must be between 0 and 1.");
            }
            _topP = value;
        }
    }
    private decimal? _topP = 1;

    /// <summary>
    /// 是否返回所输出 token 的对数概率。
    /// 如果为 true，则在 message 的 content 中返回每个输出 token 的对数概率。
    /// </summary>
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; init; }

    /// <summary>
    /// Possible values: <= 20
    /// 一个介于 0 到 20 之间的整数 N，指定每个输出位置返回输出概率 top N 的 token，且返回这些 token 的对数概率。
    /// 指定此参数时，logprobs 必须为 true。
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs
    {
        get => _topLogprobs;
        init
        {
            if (value > 20 || value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TopLogprobs), "The value must be between 0 and 20.");
            }
            _topLogprobs = value;
        }
    }
    private int? _topLogprobs;
}

public record ResponseFormat
{
    /// <summary>
    /// Possible values: [text, json_object]
    /// Default value: text
    /// Must be one of text or json_object.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type
    {
        get => _type;
        init
        {
            if (value != "text" && value != "json_object")
            {
                throw new ArgumentException("Invalid Type value. Type must be one of 'text' or 'json_object'.");
            }
            _type = value;
        }
    }

    private string _type = "text";
}

public record StreamOption
{
    /// <summary>
    /// 如果设置为 true，在流式消息最后的 data: [DONE] 之前将会传输一个额外的块。
    /// 此块上的 usage 字段显示整个请求的 token 使用统计信息，而 choices 字段将始终是一个空数组。
    /// 所有其他块也将包含一个 usage 字段，但其值为 null。
    /// </summary>
    [JsonPropertyName("include_usage")]
    public bool IncludeUseage { get; init; } = true;
}

public record ChatMessage
{
    /// <summary>
    /// 该消息的发起角色，其值为user 或 system 或 assistant。
    /// </summary>
    [JsonPropertyName("role")]
    public string Role
    {
        get => _role;
        init
        {
            if (value != "user" && value != "system" && value != "assistant")
            {
                throw new ArgumentException("Invalid Role value. Role must be one of 'user', 'system' or 'assistant'.");
            }
            _role = value;
        }
    }
    private string _role;

    /// <summary>
    /// 消息的内容。
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; init; }

    /// <summary>
    /// 可以选填的参与者的名称，为模型提供信息以区分相同角色的参与者。
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }
}

public record DeepSeekResponse
{
    /// <summary>
    /// 该对话的唯一标识符。
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// 模型生成的 completion 的选择列表。
    /// </summary>
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; init; }

    /// <summary>
    /// 创建聊天完成时的 Unix 时间戳（以秒为单位）。
    /// </summary>
    [JsonPropertyName("created")]
    public long Created { get; init; }

    /// <summary>
    /// 生成该 completion 的模型名。
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; init; }

    /// <summary>
    /// This fingerprint represents the backend configuration that the model runs with.
    /// </summary>
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; init; }

    /// <summary>
    /// Possible values: [chat.completion]
    /// 对象的类型, 其值为 chat.completion。
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; init; }

    /// <summary>
    /// 该对话补全请求的用量信息。
    /// </summary>
    [JsonPropertyName("usage")]
    public Usage Usage { get; init; }
}

public record Choice
{
    /// <summary>
    /// Possible values: [stop, length, content_filter, tool_calls, insufficient_system_resource]
    /// 模型停止生成 token 的原因。
    /// stop：模型自然停止生成，或遇到 stop 序列中列出的字符串。
    /// length ：输出长度达到了模型上下文长度限制，或达到了 max_tokens 的限制。
    /// content_filter：输出内容因触发过滤策略而被过滤。
    /// insufficient_system_resource：系统推理资源不足，生成被打断。
    /// </summary>
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; init; }

    /// <summary>
    /// 该 completion 在模型生成的 completion 的选择列表中的索引。
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; init; }

    /// <summary>
    /// 模型生成的 completion 消息。
    /// </summary>
    [JsonPropertyName("message")]
    public Message Message { get; init; }

    /// <summary>
    /// 流式返回的一个 completion 增量。
    /// </summary>
    [JsonPropertyName("delta")]
    public Message Delta { get; init; }

    /// <summary>
    /// 该 choice 的对数概率信息。
    /// </summary>
    [JsonPropertyName("logprobs")]
    public Logprobs Logprobs { get; init; }
}

public record Message
{
    /// <summary>
    /// Possible values: [assistant]
    /// 生成这条消息的角色。
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; init; }

    /// <summary>
    /// 该 completion 的内容。
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; init; }

    /// <summary>
    /// 仅适用于 deepseek-reasoner 模型。
    /// 内容为 assistant 消息中在最终答案之前的推理内容。
    /// </summary>
    [JsonPropertyName("reasoning_content")]
    public string ReasoningContent { get; init; }
}

public record Logprobs
{
    /// <summary>
    /// 一个包含输出 token 对数概率信息的列表。
    /// </summary>
    [JsonPropertyName("content")]
    public LogprobsContent[] Content { get; init; }
}

public record LogprobsContent
{
    /// <summary>
    /// 输出的 token。
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; init; }

    /// <summary>
    /// 该 token 的对数概率。-9999.0 代表该 token 的输出概率极小，不在 top 20 最可能输出的 token 中。
    /// </summary>
    [JsonPropertyName("logprobs")]
    public decimal Logprobs { get; init; }

    /// <summary>
    /// 一个包含该 token UTF-8 字节表示的整数列表。
    /// 一般在一个 UTF-8 字符被拆分成多个 token 来表示时有用。
    /// 如果 token 没有对应的字节表示，则该值为 null。
    /// </summary>
    [JsonPropertyName("bytes")]
    public int[] Bytes { get; init; }

    /// <summary>
    /// 一个包含在该输出位置上，输出概率 top N 的 token 的列表，以及它们的对数概率。
    /// 在罕见情况下，返回的 token 数量可能少于请求参数中指定的 top_logprobs 值。
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public LogprobsContent[] TopLogprobs { get; init; }
}

public record Usage
{
    /// <summary>
    /// 模型 completion 产生的 token 数。
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>
    /// 用户 prompt 所包含的 token 数。
    /// 该值等于 prompt_cache_hit_tokens + prompt_cache_miss_tokens
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>
    /// 用户 prompt 中，命中上下文缓存的 token 数。
    /// </summary>
    [JsonPropertyName("prompt_cache_hit_tokens")]
    public int PromptCacheHitTokens { get; init; }

    /// <summary>
    /// 用户 prompt 中，未命中上下文缓存的 token 数。
    /// </summary>
    [JsonPropertyName("prompt_cache_miss_tokens")]
    public int PromptCacheMissTokens { get; init; }

    /// <summary>
    /// 该请求中，所有 token 的数量（prompt + completion）。
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }

    /// <summary>
    /// completion tokens 的详细信息。
    /// </summary>
    [JsonPropertyName("completion_tokens_details")]
    public CompletionTokensDetails CompletionTokensDetails { get; init; }
}

public record CompletionTokensDetails
{
    /// <summary>
    /// 推理模型所产生的思维链 token 数量
    /// </summary>
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; init; }
}