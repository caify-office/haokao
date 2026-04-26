using Girvs.AuthorizePermission;
using Girvs.Extensions;
using Girvs.Infrastructure;
using HaoKao.LiveBroadcastService.Application.Services.Management;
using HaoKao.LiveBroadcastService.Application.Services.Web;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveMessage;
using HaoKao.LiveBroadcastService.Application.ViewModels.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Application.Workers;
using HaoKao.LiveBroadcastService.Domain.Entities;
using HaoKao.LiveBroadcastService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HaoKao.LiveBroadcastService.Application.Hubs;

[Authorize(AuthenticationSchemes = GirvsAuthenticationScheme.GirvsIdentityServer4)]
public class LiveChatHub(
    OnlineUserState onlineUserState,
    ILiveMessageQueue queue,
    IStaticCacheManager cacheManager,
    ILiveMessageService service,
    ILiveAdministratorWebService liveAdministratorService
) : Hub<ILiveChatHub>
{
    #region 重写方法

    /// <summary>
    /// 开始连接
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        if (!Context.GetHttpContext()!.Request.Query.TryGetValue("liveId", out var id))
        {
            throw new HubException("Missing liveId");
        }

        var liveId = id.To<Guid>();

        // 直播间不存在, 抛出异常
        // if (!await _liveVideoService.IsLiveStarted(liveId)
        // {
        //     throw new HubException("Invalid Live");
        // }

        // 直播产品的权限验证
        // if (false)
        // {
        //     throw new HubException("用户没有观看直播的权限");
        // }

        await base.OnConnectedAsync();

        // 加入直播聊天室
        await Groups.AddToGroupAsync(Context.ConnectionId, id);

        // 保存在线状态
        await SaveOnlineState(liveId);

        // 保存用户在线数据
        await SaveOnlineUser(liveId);
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    /// <param name="exception"></param>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);

        // 根据连接Id查找用户
        foreach (var pair in onlineUserState.Connections.Where(p => p.Value.Contains(Context.ConnectionId)))
        {
            // 退出聊天室
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, pair.Key.LiveId.ToString());

            // 删除连接Id
            pair.Value.Remove(Context.ConnectionId);

            // 如果没有连接说明用户已经离线
            if (pair.Value.Count == 0)
            {
                var onlineUserService = EngineContext.Current.Resolve<ILiveOnlineUserService>();
                var user = await onlineUserService.GetUserByLiveId(pair.Key.LiveId);
                await onlineUserService.Update(new UpdateLiveOnlineUserViewModel { Id = user.Id, IsOnline = false, });
                onlineUserState.Connections.TryRemove(pair.Key, out _);

                // 更新在线人数
                if (onlineUserState.OnlineCount.TryGetValue(pair.Key.LiveId, out var count))
                {
                    onlineUserState.OnlineCount[pair.Key.LiveId] = --count;
                    await Clients.GroupExcept(pair.Key.LiveId.ToString(), [Context.ConnectionId,]).OnlineCount(count);
                }

                break;
            }
        }
    }

    #endregion

    #region 服务方法

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<Guid> ChatMessage(LiveMessageInput input)
    {
        var cm = EngineContext.Current.ClaimManager;
        var message = new LiveMessage
        {
            Id = Guid.NewGuid(),
            LiveId = input.LiveId.To<Guid>(),
            Content = input.Content,
            LiveMessageType = LiveMessageType.Chat,
            CreatorId = cm.GetUserId().To<Guid>(),
            CreatorName = input.UserName,
            CreateTime = DateTime.Now,
            TenantId = cm.GetTenantId().To<Guid>(),
        };

        await queue.EnqueueAsync(message);

        var isLiveAdmin = await liveAdministratorService.IsLiveAdmin();

        await Clients.Group(input.LiveId.ToString())
                     .ChatMessage(new LiveChatMessageOutput
                     {
                         Id = message.Id,
                         Content = message.Content,
                         MessageType = (int)message.LiveMessageType,
                         LiveId = message.LiveId,
                         UserId = message.CreatorId,
                         UserName = message.CreatorName,
                         SendTime = message.CreateTime,
                         IsLiveAdmin = isLiveAdmin,
                     });

        return message.Id;
    }

    public async Task MentionMessage(MentionMessageInput input)
    {
        var id = await ChatMessage(input);
        foreach (var userId in input.MentionUserIds)
        {
            if (onlineUserState.Connections.TryGetValue((userId, input.LiveId), out var connections))
            {
                await Clients.Clients(connections).MentionUser(id);
            }
        }
    }

    public async Task PinTopMessage(PinTopMessageInput input)
    {
        // 查询消息
        var message = await service.Get(input.MessageId);

        // 写入缓存, 一个直播间只有一个置顶消息
        var cacheKey = GirvsEntityCacheDefaults<LiveMessage>.ByIdCacheKey.Create(
            input.LiveId.ToString(),
            cacheTime: Math.Ceiling(input.Countdown / 60.0).To<int>()
        );

        var output = new PinTopMessageOutput
        {
            Id = message.Id,
            Content = message.Content,
            LiveId = input.LiveId,
            IsLiveAdmin = message.IsLiveAdmin,
            Duration = input.Countdown,
            Countdown = input.Countdown,
            PinTopTime = DateTime.Now,
            SystemTime = DateTime.Now,
            UserName = message.CreatorName,
        };

       await  cacheManager.SetAsync(cacheKey, output);

        await Clients.Group(input.LiveId.ToString()).PinTopMessage(output);
    }

    public async Task ShareLive(ShareLiveInput input)
    {
        var cm = EngineContext.Current.ClaimManager;
        var message = new LiveMessage
        {
            Id = Guid.NewGuid(),
            LiveId = input.LiveId.To<Guid>(),
            Content = $"{input.UserName} 刚刚分享了直播间",
            LiveMessageType = LiveMessageType.ShareLive,
            CreatorId = cm.GetUserId().To<Guid>(),
            CreatorName = input.UserName,
            CreateTime = DateTime.Now,
            TenantId = cm.GetTenantId().To<Guid>(),
        };

        // await _queue.EnqueueAsync(message);

        await Clients.Group(input.LiveId.ToString()).ShareLive(new LiveMessageOutput
        {
            Id = message.Id,
            Content = message.Content,
            MessageType = (int)message.LiveMessageType,
            LiveId = message.LiveId,
            SendTime = message.CreateTime,
        });
    }

    #endregion

    #region 私有方法

    private async Task SaveOnlineState(Guid liveId)
    {
        var userId = EngineContext.Current.ClaimManager.GetUserId().To<Guid>();

        // 在线状态, 可能多个客户端上线, 保存用户的连接Id
        if (onlineUserState.Connections.TryGetValue((userId, liveId), out var conn))
        {
            conn.Add(Context.ConnectionId);
            onlineUserState.OnlineCount.TryGetValue(liveId, out var count);
            await Clients.Group(liveId.ToString()).OnlineCount(count);
        }
        else
        {
            onlineUserState.Connections.TryAdd((userId, liveId), [Context.ConnectionId,]);

            // 更新在线人数
            if (onlineUserState.OnlineCount.TryGetValue(liveId, out var count))
            {
                onlineUserState.OnlineCount[liveId] = ++count;
            }
            else
            {
                count = new Random().Next(500, 700) + onlineUserState.Connections.Count;
                onlineUserState.OnlineCount.TryAdd(liveId, count);
            }

            await Clients.Group(liveId.ToString()).OnlineCount(count);
            await SendJoinRoom(liveId.To<Guid>());
        }
    }

    private async Task SaveOnlineUser(Guid liveId)
    {
        var onlineUserService = EngineContext.Current.Resolve<ILiveOnlineUserService>();
        var user = await onlineUserService.GetUserByLiveId(liveId);

        // 首次连接
        if (user == null)
        {
            // 添加在线记录
            Context.GetHttpContext()!.Request.Query.TryGetValue("phone", out var phone);
            await onlineUserService.Create(new CreateLiveOnlineUserViewModel
            {
                Phone = phone,
                LiveId = liveId,
            });
        }
        // 不在线
        else if (!user.IsOnline)
        {
            // 更新在线状态
            var username = Context.GetHttpContext()!.Request.Query.TryGetValue("username", out var val) ? val.ToString() : EngineContext.Current.ClaimManager.GetUserName();
            await onlineUserService.Update(new UpdateLiveOnlineUserViewModel { Id = user.Id, IsOnline = true, CreatorName = username });
        }
    }

    private async Task SendJoinRoom(Guid liveId)
    {
        var cm = EngineContext.Current.ClaimManager;
        var id = Guid.NewGuid();
        var userId = cm.GetUserId();
        var username = Context.GetHttpContext()!.Request.Query.TryGetValue("username", out var val) ? val.ToString() : cm.GetUserName();
        var tenantId = cm.GetTenantId();
        var content = $"欢迎 {username} 加入了直播间";
        var message = new LiveMessage
        {
            Id = id,
            LiveId = liveId,
            Content = content,
            LiveMessageType = LiveMessageType.JoinRoom,
            CreatorId = userId.To<Guid>(),
            CreatorName = username,
            CreateTime = DateTime.Now,
            TenantId = tenantId.To<Guid>(),
        };

        // await _queue.EnqueueAsync(message);

        await Clients.Group(liveId.ToString()).JoinRoom(new LiveMessageOutput
        {
            Id = message.Id,
            Content = message.Content,
            MessageType = (int)message.LiveMessageType,
            LiveId = message.LiveId,
            SendTime = message.CreateTime,
        });
    }

    #endregion
}