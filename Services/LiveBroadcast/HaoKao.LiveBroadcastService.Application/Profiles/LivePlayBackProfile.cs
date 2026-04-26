using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.LivePlayBack;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class LivePlayBackProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LivePlayBackProfile()
    {
        CreateMap<CreateLivePlayBackModel, LivePlayBack>();
        CreateMap<UpdateLivePlayBackModel, LivePlayBack>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}