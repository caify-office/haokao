using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LiveComment;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 直播评论模型映射文件
/// </summary>
public class LiveCommentProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LiveCommentProfile()
    {
        CreateMap<CreateLiveCommentCommand, LiveComment>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}