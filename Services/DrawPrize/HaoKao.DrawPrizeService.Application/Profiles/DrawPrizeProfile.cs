using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Profiles;

/// <summary>
/// 抽奖模型映射文件
/// </summary>
public class DrawPrizeProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DrawPrizeProfile()
    {
        CreateMap<CreateDrawPrizeCommand, DrawPrize>();
        CreateMap<UpdateDrawPrizeCommand, DrawPrize>();
        CreateMap<SetDrawPrizeRuleCommand, DrawPrize>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}