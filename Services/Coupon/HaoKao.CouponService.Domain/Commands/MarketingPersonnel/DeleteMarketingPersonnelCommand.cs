using System;

namespace HaoKao.CouponService.Domain.Commands.MarketingPersonnel;

/// <summary>
/// 删除命令
/// </summary>
/// <param name="Id">主键</param>
public record DeleteMarketingPersonnelCommand(
    Guid Id
) : Command("删除");