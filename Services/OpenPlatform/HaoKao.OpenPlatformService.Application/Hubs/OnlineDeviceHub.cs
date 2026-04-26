using System.Collections.Concurrent;
using Girvs.AuthorizePermission;
using HaoKao.OpenPlatformService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HaoKao.OpenPlatformService.Application.Hubs;

[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class OnlineDeviceHub(OnlineDeviceState cache, IPersistedGrantRepository repository) : Hub<IOnlineDeviceHub>
{
    /// <summary>
    /// 开始连接
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var userId = EngineContext.Current.ClaimManager.GetUserId();
        var clientId = httpContext.Request.Query["client_id"];
        var sessionId = httpContext.Request.Query["idsrv.session"];

        // 按客户端分组
        await Groups.AddToGroupAsync(Context.ConnectionId, clientId);

        // 添加连接
        cache.Connections.AddOrUpdate(sessionId, [Context.ConnectionId], (_, connections) =>
        {
            connections.Add(Context.ConnectionId);
            return connections;
        });

        await base.OnConnectedAsync();

        // 根据用户当前的clientId和sessionId判断是否存在多个设备登录, 如果存在则通知前端, 保留最新的连接
        var grants = await repository.GetBySubjectIdAndClientId(userId, clientId);
        if (grants.Count > 1)
        {
            var allowedGrant = grants.OrderByDescending(x => x.CreationTime).First();
            var allowedSesstionId = allowedGrant.SessionId;
            // 通过 sessionId 获取连接
            if (cache.Connections.TryGetValue(allowedSesstionId, out var connectionIds))
            {
                await Clients.GroupExcept(clientId, connectionIds).NotifyMultiDeviceLogin();
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var httpContext = Context.GetHttpContext();
        var sessionId = httpContext.Request.Query["idsrv.session"];
        var clientId = httpContext.Request.Query["client_id"];

        foreach (var pair in cache.Connections.Where(p => p.Value.Contains(Context.ConnectionId)))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, clientId);

            // 删除连接Id
            pair.Value.Remove(Context.ConnectionId);

            // 如果没有连接说明用户已经离线
            if (pair.Value.Count == 0)
            {
                cache.Connections.TryRemove(pair.Key, out _);
                break;
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}

public class OnlineDeviceState
{
    /// <summary>
    /// 按 sessionId 分组
    /// key: sessionId, value: 连接Id集合
    /// </summary>
    public ConcurrentDictionary<string, HashSet<string>> Connections => new();
}