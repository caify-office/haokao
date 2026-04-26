namespace HaoKao.DeepSeekService.Application.ViewModels;

public record DeepSeekRequest
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

    private readonly string _model;

    /// <summary>
    /// 对话的消息列表。
    /// </summary>
    [JsonPropertyName("messages")]
    public ChatMessage[] Messages { get; set; }

    /// <summary>
    /// Possible values: >= -2 and 小于等于 2
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
            if (value is < -2.0m or > 2.0m)
            {
                throw new ArgumentOutOfRangeException(nameof(FrequencyPenalty), "The value must be between -2.0 and 2.0.");
            }
            _frequencyPenalty = value;
        }
    }

    private readonly decimal? _frequencyPenalty = 0;

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
            if (value is < 1 or > 8192)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxTokens), "The value must be between 1 and 8192.");
            }

            _maxTokens = value;
        }
    }

    private readonly int _maxTokens = 4096;

    /// <summary>
    /// Possible values: >= -2 and 小于等于 2
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
            if (value is < -2 or > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(PresencePenalty), "The value must be between -2 and 2.");
            }
            _presencePenalty = value;
        }
    }

    private readonly decimal? _presencePenalty = 0;

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

    private readonly string[] _stop;

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
    /// Possible values: 小于等于 2
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
            if (value is < 0 or > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(Temperature), "The value must be between 0 and 2.");
            }
            _temperature = value;
        }
    }

    private readonly decimal? _temperature = 1;

    /// <summary>
    /// Possible values: 小于等于 1
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
            if (value is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(TopP), "The value must be between 0 and 1.");
            }
            _topP = value;
        }
    }

    private readonly decimal? _topP = 1;

    /// <summary>
    /// 是否返回所输出 token 的对数概率。
    /// 如果为 true，则在 message 的 content 中返回每个输出 token 的对数概率。
    /// </summary>
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; init; }

    /// <summary>
    /// Possible values: 小于等于 20
    /// 一个介于 0 到 20 之间的整数 N，指定每个输出位置返回输出概率 top N 的 token，且返回这些 token 的对数概率。
    /// 指定此参数时，logprobs 必须为 true。
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs
    {
        get => _topLogprobs;
        init
        {
            if (value is > 20 or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(TopLogprobs), "The value must be between 0 and 20.");
            }
            _topLogprobs = value;
        }
    }

    private readonly int? _topLogprobs;
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

    private readonly string _type = "text";
}

public record StreamOption
{
    /// <summary>
    /// 如果设置为 true，在流式消息最后的 data: [DONE] 之前将会传输一个额外的块。
    /// 此块上的 usage 字段显示整个请求的 token 使用统计信息，而 choices 字段将始终是一个空数组。
    /// 所有其他块也将包含一个 usage 字段，但其值为 null。
    /// </summary>
    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; init; }
}