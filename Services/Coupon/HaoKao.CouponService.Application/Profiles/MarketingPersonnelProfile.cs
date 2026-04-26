using Girvs.AutoMapper;
using HaoKao.CouponService.Domain.Commands.MarketingPersonnel;
using HaoKao.CouponService.Domain.Models;

namespace HaoKao.CouponService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class MarketingPersonnelProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public MarketingPersonnelProfile()
    {
        CreateMap<CreateMarketingPersonnelCommand, MarketingPersonnel>();
        CreateMap<UpdateMarketingPersonnelCommand, MarketingPersonnel>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}