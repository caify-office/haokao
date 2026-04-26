using Girvs.AutoMapper;
using HaoKao.CouponService.Domain.Commands.UserCoupon;
using HaoKao.CouponService.Domain.Models;

namespace HaoKao.CouponService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class UserCouponProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserCouponProfile()
    {
        CreateMap<CreateUserCouponCommand, UserCoupon>();
        CreateMap<UpdateUserCouponCommand, UserCoupon>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}