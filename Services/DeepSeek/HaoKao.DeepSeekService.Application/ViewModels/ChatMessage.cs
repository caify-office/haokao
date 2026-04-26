namespace HaoKao.DeepSeekService.Application.ViewModels;

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

    private readonly string _role;

    /// <summary>
    /// 消息的内容。
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; init; }

    /// <summary>
    /// 可以选填的参与者的名称，为模型提供信息以区分相同角色的参与者。
    /// </summary>
    [JsonPropertyName("name"), JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public string Name { get; init; }

    /// <summary>
    /// 创建一个用户角色的消息
    /// </summary>
    /// <param name="content">消息内容</param>
    /// <returns>一个新的ChatMessage实例</returns>
    public static ChatMessage FromUser(string content) => new() { Role = ChatRole.User, Content = content };

    /// <summary>
    /// 创建一个助手角色的消息
    /// </summary>
    /// <param name="content">消息内容</param>
    /// <returns>一个新的ChatMessage实例</returns>
    public static ChatMessage FromAssistant(string content) => new() { Role = ChatRole.Assistant, Content = content };

    /// <summary>
    /// 创建一个系统角色的消息
    /// </summary>
    /// <param name="content">消息内容</param>
    /// <returns>一个新的ChatMessage实例</returns>
    public static ChatMessage FromSystem(string content) => new() { Role = ChatRole.System, Content = content };
}

public record ChatViewModel : IDto
{
    /// <summary>
    /// 会话Id
    /// </summary>
    [Required]
    public Guid ChatSessionId { get; init; }

    /// <summary>
    /// 用户输入
    /// </summary>
    [Required, MaxLength(500)]
    public string Prompt { get; init; }
}