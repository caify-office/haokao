namespace HaoKao.DrawPrizeService.Domain.Commands.Prize;
/// <summary>
/// 取消奖品保底
/// </summary>
/// <param name="Id">主键</param>
public record CancelPrizeGuaranteedCommand(
   Guid Id) : Command("取消奖品保底");