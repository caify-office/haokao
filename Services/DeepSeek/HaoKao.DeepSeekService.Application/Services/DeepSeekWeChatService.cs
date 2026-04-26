using HaoKao.DeepSeekService.Application.ViewModels;

namespace HaoKao.DeepSeekService.Application.Services;

public interface IDeepSeekWeChatService : IAppWebApiService, IManager
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

[DynamicWebApi(Module = ServiceAreaName.WeChat)]
[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class DeepSeekWeChatService(IDeepSeekWebService service) : IDeepSeekWeChatService
{
    /// <summary>
    /// 根据输入的上下文，来让模型补全对话内容
    /// </summary>
    /// <param name="viewModel">包含会话Id和用户输入的请求参数</param>
    /// <returns>SSE 流响应</returns>
    [HttpPost]
    public Task CompletionAsync([FromBody] ChatViewModel viewModel)
    {
        return service.CompletionAsync(viewModel);
    }

    /// <summary>
    /// 重新生成上一次的回答
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    [HttpGet]
    public Task RegenerateAsync(Guid sessionId)
    {
        return service.RegenerateAsync(sessionId);
    }

    /// <summary>
    /// 根据最近的对话内容，生成三个建议性的后续问题
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含三个建议问题字符串的列表</returns>
    [HttpGet]
    public Task<IReadOnlyList<string>> GenerateSuggestedQuestionsAsync(Guid sessionId)
    {
        return service.GenerateSuggestedQuestionsAsync(sessionId);
    }

    /// <summary>
    /// 根据会话Id获取聊天历史记录
    /// </summary>
    /// <param name="sessionId">会话Id</param>
    /// <returns>一个包含历史消息的列表（不含系统提示）</returns>
    [HttpGet]
    public Task<IReadOnlyList<ChatMessage>> GetChatHistoryAsync(Guid sessionId)
    {
        return service.GetChatHistoryAsync(sessionId);
    }
}