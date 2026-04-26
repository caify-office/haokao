using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveAnnouncement;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class LiveAnnouncementProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LiveAnnouncementProfile()
    {
        CreateMap<CreateLiveAnnouncementCommand, LiveAnnouncement>();
        CreateMap<UpdateLiveAnnouncementCommand, LiveAnnouncement>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}