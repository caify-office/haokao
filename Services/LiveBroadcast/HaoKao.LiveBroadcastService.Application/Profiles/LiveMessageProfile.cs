using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveMessage;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 直播消息模型映射文件
/// </summary>
public class LiveMessageProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LiveMessageProfile()
    {
        CreateMap<CreateLiveMessageCommand, LiveMessage>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}