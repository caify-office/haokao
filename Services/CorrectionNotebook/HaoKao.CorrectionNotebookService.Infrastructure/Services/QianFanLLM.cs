using Girvs.Cache.Caching;
using Girvs.Extensions;
using HaoKao.CorrectionNotebookService.Domain.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace HaoKao.CorrectionNotebookService.Infrastructure.Services;

public record QianFanLLMOptions : IAppModuleConfig
{
    /// <summary>
    /// API Key
    /// <see cref="https://console.bce.baidu.com/qianfan/ais/console/applicationConsole/application"/>
    /// </summary>
    public string ApiKey { get; init; } = "gz7oEcnvI85quAFm21Ehuh9s";

    public string SecretKey { get; init; } = "k5Py32RhIzz3DRP14iuqDBKkUITMpD0R";

    public string ModelName { get; init; } = "ERNIE-3.5-8K";

    public Uri Endpoint { get; init; } = new("https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions");

    public void Init() { }
}

/// <summary>
/// 千帆大模型(文心一言)
/// <see cref="https://console.bce.baidu.com/qianfan/overview"/>
/// </summary>
public class QianFanLLM(IStaticCacheManager cacheManager) : ILargeLanguageModel
{
    private readonly QianFanLLMOptions _options = Singleton<AppSettings>.Instance.Get<QianFanLLMOptions>();

    private readonly IStaticCacheManager _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));

    public Guid Id => new("6767b304-a2ce-4f04-a3f2-9a7f86c0b001");

    public string Name => _options.ModelName;

    public async Task<string> CompletionAsync(string content)
    {
        using var client = HttpClientFactory.Create();
        using var request = await CreateRequestAsync(content, false);
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            var data = await response.Content.ReadAsAsync<QianFanResponse>();
            return data.Result;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }

    public async IAsyncEnumerable<string> CompletionStreamAsync(string content)
    {
        using var client = HttpClientFactory.Create();
        using var request = await CreateRequestAsync(content, true);
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

    private async Task<HttpRequestMessage> CreateRequestAsync(string content, bool stream)
    {
        var accessToken = await GetAccessTokenFromCache();
        var url = QueryHelpers.AddQueryString(_options.Endpoint.ToString(), "access_token", accessToken);
        return new(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(new QianFanRequestBody
            {
                Stream = stream,
                Messages = [
                    new Message { Role = "user", Content = content }
                ]
            }), Encoding.UTF8, "application/json")
        };
    }

    private Task<string> GetAccessTokenFromCache()
    {
        var prefix = $"correctionnotebookservice:{nameof(QianFanLLM).ToLower()}:{JsonSerializer.Serialize(_options).ToMd5()}";
        var cacheKey = new CacheKey(prefix).Create(cacheTime: TimeSpan.FromDays(29).Seconds);
        return _cacheManager.GetAsync(cacheKey, GetAccessTokenAsync);
    }

    /// <summary>
    /// 获取AccessToken
    /// <see cref="https://cloud.baidu.com/doc/WENXINWORKSHOP/s/Ilkkrb0i5"/>
    /// </summary>
    /// <remarks>access_token默认有效期30天，单位是秒，生产环境注意及时刷新</remarks>
    /// <returns></returns>
    private async Task<string> GetAccessTokenAsync()
    {
        var dict = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _options.ApiKey,
            ["client_secret"] = _options.SecretKey
        };
        var url = QueryHelpers.AddQueryString("https://aip.baidubce.com/oauth/2.0/token", dict);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        using var client = HttpClientFactory.Create();
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        try
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<AccessTokenResponse>(stream);
            return data.AccessToken;
        }
        catch
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new GirvsException(message);
        }
    }
}

file record AccessTokenResponse
{
    /// <summary>
    /// 访问凭证
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }

    /// <summary>
    /// 有效期，Access Token的有效期
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; init; }

    /// <summary>
    /// 错误码
    /// </summary>
    [JsonPropertyName("error_code")]
    public string Error { get; init; }

    /// <summary>
    /// 错误描述信息，帮助理解和解决发生的错误
    /// </summary>
    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; init; }

    /// <summary>
    /// 暂时未使用，可忽略
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; }

    /// <summary>
    /// 暂时未使用，可忽略
    /// </summary>
    [JsonPropertyName("session_key")]
    public string SessionKey { get; init; }

    /// <summary>
    /// 暂时未使用，可忽略
    /// </summary>
    [JsonPropertyName("session_secret")]
    public string SessionSecret { get; init; }

    /// <summary>
    /// 暂时未使用，可忽略
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; init; }
}

file record QianFanRequestBody
{
    /// <summary>
    /// 聊天上下文信息
    /// </summary>
    [JsonPropertyName("messages")]
    public Message[] Messages { get; init; }

    /// <summary>
    /// 是否以流式接口的形式返回数据，默认false
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; init; } = false;

    /// <summary>
    /// 较高的数值会使输出更加随机，而较低的数值会使其更加集中和确定
    /// 默认0.95，范围 [0, 1.0]，不能为0
    /// </summary>
    [JsonPropertyName("temperature")]
    public float Temperature
    {
        get => _temperature;
        init
        {
            if (value <= 0 || value > 1.0)
            {
                throw new ArgumentException("temperature must be in [0, 1.0], and cannot be 0");
            }
            _temperature = value;
        }
    }
    private float _temperature = 0.95f;

    /// <summary>
    /// 影响输出文本的多样性，取值越大，生成文本的多样性越强
    /// 默认0.7，取值范围 [0, 1.0]
    /// </summary>
    [JsonPropertyName("top_p")]
    public float TopP
    {
        get => _topP;
        init
        {
            if (value < 0 || value > 1.0)
            {
                throw new ArgumentException("top_p must be in [0, 1.0]");
            }
            _topP = value;
        }
    }
    private float _topP = 0.7f;

    /// <summary>
    /// 通过对已生成的token增加惩罚，减少重复生成的现象
    /// 值越大表示惩罚越大
    /// 默认1.0，取值范围：[1.0, 2.0]
    /// </summary>
    [JsonPropertyName("penalty_score")]
    public float PenaltyScore
    {
        get => _penaltyScore;
        init
        {
            if (value < 1.0 || value > 2.0)
            {
                throw new ArgumentException("penalty_score must be in [1.0, 2.0]");
            }
            _penaltyScore = value;
        }
    }
    private float _penaltyScore = 1.0f;

    /// <summary>
    /// 模型人设，主要用于人设设定
    /// </summary>
    [JsonPropertyName("system")]
    public string System { get; init; }

    /// <summary>
    /// 生成停止标识，当模型生成结果以stop中某个元素结尾时，停止文本生成
    /// 每个元素长度不超过20字符，最多4个元素
    /// </summary>
    [JsonPropertyName("stop")]
    public string[] Stop { get; init; }

    /// <summary>
    /// 指定模型最小输出token数
    /// 该参数取值范围[2, 2048]
    /// </summary>
    [JsonPropertyName("min_output_tokens")]
    public int? MinOutputTokens
    {
        get => _minOutputTokens;
        init
        {
            if (value.HasValue && (value < 2 || value > 2048))
            {
                throw new ArgumentException("min_output_tokens must be in [2, 2048]");
            }
            _minOutputTokens = value;
        }
    }
    private int? _minOutputTokens;

    /// <summary>
    /// 指定模型最大输出token数
    /// 如果设置此参数，范围[2, 2048]
    /// 如果不设置此参数，最大输出token数为1024
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public int MaxOutputTokens
    {
        get => _maxOutputTokens;
        init
        {
            if (value < 2 || value > 2048)
            {
                throw new ArgumentException("max_output_tokens must be in [2, 2048]");
            }
            _maxOutputTokens = value;
        }
    }
    private int _maxOutputTokens = 1024;

    /// <summary>
    /// 正值根据迄今为止文本中的现有频率对新token进行惩罚，从而降低模型逐字重复同一行的可能性
    /// 默认0.1，取值范围[-2.0, 2.0]
    /// </summary>
    [JsonPropertyName("frequency_penalty")]
    public float FrequencyPenalty
    {
        get => _frequencyPenalty;
        init
        {
            if (value < -2.0 || value > 2.0)
            {
                throw new ArgumentException("frequency_penalty must be in [-2.0, 2.0]");
            }
            _frequencyPenalty = value;
        }
    }
    private float _frequencyPenalty = 0.1f;

    /// <summary>
    /// 正值根据token记目前是否出现在文本中来对其进行惩罚，从而增加模型谈论新主题的可能性
    /// 默认0.0，取值范围[-2.0, 2.0]
    /// </summary>
    [JsonPropertyName("presence_penalty")]
    public float PresencePenalty
    {
        get => _presencePenalty;
        init
        {
            if (value < -2.0 || value > 2.0)
            {
                throw new ArgumentException("presence_penalty must be in [-2.0, 2.0]");
            }
            _presencePenalty = value;
        }
    }
    private float _presencePenalty = 0.0f;

    /// <summary>
    /// 表示最终用户的唯一标识符
    /// </summary>
    [JsonPropertyName("user_id")]
    public string UserId { get; init; }
}

file record Message
{
    /// <summary>
    /// 对话角色: user 表示用户，assistant 表示对话助手
    /// </summary>
    [JsonPropertyName("role")]
    public string Role
    {
        get => _role;
        init
        {
            if (value != "user" && value != "assistant")
            {
                throw new ArgumentException("role must be user or assistant");
            }
            _role = value;
        }
    }
    private string _role;

    /// <summary>
    /// 对话内容, 不能为空
    /// </summary>
    [JsonPropertyName("content")]
    public string Content
    {
        get => _content;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("content must not be empty");
            }
            _content = value;
        }
    }
    private string _content;
}

file record QianFanResponse
{
    /// <summary>
    /// 本轮对话的id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// 回包类型 chat.completion：多轮对话返回
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; init; }

    /// <summary>
    /// 时间戳
    /// </summary>
    [JsonPropertyName("created")]
    public int Created { get; init; }

    /// <summary>
    /// 表示当前子句的序号。只有在流式接口模式下会返回该字段
    /// </summary>
    [JsonPropertyName("sentence_id")]
    public int SentenceId { get; init; }

    /// <summary>
    /// 表示当前子句是否是最后一句。只有在流式接口模式下会返回该字段
    /// </summary>
    [JsonPropertyName("is_end")]
    public bool IsEnd { get; init; }

    /// <summary>
    /// 当前生成的结果是否被截断
    /// </summary>
    [JsonPropertyName("is_truncated")]
    public bool IsTruncated { get; init; }

    /// <summary>
    /// 对话返回结果
    /// </summary>
    [JsonPropertyName("result")]
    public string Result { get; init; }

    /// <summary>
    /// 表示用户输入是否存在安全风险，是否关闭当前会话，清理历史会话信息。
    /// true：是，表示用户输入存在安全风险，建议关闭当前会话，清理历史会话信息。
    /// false：否，表示用户输入无安全风险
    /// </summary>
    [JsonPropertyName("need_clear_history")]
    public bool NeedClearHistory { get; init; }

    /// <summary>
    /// 当need_clear_history为true时，此字段会告知第几轮对话有敏感信息，如果是当前问题，ban_round=-1
    /// </summary>
    [JsonPropertyName("ban_round")]
    public int BanRound { get; init; }

    /// <summary>
    /// token统计信息
    /// </summary>
    [JsonPropertyName("usage")]
    public Usage Usage { get; init; }
}

file record Usage
{
    /// <summary>
    /// 问题tokens数
    /// </summary>
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; init; }

    /// <summary>
    /// 回答tokens数
    /// </summary>
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; init; }

    /// <summary>
    /// tokens总数
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; init; }
}