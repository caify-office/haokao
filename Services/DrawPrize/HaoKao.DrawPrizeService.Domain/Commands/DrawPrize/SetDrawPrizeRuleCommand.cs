namespace HaoKao.DrawPrizeService.Domain.Commands.DrawPrize;
/// <summary>
/// 设置抽奖规则
/// </summary>
/// <param name="Id">主键</param>
/// <param name="DrawPrizeRange">抽奖范围</param>
/// <param name="DrawPrizeType">类型</param>
/// <param name="Probability">中奖概率</param>
public record SetDrawPrizeRuleCommand(
   Guid Id,
   DrawPrizeRange DrawPrizeRange,
   DrawPrizeType DrawPrizeType,
   Double Probability

) : Command("设置抽奖规则");