using System;
using HaoKao.CouponService.Domain.Enumerations;

namespace HaoKao.CouponService.Domain.Commands.UserCoupon;

/// <summary>
/// 用户领取优惠券命令
/// </summary>
/// <param name="CouponId">优惠券id</param>
/// <param name="NickName">昵称</param>
/// <param name="ChannelType"></param>
public record CreateUserCouponEventCommand(
    Guid CouponId,
    string NickName,
    ChannelType ChannelType
) : Command("用户领取优惠券命令");