namespace HaoKao.DrawPrizeService.Domain.Commands.Prize;
/// <summary>
/// 设置奖品是否保底
/// </summary>
/// <param name="Id">主键</param>
/// <param name="DrawPrizeId">所属抽奖活动Id</param>
public record SetPrizeGuaranteedCommand(
   Guid Id,
   Guid DrawPrizeId

) : Command("设置奖品是否保底");