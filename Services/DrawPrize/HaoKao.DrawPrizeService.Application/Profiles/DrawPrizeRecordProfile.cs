using AutoMapper;
using Girvs.AutoMapper;
using HaoKao.DrawPrizeService.Domain.Entities;

namespace HaoKao.DrawPrizeService.Application.Profiles;

/// <summary>
/// 抽奖记录映射文件
/// </summary>
public class DrawPrizeRecordProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DrawPrizeRecordProfile()
    {
        CreateMap<CreateDrawPrizeRecordCommand, DrawPrizeRecord>();
        CreateMap<UpdateDrawPrizeRecordCommand, DrawPrizeRecord>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}