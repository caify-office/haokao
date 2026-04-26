using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Profiles;

/// <summary>
/// 奖品模型映射文件
/// </summary>
public class PrizeProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PrizeProfile()
    {
        CreateMap<CreatePrizeCommand, Prize>();
        CreateMap<UpdatePrizeCommand, Prize>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}