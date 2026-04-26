using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveOnlineUser;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 直播在线用户模型映射文件
/// </summary>
public class LiveOnlineUserProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LiveOnlineUserProfile()
    {
        CreateMap<CreateLiveOnlineUserCommand, LiveOnlineUser>();
        CreateMap<UpdateLiveOnlineUserCommand, LiveOnlineUser>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}