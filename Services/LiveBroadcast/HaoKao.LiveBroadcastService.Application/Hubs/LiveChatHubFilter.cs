using Girvs.Infrastructure;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using Microsoft.AspNetCore.SignalR;

namespace HaoKao.LiveBroadcastService.Application.Hubs;

public class LiveChatHubFilter : IHubFilter
{
    public async ValueTask<object> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object>> next
    )
    {
        if (invocationContext.HubMethodName == nameof(LiveChatHub.ChatMessage))
        {
            if (invocationContext.HubMethodArguments[0] is LiveMessageInput args && string.IsNullOrEmpty(args.Content))
            {
                throw new HubException("聊天内容不能为空");
            }

            var service = EngineContext.Current.Resolve<IMutedUserService>();
            if (await service.IsMuted())
            {
                throw new HubException("用户被禁言");
            }
        }

        return await next(invocationContext);
    }

    public Task OnConnectedAsync(
        HubLifetimeContext context,
        Func<HubLifetimeContext, Task> next
    )
    {
        return next(context);
    }

    public Task OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception exception,
        Func<HubLifetimeContext, Exception, Task> next
    )
    {
        return next(context, exception);
    }
}