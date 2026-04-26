using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Commands.MutedUser;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class MutedUserProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public MutedUserProfile()
    {
        CreateMap<CreateMutedUserCommand, MutedUser>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}