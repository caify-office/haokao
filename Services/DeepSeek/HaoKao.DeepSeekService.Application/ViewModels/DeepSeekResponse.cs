namespace HaoKao.DeepSeekService.Application.ViewModels;

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