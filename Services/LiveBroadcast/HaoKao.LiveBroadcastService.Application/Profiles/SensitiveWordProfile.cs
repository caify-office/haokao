using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.LiveBroadcastService.Domain.Entities;

namespace HaoKao.LiveBroadcastService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class SensitiveWordProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SensitiveWordProfile()
    {
        CreateMap<CreateSensitiveWordCommand, SensitiveWord>();
        CreateMap<UpdateSensitiveWordCommand, SensitiveWord>();
    }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}