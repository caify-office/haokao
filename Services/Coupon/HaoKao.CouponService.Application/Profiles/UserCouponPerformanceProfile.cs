using Girvs.AutoMapper;
using HaoKao.CouponService.Domain.Commands.UserCouponPerformance;
using HaoKao.CouponService.Domain.Models;

namespace HaoKao.CouponService.Application.Profiles;

/// <summary>
/// 模型映射文件
/// </summary>
public class UserCouponPerformanceProfile : Profile, IOrderedMapperProfile
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserCouponPerformanceProfile()
    {
        CreateMap<CreateUserCouponPerformanceCommand, UserCouponPerformance>();
        CreateMap<UpdateUserCouponPerformanceCommand, UserCouponPerformance>();
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 201;
}