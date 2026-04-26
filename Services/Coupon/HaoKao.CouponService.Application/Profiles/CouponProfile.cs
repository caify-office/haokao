using Girvs.AutoMapper;
using HaoKao.CouponService.Application.ViewModels.Coupon;
using HaoKao.CouponService.Domain.Commands.Coupon;
using HaoKao.CouponService.Domain.Models;

namespace HaoKao.CouponService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class CouponProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CouponProfile()
    {
        CreateMap<Coupon, BrowseCouponViewModel>();
        CreateMap<CreateCouponCommand, Coupon>();
        CreateMap<UpdateCouponCommand, Coupon>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}